using TMPro;
using UnityEngine;

public class LoggerDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void clearLog()
    {
        text.text = "Logger:\n";
    }
}
