using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;

namespace RunningObjects.MVC.Query
{
    internal class ClassFactory
    {
        public static readonly ClassFactory Instance = new ClassFactory();

        static ClassFactory() { }

        ModuleBuilder module;
        Dictionary<Signature, Type> classes;
        int classCount;
        ReaderWriterLock rwLock;

        private ClassFactory()
        {
            AssemblyName name = new AssemblyName("RunningObjects.MVC.Query.DynamicClasses");
            AssemblyBuilder assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
#if ENABLE_LINQ_PARTIAL_TRUST
            new ReflectionPermission(PermissionState.Unrestricted).Assert();
#endif
            try
            {
                module = assembly.DefineDynamicModule("Module");
            }
            finally
            {
#if ENABLE_LINQ_PARTIAL_TRUST
                PermissionSet.RevertAssert();
#endif
            }
            classes = new Dictionary<Signature, Type>();
            rwLock = new ReaderWriterLock();
        }

        public Type GetDynamicClass(Type referenceType)
        {
            var properties = new List<DynamicProperty>();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(referenceType))
            {
                var ignoredAttrTypes = new[]
                {
                    typeof (CLSCompliantAttribute),
                    typeof (DefaultMemberAttribute),
                    typeof (SerializableAttribute),
                    typeof (ComVisibleAttribute)
                };
                var attrs = pd.Attributes.Cast<Attribute>().Where(attr => !ignoredAttrTypes.Contains(attr.GetType()));
                properties.Add(new DynamicProperty(pd.Name, pd.PropertyType, attrs.ToArray()));
            }
            return GetDynamicClass(referenceType, properties);
        }


        public Type GetDynamicClass(Type referenceType, IEnumerable<DynamicProperty> properties)
        {
            rwLock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                Signature signature = new Signature(properties);
                Type type;
                if (!classes.TryGetValue(signature, out type))
                {
                    type = CreateDynamicClass(referenceType, signature.properties);
                    classes.Add(signature, type);
                }
                return type;
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
        }

        Type CreateDynamicClass(Type type, DynamicProperty[] properties)
        {
            LockCookie cookie = rwLock.UpgradeToWriterLock(Timeout.Infinite);
            try
            {
                string typeName = string.Format("{0}_{1}", type.Name, Guid.NewGuid().ToString("N"));
#if ENABLE_LINQ_PARTIAL_TRUST
                new ReflectionPermission(PermissionState.Unrestricted).Assert();
#endif
                try
                {
                    //Type exists = module.GetType(typeName);
                    //if (exists != null)
                    //    return exists;

                    TypeBuilder tb = module.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public, typeof(DynamicClass));

                    GenerateTypeAttributes(tb, type);
                    GenerateFields(tb, type);
                    GenerateConstructorSignatures(tb, type);
                    GenerateMethods(tb, type);
                    var fields = GenerateProperties(tb, properties);
                    GenerateEquals(tb, fields);
                    GenerateGetHashCode(tb, fields);
                    var result = tb.CreateType();
                    classCount++;

                    return result;
                }
                finally
                {
#if ENABLE_LINQ_PARTIAL_TRUST
                    PermissionSet.RevertAssert();
#endif
                }
            }
            finally
            {
                rwLock.DowngradeFromWriterLock(ref cookie);
            }
        }

        private static void GenerateTypeAttributes(TypeBuilder tb, Type type)
        {
            var attributes = type.GetCustomAttributes(false).OfType<Attribute>();
            foreach (var attribute in attributes)
            {
                var cab = CreateAttributeBuilder(attribute);
                if (cab != null)
                    tb.SetCustomAttribute(cab);
            }
        }

        private static void GenerateConstructorSignatures(TypeBuilder tb, Type type)
        {
            var defaultCtor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
            var defaultCtorGen = defaultCtor.GetILGenerator();
            defaultCtorGen.Emit(OpCodes.Ret);

            var constructors = type.GetConstructors().Where(c => c.GetParameters().Any());
            foreach (var constructor in constructors)
            {
                var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType).ToArray();
                ConstructorBuilder builder = tb.DefineConstructor(constructor.Attributes, constructor.CallingConvention, parameterTypes);

                foreach (var attr in constructor.GetCustomAttributes(false).OfType<Attribute>())
                {
                    var cab = CreateAttributeBuilder(attr);
                    if (cab != null)
                        builder.SetCustomAttribute(cab);
                }

                var generator = builder.GetILGenerator();
                generator.Emit(OpCodes.Ret);
            }
        }

        private static void GenerateFields(TypeBuilder tb, Type type)
        {
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (var field in fields)
            {
                var builder = tb.DefineField(field.Name, field.FieldType, field.Attributes);
                foreach (var attr in field.GetCustomAttributes(false).OfType<Attribute>())
                {
                    var cab = CreateAttributeBuilder(attr);
                    if (cab != null)
                        builder.SetCustomAttribute(cab);
                }
            }
        }

        private void GenerateMethods(TypeBuilder typeBuilder, Type type)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
            var methodsToCopy = type.GetMethods(flags).Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"));

            foreach (var method in methodsToCopy)
            {
                var attrs = MethodAttributes.ReuseSlot | MethodAttributes.HideBySig;
                if (method.IsPublic) attrs |= MethodAttributes.Public;
                if (method.IsPrivate) attrs |= MethodAttributes.Private;
                if (method.IsVirtual) attrs |= MethodAttributes.Virtual;
                if (method.IsStatic) attrs |= MethodAttributes.Static;

                var builder = typeBuilder.DefineMethod(method.Name, attrs, CallingConventions.Standard, method.ReturnType, method.GetParameters().Select(p => p.ParameterType).ToArray());

                var methodIL = method.GetMethodBody().GetILAsByteArray();
                builder.CreateMethodBody(methodIL, methodIL.Length);
            }
        }

        FieldInfo[] GenerateProperties(TypeBuilder tb, DynamicProperty[] properties)
        {
            FieldInfo[] fields = new FieldBuilder[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                DynamicProperty dp = properties[i];
                FieldBuilder fb = tb.DefineField("_" + dp.Name, dp.Type, FieldAttributes.Private);

                PropertyBuilder pb = tb.DefineProperty(dp.Name, PropertyAttributes.HasDefault, dp.Type, null);

                foreach (var attr in dp.Attributes)
                {
                    var cb = CreateAttributeBuilder(attr);
                    if (cb != null)
                        pb.SetCustomAttribute(cb);
                }

                MethodBuilder mbGet = tb.DefineMethod("get_" + dp.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, dp.Type, Type.EmptyTypes);
                ILGenerator genGet = mbGet.GetILGenerator();
                genGet.Emit(OpCodes.Ldarg_0);
                genGet.Emit(OpCodes.Ldfld, fb);
                genGet.Emit(OpCodes.Ret);

                MethodBuilder mbSet = tb.DefineMethod("set_" + dp.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { dp.Type });
                ILGenerator genSet = mbSet.GetILGenerator();
                genSet.Emit(OpCodes.Ldarg_0);
                genSet.Emit(OpCodes.Ldarg_1);
                genSet.Emit(OpCodes.Stfld, fb);
                genSet.Emit(OpCodes.Ret);
                pb.SetGetMethod(mbGet);
                pb.SetSetMethod(mbSet);
                fields[i] = fb;
            }
            return fields;
        }

        private static CustomAttributeBuilder CreateAttributeBuilder(Attribute attr)
        {

            var catype = attr.GetType();
            var cactor = catype.GetConstructor(Type.EmptyTypes);
            if (cactor != null)
            {
                var caFields = catype.GetFields(BindingFlags.Public | BindingFlags.NonPublic);
                var caFieldValues = new List<object>();
                foreach (var caField in caFields)
                    caFieldValues.Add(caField.GetValue(attr));

                var caProperties = new List<PropertyInfo>(catype.GetProperties());
                var caPropertyValues = new List<object>();
                caProperties.RemoveAll(p => !p.CanWrite);
                foreach (PropertyDescriptor propDesc in TypeDescriptor.GetProperties(catype))
                {
                    if (!propDesc.IsReadOnly)
                    {
                        object capv;
                        if (typeof(DisplayAttribute) == catype)
                        {
                            var getMethod = catype.GetMethod(string.Format("Get{0}", propDesc.Name));
                            capv = getMethod != null ? getMethod.Invoke(attr, null) : propDesc.GetValue(attr);
                        }
                        else
                            capv = propDesc.GetValue(attr);
                        caPropertyValues.Add(capv != null
                                                 ? Convert.ChangeType(capv, propDesc.PropertyType)
                                                 : GetDefault(propDesc.PropertyType));
                    }
                }
                try
                {
                    return new CustomAttributeBuilder(cactor, Type.EmptyTypes, caProperties.ToArray(), caPropertyValues.ToArray(), caFields, caFieldValues.ToArray());
                }
                catch
                { }
            }
            return null;
        }

        public static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        void GenerateEquals(TypeBuilder tb, FieldInfo[] fields)
        {
            MethodBuilder mb = tb.DefineMethod("Equals",
                                               MethodAttributes.Public | MethodAttributes.ReuseSlot |
                                               MethodAttributes.Virtual | MethodAttributes.HideBySig,
                                               typeof(bool), new[] { typeof(object) });
            ILGenerator gen = mb.GetILGenerator();
            LocalBuilder other = gen.DeclareLocal(tb);
            Label next = gen.DefineLabel();
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Isinst, tb);
            gen.Emit(OpCodes.Stloc, other);
            gen.Emit(OpCodes.Ldloc, other);
            gen.Emit(OpCodes.Brtrue_S, next);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ret);
            gen.MarkLabel(next);
            foreach (FieldInfo field in fields)
            {
                Type ft = field.FieldType;
                Type ct = typeof(EqualityComparer<>).MakeGenericType(ft);
                next = gen.DefineLabel();
                gen.EmitCall(OpCodes.Call, ct.GetMethod("get_Default"), null);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
                gen.Emit(OpCodes.Ldloc, other);
                gen.Emit(OpCodes.Ldfld, field);
                gen.EmitCall(OpCodes.Callvirt, ct.GetMethod("Equals", new[] { ft, ft }), null);
                gen.Emit(OpCodes.Brtrue_S, next);
                gen.Emit(OpCodes.Ldc_I4_0);
                gen.Emit(OpCodes.Ret);
                gen.MarkLabel(next);
            }
            gen.Emit(OpCodes.Ldc_I4_1);
            gen.Emit(OpCodes.Ret);
        }

        void GenerateGetHashCode(TypeBuilder tb, FieldInfo[] fields)
        {
            MethodBuilder mb = tb.DefineMethod("GetHashCode",
                                               MethodAttributes.Public | MethodAttributes.ReuseSlot |
                                               MethodAttributes.Virtual | MethodAttributes.HideBySig,
                                               typeof(int), Type.EmptyTypes);
            ILGenerator gen = mb.GetILGenerator();
            gen.Emit(OpCodes.Ldc_I4_0);
            foreach (FieldInfo field in fields)
            {
                Type ft = field.FieldType;
                Type ct = typeof(EqualityComparer<>).MakeGenericType(ft);
                gen.EmitCall(OpCodes.Call, ct.GetMethod("get_Default"), null);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
                gen.EmitCall(OpCodes.Callvirt, ct.GetMethod("GetHashCode", new[] { ft }), null);
                gen.Emit(OpCodes.Xor);
            }
            gen.Emit(OpCodes.Ret);
        }
    }
}