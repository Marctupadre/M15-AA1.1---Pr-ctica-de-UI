using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;

    public float distanceSpeed;
    public Vector2 distanceLimits;
    float desiredDistance;
    public enum Target { Ball, Target, Middlepoint}
    public float centerSpeed;
    public Target target;
    Vector3 desiredCenter;
    public float rotationSpeed;
    Vector3 initialRotation;
    float desiredRotation;
    private void Start()
    {
        cam = Camera.main;
        Zoom(10);
        initialRotation = transform.localEulerAngles;
    }
    public void Zoom(float value)
    {
        desiredDistance = Mathf.Clamp(desiredDistance + value, distanceLimits.x, distanceLimits.y);
    }
    public void Rotate(float value)
    {
        desiredRotation += value;
    }
    public void SwitchTargetBall()
    {
        this.target = Target.Ball;
    }
    public void SwitchTargetTarget()
    {
        this.target = Target.Target;
    }
    public void SwitchTargetMiddlepoint()
    {
        this.target = Target.Middlepoint;
    }
    public void SwitchTarget(Target target)
    {
        this.target = target;
    }
    void LateUpdate()
    {
        switch (target)
        {
            case Target.Target:
                desiredCenter = BallGameManager.instance.currentTarget.position;
                break;
            case Target.Middlepoint:
                desiredCenter = (BallGameManager.instance.currentTarget.position + BallGameManager.instance.ball.transform.position) * 0.5f;
                break;
            default:
                desiredCenter = BallGameManager.instance.ball.transform.position;
                break;
        }
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0,0,-desiredDistance), distanceSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredCenter, centerSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(initialRotation.x, desiredRotation, initialRotation.z), rotationSpeed * Time.deltaTime);
    }
}
