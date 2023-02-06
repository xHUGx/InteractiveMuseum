using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBetweenObjects : MonoBehaviour
{
    [SerializeField] private GameObject firstObject;
    [SerializeField] private GameObject secondObject;

    [SerializeField] private Color color;

    [SerializeField] private float distance;

    void Update()
    {
        if (firstObject == null || secondObject == null) return;
        distance = Vector3.Distance(firstObject.transform.position, secondObject.transform.position);
    }

    private void OnDrawGizmos()
    {
        if (firstObject == null || secondObject == null) return;
        Gizmos.color = color;
        Gizmos.DrawLine(firstObject.transform.position, secondObject.transform.position);
    }
}