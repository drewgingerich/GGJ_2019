using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestFloor : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    public float groundLevel;

    public Bounds bounds { 
        get {
            Bounds bounds = spriteRenderer.sprite.bounds;
            bounds.center = transform.position;
            return bounds;
        }
    }

    void OnDrawGizmosSelected() {
        float leftEdge = bounds.min.x + transform.position.x;
        float rightEdge = bounds.max.x + transform.position.x;

        Vector3 groundLineStart = new Vector3(leftEdge, groundLevel, 0);
        Vector3 groundLineEnd = new Vector3(rightEdge, groundLevel, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundLineStart, groundLineEnd);
    }
}
