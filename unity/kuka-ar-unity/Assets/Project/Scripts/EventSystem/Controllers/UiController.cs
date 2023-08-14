using UnityEngine;

public class UiController : MonoBehaviour
{
    public int id;
    [SerializeField] private GameObject menuUi;
    [SerializeField] private GameObject moreOptions;
    [SerializeField] private GameObject serverConfig;
    private bool showMoreOptionsDialog;
    void Start()
    {
        showMoreOptionsDialog = false;
        
        menuUi.SetActive(true);
        moreOptions.SetActive(showMoreOptionsDialog);
        serverConfig.SetActive(showMoreOptionsDialog);
        
        MenuEvents.Event.OnClickMoreOptions += ShowMoreOptions;
    }

    private void ShowMoreOptions(int uid)
    {
        Debug.Log("ID:" + id + "UID" + uid);
        if (id == uid)
        {
            showMoreOptionsDialog = !showMoreOptionsDialog;
            Debug.Log(showMoreOptionsDialog);
        }
        //menuUi.SetActive(!showMoreOptionsDialog);
        serverConfig.SetActive(showMoreOptionsDialog);
    }
}
