using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxDriver : MonoBehaviour
{
    [SerializeField]
    Transform zNearReference;
    [SerializeField]
    Transform zFarReference;
    [SerializeField]
    bool horizontalParallax = true;
    [SerializeField]
    bool verticalParallax = false;
    [SerializeField]
    List<Transform> parallaxItems;

    Vector3 previousPosition;

    public void AddParallaxItem(Transform item) {
        parallaxItems.Add(item);
    }

    public void RemoveParallaxItem(Transform item) {
        parallaxItems.Remove(item);
    }

    void Update()
    {
        float referenceZDiff = zFarReference.position.z - zNearReference.position.z;
        float inverseReferenceZDiff = 1 / referenceZDiff;
        Vector3 referenceXYDiff = transform.position - previousPosition;

        Vector3 parallaxShift;
        foreach (Transform item in parallaxItems) {
            float itemZDiff = zFarReference.position.z - item.position.z;
            float moveRatio = 1 - itemZDiff * inverseReferenceZDiff;
            parallaxShift = referenceXYDiff * moveRatio;

            parallaxShift.z = 0;
            if (!horizontalParallax) {
                parallaxShift.x = 0;
            }
            if (!verticalParallax) {
                parallaxShift.y = 0;
            }

            item.position += parallaxShift;
        }

        previousPosition = transform.position;
    }
}
