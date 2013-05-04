using System;
using System.Threading.Tasks;
using System.Web;

namespace RunningObjects.Extensions.AngularJS
{
	public class AngularJsResourceHandler : IHttpAsyncHandler
	{
		public bool IsReusable { get { return false; } }

		public void ProcessRequest(HttpContext context)
		{
			var task = CreateTask();
			if (task.Status == TaskStatus.Created)
				task.RunSynchronously();
			else 
				task.Wait();
		}

		public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
		{
			var task = CreateTask().ContinueWith((t, o) => (o as AsyncCallback)(t), cb);
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

		private static Task CreateTask()
		{
			return Task.Run(() =>
			{
				
			});
		}
	}
}
