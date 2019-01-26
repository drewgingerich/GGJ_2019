using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FollowHorizontalMovement : MonoBehaviour
{
    
    SpriteRenderer sr;

    float lastXPosition;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (transform.position.x - lastXPosition > 0) {
            sr.flipX = false;
        } else if (transform.position.x - lastXPosition < 0) {
            sr.flipX = true;
        }
		lastXPosition = transform.position.x;
    }
}
