using UnityEngine;

public class JogsBehavior : MonoBehaviour
{
    private JogsController jogsController;
    private Vector3 jogsHomePosition;
    private const int Distance = 280;
    void Start()
    {
        jogsController = GetComponent<JogsController>();
        jogsController.jogButton.SetActive(!jogsController.ShowJogs);
        jogsController.jogsValues.SetActive(jogsController.ShowJogs);
        jogsHomePosition = jogsController.jogButton.transform.position;
    }
    
    void Update()
    {
        if (jogsController.ShowJogs)
        {
            ShowJogs();
        }
        else
        {
            HideJogs();
        }
    }

    private void HideJogs()
    {
        var toggleActive = false;
        foreach (Transform child in jogsController.jogsValues.transform)
        {
            var translation = Vector3.up *
                              (jogsController.transformFactor * (Time.deltaTime * child.GetSiblingIndex() * Distance));
            var newPosition = child.position + translation;
            if (newPosition.y > jogsHomePosition.y)
            {
                toggleActive = true;
                break;
            }

            translation *= jogsController.transformFactor;
            child.Translate(translation);
        }

        if (toggleActive)
        {
            jogsController.jogButton.SetActive(true);
            jogsController.jogsValues.SetActive(false);
        }
        
    }

    private void ShowJogs()
    {
        jogsController.jogButton.SetActive(false);
        jogsController.jogsValues.SetActive(true);
        foreach (Transform child in jogsController.jogsValues.transform)
        {
            var translation = Vector3.down *
                              (jogsController.transformFactor * (Time.deltaTime * child.GetSiblingIndex() * Distance));
            var newPosition = child.position + translation;
            if (newPosition.y < jogsHomePosition.y - child.GetSiblingIndex() * Distance)
            {
                break;
            }
            
            child.Translate(translation);
        }
    }
}
