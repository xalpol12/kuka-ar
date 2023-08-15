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
    public event Action<int> OnClickSaveServerConfig;

    public void ServerPing(int id)
    {
        OnClickPingServer?.Invoke(id);
    }

    public void SaveServerConfig(int id)
    {
        OnClickSaveServerConfig?.Invoke(id);
    }
}
