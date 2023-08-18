using System;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using UnityEngine;

namespace Project.Scripts.Utils
{
    public class AotTypeEnforcer : MonoBehaviour
    {
        private void Awake()
        {
            AotHelper.EnsureDictionary<string, ValueWithError>();
            AotHelper.EnsureDictionary<string, Dictionary<string, ValueWithError>>();
            AotHelper.EnsureList<ExceptionMessagePair>();
        }
    }
}
