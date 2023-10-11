using Project.Scripts.EventSystem.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.EventSystem.Controllers.WebView
{
    public class WebViewController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Web view controller ID")]
        private int id;
        
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
}
