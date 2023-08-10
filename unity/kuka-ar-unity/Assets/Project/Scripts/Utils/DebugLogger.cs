using System;
using System.Collections.Concurrent;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Utils
{
    public class DebugLogger : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;
        private ConcurrentQueue<string> messages;

        private void Start()
        {
            messages = new ConcurrentQueue<string>();
        }

        public void AddLog(String log)
        {
            messages.Enqueue(log);
        }

        public void ClearLogs()
        {
            messages.Clear();
            textField.text = "--Debug field--";
        }

        private void Update()
        {
            if (messages.TryDequeue(out string message))
            {
                textField.text += message;
            }
        }
        
        #region Singleton logic
        
        private static DebugLogger instance = null;

        public static DebugLogger Instance()
        {
            if (!Exists())
            {
                throw new Exception (
                    "DebugLogger could not find the DebugLogger object. " +
                    "Please ensure you have added the DebugLogger Prefab to your scene.");
            }
            return instance;
        }
    
        private static bool Exists()
        {
            return instance != null;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        void OnDestroy()
        {
            instance = null;
        }
        
        #endregion
    }
}
