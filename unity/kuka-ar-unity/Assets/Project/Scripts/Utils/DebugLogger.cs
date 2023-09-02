using System;
using System.Collections.Concurrent;
using Project.Scripts.Connectivity.ExceptionHandling;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Utils
{
    public class DebugLogger : MonoBehaviour
    {
        public static DebugLogger Instance;
        
        [SerializeField] private TextMeshProUGUI textField;
        private ConcurrentQueue<string> messages;
        
        private GlobalExceptionStorage globalExceptionStorage;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            messages = new ConcurrentQueue<string>();
            globalExceptionStorage = GlobalExceptionStorage.Instance;
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

            if (globalExceptionStorage.TryPopException(out var exception))
            {
                textField.text += exception.ToString();
            }

            if (textField.text.Length > 300)
            {
                messages.Clear();
            }
        }

        void OnDestroy()
        {
            Instance = null;
        }
    }
}
