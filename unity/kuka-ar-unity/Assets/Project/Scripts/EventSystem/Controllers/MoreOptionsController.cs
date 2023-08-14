using UnityEngine;

public class MoreOptionsController : MonoBehaviour
{
    public int id;
    public GameObject moreOptionsPrefab;
    private bool showMoreOptionsDialog;
    void Start()
    {
        showMoreOptionsDialog = false;
        moreOptionsPrefab.SetActive(false);
        
        MenuEvents.Event.OnClickMoreOptions += ShowMoreOptions;
    }

    private void Update()
    {
        moreOptionsPrefab.SetActive(showMoreOptionsDialog);
    }

    private void ShowMoreOptions(int uid)
    {
        Debug.Log("ID:" + id + "UID" + uid);
        if (id == uid)
        {
            showMoreOptionsDialog = !showMoreOptionsDialog;
            Debug.Log(showMoreOptionsDialog);
        }
    }
}
