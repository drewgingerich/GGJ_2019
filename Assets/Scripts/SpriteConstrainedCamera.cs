using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraBounds))]
public class SpriteConstrainedCamera : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    float speed = 3f;

    Bounds bounds;

    void Start() {
        CameraBounds cameraBounds = GetComponent<CameraBounds>();
        bounds = cameraBounds.GetBoundsLocalSpace();
    }

    void Update() {
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        spriteBounds.center = spriteRenderer.transform.position;

        Vector3 diff = player.position - transform.position;
        Vector3 newPosition = transform.position + diff * Time.deltaTime * speed;

        Vector3 min = bounds.min + newPosition;
        Vector3 max = bounds.max + newPosition;

        diff = Vector2.zero;

        if (min.x < spriteBounds.min.x) {
            diff.x = spriteBounds.min.x - min.x;
        }
        if (min.y < spriteBounds.min.y) {
            diff.y = spriteBounds.min.y - min.y;
        }
        if (max.x > spriteBounds.max.x) {
            diff.x = spriteBounds.max.x - max.x;
        }
        if (max.y > spriteBounds.max.y) {
            diff.y = spriteBounds.max.y - max.y;
        }

        newPosition += diff;
        newPosition.z = -10;
        transform.position = newPosition;
    } 
}
