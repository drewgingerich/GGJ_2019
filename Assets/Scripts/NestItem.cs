using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class NestItem : MonoBehaviour
{
    public static List<NestItem> sceneItems = new List<NestItem>();
    public static List<NestItem> ActiveItems = new List<NestItem>();

    const float minFullSoundDistance = 2;
    const float minSoundDistance = 10;

    [SerializeField]
    ForestFloor floor;

    [SerializeField]
    float speed = 1;

    [SerializeField]
    Instrument instrument;

    [System.NonSerialized]
    public bool isHeld = false;

    Coroutine fallRoutine;


    void Awake(){
        sceneItems.Add(this);
        ActiveItems.Add(this);
    }

    void Update() {
        float distance = (transform.position - BirdController.activeController.transform.position).magnitude;
        if (distance < minFullSoundDistance) {
            instrument.SetVolume(1);
        } else if (distance < minSoundDistance) {
            float adjustedDistance = (distance - minFullSoundDistance) / minSoundDistance;
            instrument.SetVolume(1 - Mathf.Sqrt(adjustedDistance));
            // instrument.SetVolume(1 - Mathf.Pow(adjustedDistance, 0.3f));
            // instrument.SetVolume(1 - Mathf.Log10(adjustedDistance));

        } else {
            instrument.SetVolume(0);
        }
    }

    public void Fall() {
        fallRoutine = StartCoroutine(FallRoutine());
    }

    IEnumerator FallRoutine () {
        while (transform.position.y > floor.groundLevel && !isHeld) { // && !IsAtNest()) {
            Vector3 move = Vector3.down * speed * Time.deltaTime;
            transform.position += move;
            yield return null;
        }
    }

    public void Rest() {
        StopCoroutine(fallRoutine);
    }
}