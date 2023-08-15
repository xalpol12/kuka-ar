using System.Threading.Tasks;
using UnityEngine;

public class ServerHttpService : MonoBehaviour
{
    public static ServerHttpService Instance;
    internal bool PingResponse;
    private void Awake()
    {
        Instance = this;
    }
    
    internal async void PingOperation(string ip)
    {
        var ping = new Ping(ip);

        while (!ping.isDone)
        {
            PingResponse = false;
            await Task.Yield();
        }

        PingResponse = true;
    }
}
