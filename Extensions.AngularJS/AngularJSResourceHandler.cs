using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using RunningObjects.Core;
using RunningObjects.Core.Mapping;
using RunningObjects.Core.Mapping.Configuration;

namespace RunningObjects.Extensions.AngularJS
{
	public class AngularJsResourceHandler : IHttpAsyncHandler
	{
		public bool IsReusable { get { return false; } }

		public void ProcessRequest(HttpContext context)
		{
			var task = CreateTask(context);
			if (task.Status == TaskStatus.Created)
				task.RunSynchronously();
			else
				task.Wait();
		}

		public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
		{
			var task = CreateTask(context).ContinueWith((t, o) => (o as AsyncCallback)(t), cb);
			if (task.Status == TaskStatus.Created)
				task.Start();
			return task;
		}

		public void EndProcessRequest(IAsyncResult result)
		{
			var task = result as Task;
			if (task == null)
				return;
			task.Wait();
			task.Dispose();
		}

		private static Task CreateTask(HttpContext context)
		{
			return Task.Run(() =>
			{
				context.Response.ContentType = "text/javascript";
				using (var writer = new StreamWriter(context.Response.OutputStream))
				{
					writer.WriteLine("var ro = angular.module('runningobjects', ['ngResource'])");
					foreach (var type in MappingConfiguration.Assemblies.Values.SelectMany(assembly => assembly.Types))
						WriteResource(context, writer, type);
				}
			});
		}
		
		private static void WriteResource(HttpContext context, StreamWriter writer, TypeMappingConfiguration type)
		{
			var url = context.Request.Url;
			// TODO: find a better way to define when it's local or remote
			var domain = (context.Request["local"] == "1") ? string.Empty : string.Format("//{0}\\:{1}", url.Host, url.Port);
			var baseUrl = domain + VirtualPathUtility.ToAbsolute("~/");
			writer.WriteLine("ro.factory('{0}', function($resource) {{ return $resource('{1}api/{0}/:id/:methodName/:index', {{id:'@id'}}, {{", type.UnderlineType.PartialName(), baseUrl);
			writer.WriteLine("'all': {method:'GET', isArray:true}, ");
			writer.WriteLine("'get': {method:'GET'}, ");
			writer.WriteLine("'insert': {method:'POST'}, ");
			writer.WriteLine("'update': {method:'PUT'}, ");
			writer.WriteLine("'delete': {method:'DELETE'}, ");

			var typeMapping = ModelMappingManager.MappingFor(type.UnderlineType);

			// write the instance methods
			foreach (var instanceMethod in typeMapping.InstanceMethods)
				WriteMethod(writer, instanceMethod);

			// write the static methods
			foreach (var staticMethod in typeMapping.StaticMethods)
				WriteMethod(writer, staticMethod, true);

			writer.WriteLine("})});");
			writer.WriteLine("");
		}

		private static void WriteMethod(StreamWriter writer, MethodMapping method, bool isStatic = false)
		{
			var removeId = isStatic ? ", id : null" : string.Empty;
			writer.Write("'{0}': {{ method:'POST', params : {{ methodName:'{1}' {2} ", ToCamelCase(method.MethodName), method.MethodName, removeId);
			foreach (var parameter in method.Parameters)
			{
				writer.Write(", {0}:'@{0}' ", parameter.Name);
			}
			writer.WriteLine(" } }, ");
		}

		private static string ToCamelCase(string name)
		{
			return char.ToLower(name[0]) + name.Substring(1);
		}
	}
}
