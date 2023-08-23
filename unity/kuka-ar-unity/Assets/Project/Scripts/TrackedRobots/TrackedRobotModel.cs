using System;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.TrackedRobots
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
        private HashSet<ExceptionMessagePair> foundExceptions { get; }
        private KRLInt activeBase;
        private KRLInt activeTcp;
        private KRLFrame tcpOrientation;
        private KRLJoints activeJoints;

        private Queue<KRLFrame> orientationUpdates;
        private readonly float positionThreshold;
        private readonly float rotationThreshold;
        private KRLFrame lastEnqueued;

        public TrackedRobotModel(GameObject instantiatedObject, float positionThreshold, float rotationThreshold)
        {
            gameObject = instantiatedObject;
            this.positionThreshold = positionThreshold;
            this.rotationThreshold = rotationThreshold;
            
            foundExceptions = new HashSet<ExceptionMessagePair>();
            orientationUpdates = new Queue<KRLFrame>();
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
                DebugLogger.Instance.AddLog($"Pos: {update.Position.ToString()}, " +
                                              $"rot: {update.Rotation.ToString()}; ");
            }
        }

        private void UpdateRobotData(string key, KRLValue value)
        {
            switch (key)
            {
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
        
        private void SetValueIfChanged(KRLFrame update)
        {
            if (IsNewValueGreaterThanPositionThreshold(update, lastEnqueued) || 
                IsNewValueGreaterThanRotationThreshold(update, lastEnqueued))
            {
                orientationUpdates.Enqueue(update);
                lastEnqueued = update;
            }
        }

        private void UpdateExceptions(HashSet<ExceptionMessagePair> exceptions)
        {
            foundExceptions.UnionWith(exceptions);
        }

        private bool IsNewValueGreaterThanPositionThreshold(KRLFrame newValue, KRLFrame oldValue)
        {
            Vector3 difference = newValue.Position - oldValue.Position;

            return Math.Abs(difference.x) > positionThreshold ||
                   Math.Abs(difference.y) > positionThreshold ||
                   Math.Abs(difference.z) > positionThreshold;
        }

        private bool IsNewValueGreaterThanRotationThreshold(KRLFrame newValue, KRLFrame oldValue)
        {
            Vector3 difference = newValue.Rotation - oldValue.Rotation;

            return Math.Abs(difference.x) > rotationThreshold ||
                   Math.Abs(difference.y) > rotationThreshold ||
                   Math.Abs(difference.z) > rotationThreshold;
        }
    }
}
