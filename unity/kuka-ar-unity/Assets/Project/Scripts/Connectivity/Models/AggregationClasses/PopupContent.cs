using System;
using UnityEngine;

namespace Project.Scripts.Connectivity.Models.AggregationClasses
{
    public struct PopupContent
    {
        public string Header { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public DateTime DateTimeMark { get; set; }
        public Sprite Icon { get; set; }
    }
}