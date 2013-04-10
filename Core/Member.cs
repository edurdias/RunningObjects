using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RunningObjects.Core.Mapping;

namespace RunningObjects.Core
{
    public abstract class Member
    {
        private Model underliningModel;

        protected Member(Type memberType)
        {
            MemberType = memberType;
        }

        public Type MemberType { get; private set; }

        public Model Model { get; protected set; }
        public Model UnderliningModel
        {
            get
            {
                if (IsModel || IsModelCollection)
                {
                    if (underliningModel == null)
                    {
                        var type = IsModel ? MemberType : GetElementType(MemberType);
                        var mapping = ModelMappingManager.MappingFor(type);
                        var descriptor = new ModelDescriptor(mapping);
                        underliningModel = new Model(type, descriptor, Value);
                    }
                    return underliningModel;
                }
                return null;
            }
        }

        public abstract string Name { get; }
        public abstract object Value { get; set; }
        public abstract IEnumerable<Attribute> Attributes { get; }

        public ModelMetadata Metadata
        {
            get
            {
                return ModelMetadataProviders.Current.GetMetadataForProperty(() => this, Model != null ? Model.ModelType : null, Name);
            }
        }

        public bool IsModel
        {
            get { return ModelMappingManager.Exists(MemberType); }
        }

        public bool IsModelCollection
        {
            get
            {
                var elementType = GetElementType(MemberType);
                return elementType != null && ModelMappingManager.Exists(elementType);
            }
        }

        private static Type GetElementType(Type type)
        {
            var collectionType = type.GetInterface("IEnumerable`1");
            if (collectionType != null)
            {
                var elementType = collectionType.HasElementType
                                      ? collectionType.GetElementType()
                                      : collectionType.GetGenericArguments()[0];

                return elementType;
            }
            return null;
        }

        public override string ToString()
        {
            return UnderliningModel.Text != null && UnderliningModel.TextValue != null
                       ? UnderliningModel.TextValue.ToString()
                       : (Value != null ? Value.ToString() : string.Empty);
        }
    }
}