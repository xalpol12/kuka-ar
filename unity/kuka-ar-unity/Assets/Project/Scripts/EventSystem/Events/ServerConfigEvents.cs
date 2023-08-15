using System;
using UnityEngine;

public class ServerConfigEvents : MonoBehaviour
{
    public static ServerConfigEvents Events;

    private void Awake()
    {
        Events = this;
    }

    public event Action<int> OnClickPingServer;

    public void ServerPing(int id)
    {
        OnClickPingServer?.Invoke(id);
    }
}
