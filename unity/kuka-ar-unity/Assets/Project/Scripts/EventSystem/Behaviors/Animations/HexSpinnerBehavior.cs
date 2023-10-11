using System;
using System.Collections;
using Project.Scripts.EventSystem.Controllers.Animations;
using UnityEngine;

namespace Project.Scripts.EventSystem.Behaviors.Animations
{
    public class HexSpinnerBehavior : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("General animation speed")]
        private float animSpeed;
        
        [SerializeField]
        [Tooltip("Hex rotation animation speed")]
        private float rotationAnimSpeed;
        
        [SerializeField]
        [Tooltip("Animation time - in case of error [s]")]
        private float duration;
        
        [SerializeField]
        [Tooltip("Number of rotation pauses - [360def / n]")]
        private int numberOfRotations;
        
        [SerializeField]
        [Tooltip("Enables hexes rotation animation")]
        private bool withRotation;
        
        private HexSpinnerController controller;
        private float rad;
        private int direction;
        private int countRotation;
        private int rotationAngle;
        private void Start()
        {
            controller = GetComponent<HexSpinnerController>();
            rad = 0f;
            direction = 1;
            countRotation = 0;
            rotationAngle = 360 / numberOfRotations;
        }
        
        private void Update()
        {
            foreach (var hex in controller.HexImages)
            {
                switch (rad)
                {
                    case > 1f:
                        direction = -1;
                        if (withRotation) StartCoroutine(RotateHex());
                        break;
                    case < 0f:
                        direction = 1;
                        if (withRotation) StartCoroutine(RotateHex());
                        break;
                }
                
                hex.fillAmount = rad;
                rad += direction * animSpeed * Time.deltaTime;
            }
        }

        private void OnDisable()
        {
            AdjustRotation(0);
        }
        
        private IEnumerator RotateHex()
        {
            var time = 0f;
            while (Math.Abs(controller.HexRects[0].rotation.eulerAngles.z) < countRotation * rotationAngle)
            {
                for(var i = 0; i < controller.Spin.Length; i++)
                {
                    controller.HexRects[i].transform.Rotate(
                         controller.Spin[i] * (Time.deltaTime * rotationAnimSpeed));
                    controller.HexRects[i + controller.Spin.Length].transform.eulerAngles =
                        controller.HexRects[i].rotation.eulerAngles;
                }

                if (time > duration)
                {
                    countRotation = 0;
                    AdjustRotation(0);
                    yield break;
                }

                time += Time.deltaTime;
                yield return null;
            }
            
            if (countRotation > numberOfRotations - 1)
            {
                countRotation = 0;
                AdjustRotation(0);
                yield break;
            }
            
            AdjustRotation(countRotation * rotationAngle);
            countRotation++;
        }

        private void AdjustRotation(float deg)
        {
            for (var i = 0; i < controller.Spin.Length; i++)
            {
                controller.HexRects[i].transform.eulerAngles = new Vector3(0,0,deg);
                controller.HexRects[i +  controller.Spin.Length].transform.eulerAngles = new Vector3(0,0,deg);
            } 
        }
    }
}
