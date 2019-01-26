using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class NestItem : MonoBehaviour
{

    const string mixerVolumeParameterName = "volume";

    [SerializeField]
    LandscapeConstants LandscapeConstants;

    float speed = 1;

    [System.NonSerialized]
    public bool isHeld;

    [SerializeField]
    AudioMixerGroup mixerGroup;

    public static List<NestItem> ActiveItems;

    void Awake(){
        if (ActiveItems == null) {
            ActiveItems = new List<NestItem>();
        }
        ActiveItems.Add(this);
    }

    void Update() {
        float distance = (transform.position - BirdController.activeController.transform.position).magnitude;
        // Adjust volume based on distance.
    }

    public IEnumerator Fall () {
        while (transform.position.y > LandscapeConstants.GroundThreshhold && !isHeld) {
            Vector3 move = Vector3.down * speed * Time.deltaTime;
            transform.position += move;
            yield return null;
        }
    }
}
