using System.Net.Http;
using System.Threading.Tasks;

namespace Project.Scripts.Connectivity.RestAPI
{
    public interface IRequestCommand<TResult>
    {
        Task<TResult> Execute(HttpClient httpClient);
    }
}
