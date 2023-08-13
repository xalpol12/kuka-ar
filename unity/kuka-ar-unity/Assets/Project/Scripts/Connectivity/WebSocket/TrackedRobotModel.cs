using System;
using System.Collections.Generic;
using Connectivity.Models.AggregationClasses;
using Connectivity.Models.KRLValues;
using Connectivity.Models.SimpleValues.Pairs;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Utils;
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
        
        private readonly GameObject gameObject;
        private HashSet<ExceptionMessagePair> FoundExceptions { get; }
        private KRLInt activeBase;
        private KRLInt activeTcp;
        private KRLFrame tcpOrientation;
        private KRLJoints activeJoints;

        private Queue<KRLFrame> orientationUpdates;
        private readonly float threshold;
        private KRLFrame lastEnqueued;

        public TrackedRobotModel(GameObject instantiatedObject, float threshold)
        {
            gameObject = instantiatedObject;
            FoundExceptions = new HashSet<ExceptionMessagePair>();
            orientationUpdates = new Queue<KRLFrame>();
            this.threshold = threshold;
            lastEnqueued = new KRLFrame
            {
                Position = Vector3.zero
            };
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
        
        public void UpdateGameObjectOrientation()
        {
            if (orientationUpdates.TryDequeue(out var update))
            {
                gameObject.transform.position = update.Position;
                gameObject.transform.rotation = Quaternion.Euler(update.Rotation);
                DebugLogger.Instance().AddLog($"Pos: {update.Position.ToString()}, " +
                                              $"rot: {update.Rotation.ToString()} ");
            }
        }

        private void UpdateRobotData(string key, KRLValue value)
        {
            switch (key)
            {
                //TODO: Already updated flag to change value only if it was already used to translate a point,
                case ValueName.ActiveBase:
                    activeBase = (KRLInt)value;
                    break;
                case ValueName.ActiveTcp:
                    activeTcp = (KRLInt)value;
                    break;
                case ValueName.Base:
                    SetValueIfChanged((KRLFrame)value);
                    break;
                case ValueName.Tcp:
                    tcpOrientation = (KRLFrame)value;
                    break;
                case ValueName.Joints:
                    activeJoints = (KRLJoints)value;
                    break;
            }
        }
        
        // current value
        // queue: <future values>
        // dequeue: current value = queue.TryDequeue and update position of gameobject
        // enqueue: if |new - last inserted| > threshold -> enqueue
        private void SetValueIfChanged(KRLFrame update)
        {
            if (IsNewValueGreaterThanThreshold(update, lastEnqueued))
            {
                orientationUpdates.Enqueue(update);
                lastEnqueued = update;
            }
        }

        private void UpdateExceptions(HashSet<ExceptionMessagePair> exceptions)
        {
            FoundExceptions.UnionWith(exceptions);
        }

        private bool IsNewValueGreaterThanThreshold(KRLFrame newValue, KRLFrame oldValue)
        {
            Vector3 difference = newValue.Position - oldValue.Position;
            
            return Math.Abs(difference.x) > threshold ||
                   Math.Abs(difference.y) > threshold ||
                   Math.Abs(difference.z) > threshold;
        }
    }
}
