
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    
    [Header("Slider")]
    [SerializeField] private Slider progressbar;

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
