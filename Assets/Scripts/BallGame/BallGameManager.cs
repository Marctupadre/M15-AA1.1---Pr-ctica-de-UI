using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGameManager : MonoBehaviour
{
    public Transform[] targets;
    public Transform currentTarget { get { return targets[currentTargetIndex]; } }
    public int currentTargetIndex = -1;
    public GameObject targetVisuals;
    public BallController ball;
    public float maxDistanceIndicator = 5f;
    public LineRenderer followLine;
    public float distance;
    public float distanceToWin;
    int lastTarget = -1;

    public static BallGameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        NewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(currentTarget.position, ball.transform.position);
        if (distance < distanceToWin)
        {
            NewTarget();
        }

        Vector3 direction = targetVisuals.transform.position - ball.transform.position;
        float dist = direction.magnitude;

        followLine.SetPosition(0, ball.transform.position);
        if (dist > maxDistanceIndicator)
        {
            direction = direction / dist * maxDistanceIndicator;
        }
        followLine.SetPosition(1, ball.transform.position + direction);
        if (dist > maxDistanceIndicator * 2f)
        {
            followLine.SetPosition(2, ball.transform.position + direction.normalized * (dist - Mathf.Min(maxDistanceIndicator, dist)));
        }
        else
        {
            followLine.SetPosition(2, ball.transform.position + direction);
        }
        followLine.SetPosition(3, targetVisuals.transform.position);
    }

    public void NewTarget()
    {
        if(targets.Length > 2)
        {
            lastTarget = currentTargetIndex;
            while(lastTarget == currentTargetIndex)
            {
                currentTargetIndex = Random.Range(0, targets.Length);
            }
            targetVisuals.transform.position = targets[currentTargetIndex].transform.position;
        }
    }

    public void SetPause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
}
