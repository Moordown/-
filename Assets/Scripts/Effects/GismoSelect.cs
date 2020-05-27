using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GismoSelect : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
