using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthShading : MonoBehaviour
{
    [SerializeField]
    Transform nearZReference;
    [SerializeField]
    Transform farZReference;
    [SerializeField]
    float darkLevel = 0.4f;
    [SerializeField]
    float lightLevel = 1f;
    [SerializeField]
    List<SpriteRenderer> shadedSprites;

    void Start()
    {
        float referenceDistance = farZReference.position.z - nearZReference.position.z;
        float inverseReferenceDistance = 1 / referenceDistance;

        float shadeRange = lightLevel - darkLevel;

        foreach (SpriteRenderer sprite in shadedSprites) {
            float spriteDistance = farZReference.position.z - sprite.transform.position.z;
            float distanceRatio = spriteDistance * inverseReferenceDistance;
            float shadeMultiplier = shadeRange * distanceRatio + darkLevel;

            Color color = sprite.color;
            color *= shadeMultiplier;
            color.a = 1f;
            sprite.color = color;
        }
    }
}
