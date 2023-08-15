using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ServerHttpService : MonoBehaviour
{
    public static ServerHttpService Instance;
    [SerializeField] private GameObject cloudComponent;
    private Sprite pingSuccessIcon;
    private Sprite pingFailedIcon;
    private Image cloudIcon;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cloudIcon = cloudComponent.GetComponent<Image>();
        pingSuccessIcon = Resources.Load<Sprite>("Icons/cloudSuccessIcon");
        pingFailedIcon = Resources.Load<Sprite>("Icons/cloudFailedIcon");
    }

    internal IEnumerator PingOperation(string ip)
    {
        var ping = new Ping(ip);
        while (!ping.isDone)
        {
            SwapCloud(false);
            yield return new WaitForSeconds(0.05f);
        }
        
        SwapCloud(true);
    }

    private void SwapCloud(bool pingStatus)
    {
        cloudIcon.sprite = pingStatus ? pingSuccessIcon : pingFailedIcon;
    }
}
