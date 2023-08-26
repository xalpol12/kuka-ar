using UnityEngine;

namespace Project.Scripts.EventSystem.Services.Menu
{
    public class JogsControlService : MonoBehaviour
    {
        public static JogsControlService Instance;
    
    private void Start()
    {
        IsBottomNavDocked = true;
        IsAddRobotDialogOpen = false;
    }
}
