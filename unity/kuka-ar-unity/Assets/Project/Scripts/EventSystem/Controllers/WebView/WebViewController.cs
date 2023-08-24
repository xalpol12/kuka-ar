using UnityEngine;
using UnityEngine.SceneManagement;

public class WebViewController : MonoBehaviour
{
    [SerializeField] private int id;
    private void Start()
    {
        WebViewEvents.Events.OnClickOpenMoreOptions += SwapScene;
    }

    private void SwapScene(int uid)
    {
        if (id == uid)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
