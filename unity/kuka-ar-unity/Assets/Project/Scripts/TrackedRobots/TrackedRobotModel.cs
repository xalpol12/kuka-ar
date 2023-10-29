using System;
using System.Collections.Generic;
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
            public const string ActiveTool = "$ACT_TOOL";
            public const string Base = "$BASE";
            public const string Tool = "$POS_ACT";
            public const string Joints = "$AXIS_ACT";
        }

        private readonly GameObject baseObject;
        private readonly GameObject toolObject;
        private readonly float posThresh;
        private readonly float rotThresh;

        private readonly Dictionary<string, IKrlWrapper> krlValues;

        public TrackedRobotModel(GameObject baseObject, GameObject toolObject, float posThresh, float rotThresh)
        {
            this.baseObject = baseObject;
            this.toolObject = toolObject;
            this.posThresh = posThresh;
            this.rotThresh = rotThresh;

            krlValues = new Dictionary<string, IKrlWrapper>(5);

            SetupRobotVariables();
            SubscribeToValueUpdatedEvents();
        }

        private void SetupRobotVariables()
        {
            krlValues.Add(ValueName.ActiveBase, new KrlIntWrapper());
            krlValues.Add(ValueName.ActiveTool, new KrlIntWrapper());
            krlValues.Add(ValueName.Base, new KrlFrameWrapper(posThresh, rotThresh));
            krlValues.Add(ValueName.Tool, new KrlFrameWrapper(posThresh, rotThresh));
            krlValues.Add(ValueName.Joints, new KrlJointsWrapper(rotThresh));
        }

        private void SubscribeToValueUpdatedEvents()
        {
            ((KrlIntWrapper) krlValues[ValueName.ActiveBase]).ValueUpdated += OnActiveBaseUpdated;
            ((KrlIntWrapper) krlValues[ValueName.ActiveTool]).ValueUpdated += OnActiveToolUpdated;
            ((KrlJointsWrapper) krlValues[ValueName.Joints]).ValueUpdated += OnActiveJointsUpdated;
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
        
        private void UpdateRobotData(string key, IKRLValue value)
        {
            krlValues[key].UpdateValue(value);
        }
        

        private void UpdateExceptions(HashSet<ExceptionMessagePair> exceptions)
        {
            GlobalExceptionStorage.Instance.AddExceptions(exceptions);
        }
        
        public void UpdateGameObjects()
        {
            if (((KrlFrameWrapper) krlValues[ValueName.Base]).TryDequeue(out var baseUpdate))
            {
                UpdateBaseGameObject(baseObject, baseUpdate);
            }

            if (((KrlFrameWrapper) krlValues[ValueName.Tool]).TryDequeue(out var toolUpdate))
            {
                UpdateToolGameObject(toolObject, toolUpdate);
            }
        }

        private void UpdateBaseGameObject(GameObject gameObject, KRLFrame update)
        {
            gameObject.transform.localPosition = update.Position;

            DebugLogger.Instance.AddLog($"Rotation values for BASE: {update.Rotation}");

            gameObject.transform.localRotation = Quaternion.Euler(update.Rotation);

        }

        private void UpdateToolGameObject(GameObject gameObject, KRLFrame update)
        {
            gameObject.transform.localPosition = update.Position;

            DebugLogger.Instance.AddLog($"Rotation values for TOOL: {update.Rotation}");

            Quaternion zRot = Quaternion.Euler(0, 0, update.Rotation.z);
            Quaternion yRot = Quaternion.Euler(0, update.Rotation.y, 0);
            Quaternion xRot = Quaternion.Euler(update.Rotation.x, 0, 0);

            // (Z, Y', X'')
            Quaternion finalRot = zRot * yRot * xRot;
            gameObject.transform.localRotation = finalRot;
        }

        private void OnActiveBaseUpdated(object sender, KRLInt e)
        {
            BaseValueUpdated?.Invoke(this, e);
        }

        private void OnActiveToolUpdated(object sender, KRLInt e)
        {
            ToolValueUpdated?.Invoke(this, e);
        }

        private void OnActiveJointsUpdated(object sender, KRLJoints e)
        {
            JointsValueUpdated?.Invoke(this, e);
        }
    }
}
