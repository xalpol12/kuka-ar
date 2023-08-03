using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    public int rotateSpeed = 50;
    public float scaleFactor = 0.1f;
    public float moveFactor = 5f;
    private CubeController cube;

    private void Start()
    {
        cube = GetComponent<CubeController>();
    }

    private void Update()
    {
        if (cube.isRotating)
        {
            cube.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
        
        if (cube.isScalingUp)
        {
            cube.transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        if (cube.isScalingDown)
        {
            cube.transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        
        if (cube.isMovingHorizontally)
        {
            var translation = Vector3.right * (Time.deltaTime * cube.movementDirection * moveFactor);
            gameObject.transform.Translate(translation);
        }
        
        if (cube.isMovingVertically)
        {
            var translation = Vector3.forward * (Time.deltaTime * cube.movementDirection * moveFactor);
            gameObject.transform.Translate(translation);
        }
    }
}
