using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using UnityEngine;

namespace Project.Scripts.Connectivity.ExceptionHandling
{
    public class GlobalExceptionStorage : MonoBehaviour
    {
        public static GlobalExceptionStorage Instance;

        private ConcurrentStack<ExceptionMessagePair> registeredExceptions;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        void Start()
        {
            registeredExceptions = new();
        }

        public void AddExceptions(HashSet<ExceptionMessagePair> exceptions)
        {
            foreach (var exception in exceptions)
            {
                AddException(exception);
            }
        }

        public void AddException(ExceptionMessagePair exception)
        {
            registeredExceptions.Push(exception);
        }

        // public ExceptionMessagePair? TryReadNextException()
        // {
        //     return registeredExceptions.TryPop(out var exception) ? exception : null;
        // }

        public bool TryPopException(out ExceptionMessagePair exception)
        {
            return registeredExceptions.TryPop(out exception);
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
