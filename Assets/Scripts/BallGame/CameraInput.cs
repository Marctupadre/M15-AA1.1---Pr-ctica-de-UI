using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CameraController))]
public class CameraInput : MonoBehaviour
{
    CameraController controller;
    void Start()
    {
        controller = GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
