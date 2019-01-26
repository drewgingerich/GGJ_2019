using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LandscapeConstants : ScriptableObject
{
    public float GroundThreshhold = -0.5f;
    public float SkyThreshhold = 1f;
    public Vector2 NestPosition = new Vector2(8,3);

    public enum NestItemCategory {
        YARN,
        TWIG,
        CANDY_WRAPPER,
        RIBBON,
        LEAF,
        FEATHER,
        FUR,
        GLASS,
        BOTTLECAP,
        POPTOP
    }

}
