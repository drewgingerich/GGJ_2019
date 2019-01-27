using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBounds : MonoBehaviour
{
    Bounds localBounds;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
		var vertExtent = camera.orthographicSize;
		var horzExtent = vertExtent * Screen.width / Screen.height;
        localBounds = new Bounds(Vector3.zero, new Vector3(horzExtent * 2, vertExtent * 2, 0));

    }

    public Bounds GetBoundsLocalSpace() {
        return localBounds;
    }

    public Bounds GetBoundsWorldSpace() {
        Bounds worldBounds = localBounds;
        worldBounds.center = transform.position;
        return worldBounds;
    }
}
