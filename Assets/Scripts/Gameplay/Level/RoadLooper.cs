using UnityEngine;
using System.Collections.Generic;

public class RoadLooper : MonoBehaviour
{
    [SerializeField] private List<Transform> _roadSegments;
    [SerializeField] private Transform _player;

    private float _segmentLength;

    private void Start()
    {
        _segmentLength = _roadSegments[0].GetComponent<Renderer>().bounds.size.z;
    }

    private void Update()
    {
        Transform firstSegment = _roadSegments[0];

        if (_player.position.z - firstSegment.position.z > _segmentLength)
        {
            MoveFirstToEnd();
        }
    }

    private void MoveFirstToEnd()
    {
        Transform first = _roadSegments[0];
        Transform last = _roadSegments[_roadSegments.Count - 1];

        Vector3 newPos = last.position + Vector3.forward * _segmentLength;
        first.position = newPos;

        _roadSegments.RemoveAt(0);
        _roadSegments.Add(first);
    }
}