using System;
using UnityEngine;

namespace EventSystem
{
    public class CubeController : MonoBehaviour
    {
        public int id;
        public GameObject cube;
        internal bool isRotating;
        internal bool isScalingUp;
        internal bool isScalingDown;
        internal bool isMovingHorizontally;
        internal bool isMovingVertically;
        internal int movementDirection;

        void Start()
        {
            isRotating = false;
            isScalingUp = false;
            isScalingDown = false;
            isMovingHorizontally = false;
            isMovingVertically = false;
            movementDirection = 1;
        
            UIEvents.Current.OnPressStartRotation += StartRotation;
            UIEvents.Current.OnReleaseStopRotation += StopRotation;

            UIEvents.Current.OnPressStartScalingUp += StartScalingUp;
            UIEvents.Current.OnReleaseStopScalingUp += StopScalingUp;
            UIEvents.Current.OnPressStartScalingDown += StartScalingDown;
            UIEvents.Current.OnReleaseStopScalingDown += StopScalingDown;

            UIEvents.Current.OnPressMoveLeft += StartMovingLeft;
            UIEvents.Current.OnPressMoveRight += StartMovingRight;
            UIEvents.Current.OnReleaseStopMovingHorizontally += StopMovingHorizontally;
        
            UIEvents.Current.OnPressMoveUp += StartMovingUp;
            UIEvents.Current.OnPressMoveDown += StartMovingDown;
            UIEvents.Current.OnReleaseStopMovingVertically += StopMovingVertically;
        }

        private void StartRotation(int id)
        {
            if (id != this.id) return;
            isRotating = true;
        }

        private void StopRotation(int id)
        {
            if (id != this.id) return;
            isRotating = false;
        }

        private void StartScalingUp(int id)
        {
            if (id != this.id) return;
            isScalingUp = true;
        }

        private void StopScalingUp(int id)
        {
            if (id != this.id) return;
            isScalingUp = false;
        }

        private void StartScalingDown(int id)
        {
            if (id != this.id) return;
            isScalingDown = true;
        }

        private void StopScalingDown(int id)
        {
            if (id != this.id) return;
            isScalingDown = false;
        }
    
        private void StartMovingLeft(int id)
        {
            isMovingHorizontally = true;
            movementDirection = -1 * Math.Abs(movementDirection);
        }

        private void StartMovingRight(int id)
        {
            isMovingHorizontally = true;
            movementDirection = Math.Abs(movementDirection);
        }

        private void StopMovingHorizontally(int id)
        {
            isMovingHorizontally = false;
        }

        private void StartMovingUp(int id)
        {
            isMovingVertically = true;
            movementDirection = Math.Abs(movementDirection);
        }

        private void StartMovingDown(int id)
        {
            isMovingVertically = true;
            movementDirection = -1 * Math.Abs(movementDirection);
        }

        private void StopMovingVertically(int id)
        {
            isMovingVertically = false;
        }
    }
}
