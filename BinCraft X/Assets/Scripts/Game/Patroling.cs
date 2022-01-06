using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patroling : MonoBehaviour
{
    [Header("0 for no patroling; 1 for moving to single point; 2 or more for patroling")]
    public List<Transform> points = new List<Transform>();

    [Header("How long to wait at patrol point")]
    public float durationWait;

    private float tWait;
    private int indexCurrentPoint;
    private bool patroling;

    private NavMeshAgent nma;

    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (points.Count > 0)
        {
            patroling = true;
            nma.destination = points[indexCurrentPoint].position;
        }
    }

    private void Update()
    {
        if (patroling)
        {
            // update time of waiting at patrol point
            if (tWait > 0)
            {
                tWait -= Time.deltaTime;
                if (tWait <= 0)
                {
                    PatrolToNextPoint();
                }
            }
            else
            {
                // check if close to patrol point
                if (IsCloseToDestination())
                {
                    if (points.Count == 1)
                    {
                        // stop patroling for single patrol point
                        patroling = false;
                    }
                    else
                    {
                        if (durationWait > 0)
                        {
                            // wait at patrol point
                            tWait = durationWait;
                        }
                        else
                        {
                            // continue to next point
                            PatrolToNextPoint();
                        }
                    }
                }
            }

        }
    }

    private void PatrolToNextPoint()
    {
        indexCurrentPoint++;
        if (indexCurrentPoint >= points.Count)
        {
            indexCurrentPoint = 0;
        }
        nma.destination = points[indexCurrentPoint].position;
    }

    // check if we arrived to navigation destination
    // this one ignores Y
    private bool IsCloseToDestination()
    {
        return Vector2.Distance(new Vector2(nma.transform.position.x, nma.transform.position.z), new Vector2(nma.destination.x, nma.destination.z)) < 0.5f;
    }
}
