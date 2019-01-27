using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHorizontalMovement : MonoBehaviour
{
    float lastXPosition;

    void Update() {
        if (transform.position.x - lastXPosition > 0) {
            transform.localScale = Vector3.one;
        } else if (transform.position.x - lastXPosition < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
		lastXPosition = transform.position.x;
    }
}
