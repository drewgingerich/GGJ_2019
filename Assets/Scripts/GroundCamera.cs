using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GroundCamera : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    ForestFloor stage;

    new Camera camera;
    Bounds bounds;

    void Awake() {
        camera = GetComponent<Camera>();
    }

    void Start() {
		var vertExtent = camera.orthographicSize;
		var horzExtent = vertExtent * Screen.width / Screen.height;
        bounds = new Bounds(Vector3.zero, new Vector3(horzExtent * 2, vertExtent * 2, 0));
    }

    void Update() {
        Vector2 diff = (Vector3)(player.position - transform.position);
        Vector3 newPosition = transform.position + (Vector3)diff * 0.1f;
        Vector2 min = (Vector2)(bounds.min + newPosition);
        Vector2 max = (Vector2)(bounds.max + newPosition);
        if (min.x < stage.bounds.min.x) {
            newPosition.x += (stage.bounds.min.x - min.x);
        } 
        if (min.y < stage.bounds.min.y) {
            newPosition.y += (stage.bounds.min.y - min.y);
        } 
        if (max.x > stage.bounds.max.x) {
            newPosition.x += (stage.bounds.max.x - max.x);
        } 
        if (max.y > stage.bounds.max.y) {
            newPosition.y += (stage.bounds.max.y - max.y);
        } 
        transform.position = newPosition;
    } 


}
