using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    ArcadeKart playerKart;

    [SerializeField]
    List<Transform> levelCheckpoints;

    [SerializeField]
    ProgressionBarExample progressionBarExample;
    
    private float trackLength;
    private int nextCPIndex;
    private int lastCPIndex;


    void Start()
    {
        lastCPIndex = levelCheckpoints.Count-1;
        nextCPIndex = 0;
        trackLength = CalculateDistanceOverCheckpoints(levelCheckpoints.Count);
        Debug.Log(trackLength);
        if (!playerKart)
        {
            playerKart = FindObjectOfType<ArcadeKart>();
        }
    }

    void Update()
    {
        Transform playerPos = playerKart.transform;
        if (PassedCheckpoint(playerPos, levelCheckpoints[nextCPIndex], levelCheckpoints[lastCPIndex]))
        {
            Debug.Log("Passed checkpoint: " + nextCPIndex);
            if (nextCPIndex == levelCheckpoints.Count-1)
            {
                nextCPIndex = 0;
            }
            else
            {
                nextCPIndex++;
            }

            if(lastCPIndex == levelCheckpoints.Count-1)
            {
                lastCPIndex = 0;
            }
            else
            {
                lastCPIndex++;
            }
        }
        Setprogressvalue();
    }

    private float CalculateDistance(Transform position1, Transform position2)
    {
        float x1 = position1.position.x;
        float z1 = position1.position.z;

        float x2 = position2.position.x;
        float z2 = position2.position.z;

        float distanceResult = Mathf.Sqrt(math.pow(x2-x1, 2)+math.pow(z2-z1, 2));
        return distanceResult;
    }

    private bool PassedCheckpoint(Transform player, Transform nextCheckpoint, Transform lastCheckpoint)
    {
        float distanceBetween = CalculateDistance(nextCheckpoint, lastCheckpoint); //Distance between checkpoints
        float distanceToNext = CalculateDistance(player, nextCheckpoint);
        float distanceToLast = CalculateDistance(player, lastCheckpoint);

        if(distanceToNext > distanceBetween)
        {
            //Debug.Log("You are an idiot.");
        }
        else if (distanceToLast>distanceBetween)
        {
            return true;
        }
        return false;
    }

    private float CalculateDistanceOverCheckpoints(int checkpointAmount)
    {
        float tempDistance = 0;
        for (int i = 0; i < checkpointAmount; i++)
        {
            int nextCP = i;
            int lastCP;
            if(i == 0)
            {
                lastCP = checkpointAmount - 1;
            }
            else
            {
                lastCP = nextCP - 1;
            }

            tempDistance += CalculateDistance(levelCheckpoints[lastCP], levelCheckpoints[nextCP]);
        }
        
        return tempDistance;
    }

    private void Setprogressvalue()
    {
        //trackLength
        //player distance

        float playerDistance = CalculateDistanceOverCheckpoints(lastCPIndex) + CalculateDistance(playerKart.transform, levelCheckpoints[lastCPIndex]);
        progressionBarExample.progressionValue = Mathf.RoundToInt((playerDistance / trackLength) * 100);
        Debug.Log("trackLength: " + trackLength);
        Debug.Log("playerDistance: " + playerDistance);
        Debug.Log("divided by track length"+(playerDistance/trackLength));
    }
}
