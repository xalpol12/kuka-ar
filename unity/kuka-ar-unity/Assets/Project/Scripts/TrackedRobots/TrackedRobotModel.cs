using System;
using System.Collections.Generic;
using System.Globalization;
using Project.Scripts.Connectivity.ExceptionHandling;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Connectivity.Models.SimpleValues.Pairs;
using Project.Scripts.Connectivity.Models.Wrappers;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.TrackedRobots
{
    public class TrackedRobotModel
    {
        public event EventHandler<KRLInt> BaseValueUpdated;
        public event EventHandler<KRLInt> ToolValueUpdated;
        public event EventHandler<KRLJoints> JointsValueUpdated;
        
        private static class ValueName
        {
            public const string ActiveBase = "$ACT_BASE";
            public const string ActiveTcp = "$ACT_TOOL";
            public const string Base = "$BASE";
            public const string Tcp = "$POS_ACT";
            public const string Joints = "$AXIS_ACT";
        }

        private readonly GameObject baseObject;
        private readonly GameObject tcpObject;
        private readonly float posThresh;
        private readonly float rotThresh;

        private readonly Dictionary<string, IKRLWrapper> krlValues;

        public TrackedRobotModel(GameObject baseObject, GameObject tcpObject, float posThresh, float rotThresh)
        {
            this.baseObject = baseObject;
            this.tcpObject = tcpObject;
            this.posThresh = posThresh;
            this.rotThresh = rotThresh;

            krlValues = new Dictionary<string, IKRLWrapper>(5);

            SetupRobotVariables();
            SubscribeToValueUpdatedEvents();
        }

        private void SetupRobotVariables()
        {
            krlValues.Add(ValueName.ActiveBase, new KRLIntWrapper());
            krlValues.Add(ValueName.ActiveTcp, new KRLIntWrapper());
            krlValues.Add(ValueName.Base, new KRLFrameWrapper(posThresh, rotThresh));
            krlValues.Add(ValueName.Tcp, new KRLFrameWrapper(posThresh, rotThresh));
            krlValues.Add(ValueName.Joints, new KRLJointsWrapper(rotThresh));
        }

        private void SubscribeToValueUpdatedEvents()
        {
            ((KRLIntWrapper) krlValues[ValueName.ActiveBase]).ValueUpdated += OnActiveBaseUpdated;
            ((KRLIntWrapper) krlValues[ValueName.ActiveTcp]).ValueUpdated += OnActiveToolUpdated;
            ((KRLJointsWrapper) krlValues[ValueName.Joints]).ValueUpdated += OnActiveJointsUpdated;
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
            krlValues[key].UpdateValue(value);
        }
        

        private void UpdateExceptions(HashSet<ExceptionMessagePair> exceptions)
        {
            GlobalExceptionStorage.Instance.AddExceptions(exceptions);
        }
        
        public void UpdateGameObjects()
        {
            if (((KRLFrameWrapper) krlValues[ValueName.Base]).TryDequeue(out var baseUpdate))
            {
                UpdateGivenGameObject(baseObject, baseUpdate);
            }

            if (((KRLFrameWrapper) krlValues[ValueName.Tcp]).TryDequeue(out var tcpUpdate))
            {
                UpdateGivenGameObject(tcpObject, tcpUpdate);
            }
        }

        private void UpdateGivenGameObject(GameObject gameObject, KRLFrame update)
        {
            gameObject.transform.position = update.Position;
            gameObject.transform.rotation = Quaternion.Euler(update.Rotation);
            DebugLogger.Instance.AddLog($"Pos: {update.Position.ToString()}, " +
                                        $"rot: {update.Rotation.ToString()}; ");
        }

        private void OnActiveBaseUpdated(object sender, KRLInt e)
        {
            DebugLogger.Instance.AddLog($"Base number updated: {e.Value.ToString()}; ");
            BaseValueUpdated?.Invoke(this, e);
        }

        private void OnActiveToolUpdated(object sender, KRLInt e)
        {
            DebugLogger.Instance.AddLog($"TCP number updated: {e.Value.ToString()}; ");
            ToolValueUpdated?.Invoke(this, e);
        }

        private void OnActiveJointsUpdated(object sender, KRLJoints e)
        {
            DebugLogger.Instance.AddLog($"Joints updated: {e.J1.ToString(CultureInfo.InvariantCulture)}; ");
            JointsValueUpdated?.Invoke(this, e);
        }
    }
}
