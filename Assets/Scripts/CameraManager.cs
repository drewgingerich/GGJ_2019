using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera groundCamera;
    public Camera nestCamera;
    private float offsetX;
    public BirdController bird;
    public LandscapeConstants LandscapeConstants;

    public void UseGroundCam(){
        groundCamera.enabled = true;
        nestCamera.enabled = false;
    }

    
    public void UseNestCam(){
        groundCamera.enabled = false;
        nestCamera.enabled = true;
    }
}
