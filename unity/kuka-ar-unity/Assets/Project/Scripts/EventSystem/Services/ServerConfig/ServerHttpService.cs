using System.Collections;
using UnityEngine;

public class ServerHttpService : MonoBehaviour
{
    public static ServerHttpService Instance;
    internal bool PingResponse;
    private void Awake()
    {
        Instance = this;
    }
    
    internal IEnumerator PingOperation(string ip)
    {
        var ping = new Ping(ip);
        while (!ping.isDone)
        {
            yield return new WaitForSeconds(0.05f);
        }

        PingResponse = ping.isDone;
    }
}
