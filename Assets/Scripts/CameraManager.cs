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

    // Start is called before the first frame update
    void Start()
    {
        offsetX = groundCamera.transform.position.x - bird.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate() {
        if (groundCamera.enabled == true) {
            var newX = Mathf.Clamp(bird.transform.position.x + offsetX, LandscapeConstants.LeftScreen, LandscapeConstants.RightScreen);
            groundCamera.transform.position = new Vector3(newX, groundCamera.transform.position.y, groundCamera.transform.position.z);
        }
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
