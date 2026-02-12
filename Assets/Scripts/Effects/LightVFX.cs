using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightVFX : MonoBehaviour
{
    Light light;
    public float colorSpeed = 0.1f;
    public Gradient color;
    float desiredColor;
    public float positionSpeed = 0.1f;
    public float positionRadius;
    Vector3 startingPosition;
    Vector3 desiredPosition;
    public float intensitySpeed = 0.1f;
    public Vector2 intensity = new Vector2(1,2);
    float desiredIntensity;
    public float rangeSpeed = 0.1f;
    public Vector2 range = new Vector2(5,10);
    float desiredRange;
    void Start()
    {
        light = GetComponent<Light>();
        startingPosition = transform.position;
    }
    void Update()
    {
        desiredColor = Random.Range(0f, 1f);
        desiredPosition = startingPosition + Random.insideUnitSphere * positionRadius;
        desiredIntensity = Random.Range(intensity.x, intensity.y);
        desiredRange = Random.Range(range.x, range.y);

        light.color = Color.Lerp(light.color, color.Evaluate(desiredColor), colorSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSpeed * Time.deltaTime);
        light.intensity = Mathf.Lerp(light.intensity, desiredIntensity, intensitySpeed * Time.deltaTime);
        light.range = Mathf.Lerp(light.range, desiredRange, rangeSpeed * Time.deltaTime);
    }
}
