using UnityEngine;
using UnityEngine.SceneManagement;

public class WebViewController : MonoBehaviour
{
    [SerializeField] private int id;
    private void Start()
    {
        WebViewEvents.Events.OnClickOpenMoreOptions += SwapScene;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SwapScene(6);
        }
    }

    private void SwapScene(int uid)
    {
        if (id == uid)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
