using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera groundCamera;
    public Camera nestCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseGroundCam(){
        groundCamera.enabled = true;
        nestCamera.enabled = false;
    }

    
    public void UseNestCam(){
        groundCamera.enabled = false;
        nestCamera.enabled = true;
    }
}
