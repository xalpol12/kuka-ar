using System.Collections.Generic;
using Connectivity.Models.AggregationClasses;
using Connectivity.Models.KRLValues;
using Connectivity.Models.SimpleValues.Pairs;
using UnityEngine;

namespace Connectivity
{
    public class TrackedRobotModel
    {
        private static class ValueName
        {
            public const string ActiveBase = "$ACT_BASE";
            public const string ActiveTcp = "$ACT_TOOL";
            public const string Base = "$BASE";
            public const string Tcp = "$POS_ACT";
            public const string Joints = "$AXIS_ACT";
        }
        private GameObject gameObject;
        private HashSet<ExceptionMessagePair> FoundExceptions { get; }
        private KRLInt activeBase;
        private KRLInt activeTcp;
        private KRLFrame baseOrientation;
        private KRLFrame tcpOrientation;
        private KRLJoints activeJoints;

        public TrackedRobotModel(GameObject instantiatedObject)
        {
            gameObject = instantiatedObject;
            FoundExceptions = new HashSet<ExceptionMessagePair>();
        }

        public void UpdateTrackedRobotVariables(IReadOnlyDictionary<string, ValueWithError> data)
        {
            foreach (var key in data.Keys)
            {
                if (data[key].Value != null)
                {
                    UpdateRobotData(key, data[key].Value);
                }
            
                if (data[key].FoundExceptions.Count > 0)
                {
                    UpdateExceptions(data[key].FoundExceptions);
                }
            }
        }

        private void UpdateRobotData(string key, KRLValue value)
        {
            switch (key)
            {
                //TODO: Already updated flag to change value only if it was already used to translate a point,
                //TODO: Threshold value to update a variable only if | new - old | > threshold
                case ValueName.ActiveBase:
                    activeBase = (KRLInt)value;
                    break;
                case ValueName.ActiveTcp:
                    activeTcp = (KRLInt)value;
                    break;
                case ValueName.Base:
                    baseOrientation = (KRLFrame)value;
                    break;
                case ValueName.Tcp:
                    tcpOrientation = (KRLFrame)value;
                    break;
                case ValueName.Joints:
                    activeJoints = (KRLJoints)value;
                    break;
            }
        }

        private void UpdateExceptions(HashSet<ExceptionMessagePair> exceptions)
        {
            FoundExceptions.UnionWith(exceptions);
        }

        public void UpdateGameObjectOrientation()
        {
            if (baseOrientation == null) return;
            gameObject.transform.position = baseOrientation.Position;
            gameObject.transform.rotation = Quaternion.Euler(baseOrientation.Rotation);
            Debug.Log(baseOrientation.Position + baseOrientation.Rotation.ToString());
        }
    }
}
