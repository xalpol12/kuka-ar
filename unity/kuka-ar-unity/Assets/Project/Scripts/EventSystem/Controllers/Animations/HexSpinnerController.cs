using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.EventSystem.Controllers.Animations
{
    public class HexSpinnerController : MonoBehaviour
    {
        [SerializeField] private GameObject blueHex; 
        [SerializeField] private GameObject redHex; 
        [SerializeField] private GameObject greenHex;
        
        internal Image[] HexImages;
        internal RectTransform[] HexRects;
        internal Vector3[] Spin; 
        
        private void Start()
        {
            var fadedBlue = blueHex.transform.parent.Find("BlueHexPath").GetComponent<RectTransform>();
            var fadedRed = redHex.transform.parent.Find("RedHexPath").GetComponent<RectTransform>();
            var fadedGreen = greenHex.transform.parent.Find("GreenHexPath").GetComponent<RectTransform>();
            
            
            HexImages = new[] {blueHex.GetComponent<Image>(),
                redHex.GetComponent<Image>(), greenHex.GetComponent<Image>()};
            HexRects = new[] {blueHex.GetComponent<RectTransform>(), fadedBlue,
                redHex.GetComponent<RectTransform>(), fadedRed,
                greenHex.GetComponent<RectTransform>(), fadedGreen};
            Spin = new[] {Vector3.forward, Vector3.back, Vector3.forward};
        }
    }
}
