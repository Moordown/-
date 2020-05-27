using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GismoLink : MonoBehaviour
{
    public Transform linkGameObject;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var position = transform.position;
        var position1 = linkGameObject.position;
        Gizmos.DrawLine(position, position1);
        Gizmos.DrawCube(position, Vector3.one);
        Gizmos.DrawCube(position1, Vector3.one);
    }
}
