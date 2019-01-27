using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestFloor : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    public float groundLevel;

    void OnDrawGizmosSelected() {
        Vector3 groundLineStart = new Vector3(-5, groundLevel, 0);
        Vector3 groundLineEnd = new Vector3(5, groundLevel, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundLineStart, groundLineEnd);
    }
}
