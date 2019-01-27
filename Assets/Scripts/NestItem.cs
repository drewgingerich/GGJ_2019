using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class NestItem : MonoBehaviour
{
    [SerializeField]
    LandscapeConstants LandscapeConstants;

    [SerializeField]
    float speed = 1;

    [System.NonSerialized]
    public bool isHeld = false;

    public static List<NestItem> ActiveItems;

    [SerializeField]
    AudioMixerGroup mixerGroup;

    [SerializeField]
    public LandscapeConstants.NestItemCategory Category;

    [SerializeField]
    public NestEvent onNestItemAdded;

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
        while (transform.position.y > LandscapeConstants.GroundThreshhold && !isHeld && !IsAtNest()) {
            Vector3 move = Vector3.down * speed * Time.deltaTime;
            transform.position += move;
            yield return null;
        }
        if (IsAtNest()) {
            Debug.Log("added to the nest!");
            onNestItemAdded.Invoke(this);
            yield return null;
        }
    }

    private bool IsAtNest(){
        return transform.position.y >= LandscapeConstants.NestPosition.y && Vector3.Distance(transform.position, LandscapeConstants.NestPosition) <  0.5;
    }

    void OnDestroy() {
        ActiveItems.Remove(this);
    }
}



[System.Serializable]
public class NestEvent : UnityEvent<NestItem>{

}