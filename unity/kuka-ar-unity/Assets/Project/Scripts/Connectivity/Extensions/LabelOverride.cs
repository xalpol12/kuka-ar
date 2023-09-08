using TMPro;
using UnityEngine;

namespace Project.Scripts.Connectivity.Extensions
{
    public class LabelOverride : MonoBehaviour
    {
        public static LabelOverride Label;

        [SerializeField] private TMP_Text statusText;

        private void Awake()
        {
            Label = this;
        }

        public void OverrideStatusLabel(string s)
        {
            statusText.text = s;
            statusText.color = s switch
            {
                "Connected" => new Color(0.176f, 0.78f, 0.439f),
                "Connecting" => new Color(0.94f, 0.694f, 0.188f),
                "Disconnected" => new Color(0.949f, 0.247f, 0.259f),
                _ => statusText.color
            };
        }
    }
}
