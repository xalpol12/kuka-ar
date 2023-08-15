using System;
using UnityEngine;

public class MoreOptionsEvents : MonoBehaviour
{
    public static MoreOptionsEvents Events;

    private void Awake()
    {
        Events = this;
    }

    public event Action<int> onClickBack;

    public void BackToMenu(int id)
    {
        onClickBack?.Invoke(id);
    }
}
