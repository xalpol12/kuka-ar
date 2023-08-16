using System;
using System.Collections.Generic;
using Connectivity.Models.AggregationClasses;
using Connectivity.Models.SimpleValues.Pairs;
using Newtonsoft.Json.Utilities;
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
