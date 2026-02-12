using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallController))]
public class BallInput : MonoBehaviour
{
    BallController controller;
    private void Start()
    {
        controller = GetComponent<BallController>();
    }
    void Update()
    {

    }
}
