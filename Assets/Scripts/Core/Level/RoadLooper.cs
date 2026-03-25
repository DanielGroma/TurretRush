using UnityEngine;
using System.Collections.Generic;

public class RoadLooper : MonoBehaviour
{
    [SerializeField] private List<Transform> roadSegments;
    [SerializeField] private Transform player;

    private float segmentLength;

    private void Start()
    {
        segmentLength = roadSegments[0].GetComponent<Renderer>().bounds.size.z;
    }

    private void Update()
    {
        Transform firstSegment = roadSegments[0];

        if (player.position.z - firstSegment.position.z > segmentLength)
        {
            MoveFirstToEnd();
        }
    }

    private void MoveFirstToEnd()
    {
        Transform first = roadSegments[0];
        Transform last = roadSegments[roadSegments.Count - 1];

        Vector3 newPos = last.position + Vector3.forward * segmentLength;
        first.position = newPos;

        roadSegments.RemoveAt(0);
        roadSegments.Add(first);
    }
}