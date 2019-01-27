using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LandscapeConstants : ScriptableObject
{
    public float GroundThreshhold = -0.5f;
    public float SkyThreshhold = 3f;
    public Vector2 NestPosition = new Vector2(8,6);
    public float LeftScreen = -10f;
    public float RightScreen = 10f; 
    public float LeftSky = -8f;
    public float RightSky = 4.3f;
    public float TopSky = 6f;
    public float ResetXForSkyMode = -6f;

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
