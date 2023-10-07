using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Controllers.LoadingScreen
{
    public class ASyncLoader : MonoBehaviour
    {
        [SerializeField]
        [Header("Menu Screens")]
        [Tooltip("Loading screen - animation or image that should be displayed")]
        private GameObject loadingScreen;
    
        [SerializeField]
        [Header("Slider")]
        [Tooltip("Progress bar component, to display loading progress")]
        private Slider progressbar;

        private void Start()
        {
            loadingScreen.SetActive(true);
        
            StartCoroutine(LoadLevelAsync("MainScene"));
        }

        private IEnumerator LoadLevelAsync(string level)
        {
            var loadOperation = SceneManager.LoadSceneAsync(level);

            while (!loadOperation.isDone)
            {
                progressbar.value = Mathf.Clamp(loadOperation.progress / 0.9f, progressbar.minValue,progressbar.maxValue);
                yield return null;
            }
        }
    }
}
