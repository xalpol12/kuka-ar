using System.Net.Http;
using System.Threading.Tasks;

namespace Project.Scripts.Connectivity.Http
{
    public interface IHttpRequest<TResult>
    {
        Task<TResult> Execute(HttpClient httpClient);
    }
}
