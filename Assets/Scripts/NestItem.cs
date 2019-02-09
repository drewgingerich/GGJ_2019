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
    float acceleration = 0.2f;

    [SerializeField]
    Instrument instrument;

    [SerializeField]
    bool lightFall;
    [SerializeField]
    float swaySpeed = 4f;
    [SerializeField]
    float swayAmplitude = 1.5f;

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
        if (lightFall) {
            fallRoutine = StartCoroutine(LightFallRoutine());
        } else {
			fallRoutine = StartCoroutine(FallRoutine());
        }
    }

    IEnumerator FallRoutine () {
        float speed = 0.1f;
        float groundLevel = floor.groundLevel + Random.Range(-2f, 0f);
        while (transform.position.y > groundLevel && !isHeld) { // && !IsAtNest()) {
            speed += acceleration * Time.deltaTime;
            Vector3 move = Vector3.down * speed * Time.deltaTime;
            transform.position += move;
            yield return null;
        }
    }

    IEnumerator LightFallRoutine() {
        float groundLevel = floor.groundLevel + Random.Range(-2f, 0f);
        Vector3 truePosition = transform.position;
        float time = 0;
        while (transform.position.y > groundLevel && !isHeld) { // && !IsAtNest()) {
            time += Time.deltaTime;
            truePosition += Vector3.down * Time.deltaTime * 1;
            Vector3 newPosition = truePosition;

            float swayAdjustmentHorizontal = Mathf.Sin(time * swaySpeed) * swayAmplitude;
            float swayAdjustmentVertical = Mathf.Abs(Mathf.Cos(time * swaySpeed) * swayAmplitude) * -1;

            newPosition.x += swayAdjustmentHorizontal;
            newPosition.y += swayAdjustmentVertical * 0.5f;

            transform.position = newPosition;
            // speed += acceleration * Time.deltaTime;
            // Vector3 move = Vector3.down * speed * Time.deltaTime;
            // transform.position += move;
            yield return null;
        }
    }

    public void Rest() {
        if (fallRoutine != null) {
            StopCoroutine(fallRoutine);
        }
    }
}