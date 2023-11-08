using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.EventSystem.Extensions;

namespace Project.Scripts.Connectivity.Http.Requests
{
	public class UpdateRobotRequest : IHttpRequest<object>
	{
		private static string url => "/update";
		private readonly Robot robot;

		public UpdateRobotRequest(Robot robot)
		{
			this.robot = robot;
		}


		public async Task<object> Execute(HttpClient httpClient)
		{
			var stringContent = new StringContent(robot.ToCamelCase(),Encoding.UTF8, "application/json");
			return await httpClient.PutAsync(httpClient.BaseAddress + url, stringContent);
		}
	}
}