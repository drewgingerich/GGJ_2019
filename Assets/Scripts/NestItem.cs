using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Audio;
using UnityEngine.Events;

public class NestItem : MonoBehaviour
{
    [SerializeField]
    Transform groupParent;
    [SerializeField]
    ForestFloor floor;
    [SerializeField]
    CameraBounds nestCameraBounds;
    [SerializeField]
    ParallaxDriver nestParallaxDriver;
    [SerializeField]
    CameraBounds forageCameraBounds;
    [SerializeField]
    ParallaxDriver forageParallaxDriver;

    [SerializeField]
    float acceleration = 0.2f;

    [SerializeField]
    bool lightFall;
    [SerializeField]
    float swaySpeed = 4f;
    [SerializeField]
    float swayAmplitude = 1.5f;

    [SerializeField]
    ProximityPlayer proximityPlayer;
    [SerializeField]
    AreaPlayer areaPlayer;

    [SerializeField]
    Interactable interactable;

    [System.NonSerialized]
    public bool isHeld = false;
    [System.NonSerialized]
    public bool nestCooldown = false;

    ParallaxDriver currentParallaxDriver;
    Coroutine fallRoutine;

    void Start() {
        currentParallaxDriver = IsInForageCameraBounds() ? forageParallaxDriver : nestParallaxDriver;
        currentParallaxDriver.AddParallaxItem(transform);
    }

    public void PickUp() {
        StopAllCoroutines();
        nestCooldown = false;
        isHeld = true;
        currentParallaxDriver.RemoveParallaxItem(transform);
        currentParallaxDriver = null;
    }

    public void Fall() {
        isHeld = false;
        interactable.Deselect();
        transform.SetParent(groupParent);
        Vector3 localPosition = transform.localPosition;
        localPosition.z = 0;
        transform.localPosition = localPosition;

        if (IsInForageCameraBounds()) {
            currentParallaxDriver = forageParallaxDriver;
        } else {
            currentParallaxDriver = nestParallaxDriver;
			StartCoroutine(ParallaxDriverSwitchRoutine());
        }
		currentParallaxDriver.AddParallaxItem(transform);

        if (lightFall) {
            fallRoutine = StartCoroutine(LightFallRoutine());
        } else {
			fallRoutine = StartCoroutine(FallRoutine());
        }
    }

    bool IsInForageCameraBounds() {
        return transform.position.y <= forageCameraBounds.GetBoundsWorldSpace().max.y;
    }

    IEnumerator ParallaxDriverSwitchRoutine() {
        while (!IsInForageCameraBounds()) {
            yield return null;
        }
		currentParallaxDriver.RemoveParallaxItem(transform);
        currentParallaxDriver = forageParallaxDriver;
		currentParallaxDriver.AddParallaxItem(transform);
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
        Land();
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
            float swayAdjustmentVertical = Mathf.Abs(Mathf.Cos(time * swaySpeed) * swayAmplitude * 1.5f) * -1;

            newPosition.x += swayAdjustmentHorizontal;
            newPosition.y += swayAdjustmentVertical * 0.5f;

            transform.position = newPosition;
            // speed += acceleration * Time.deltaTime;
            // Vector3 move = Vector3.down * speed * Time.deltaTime;
            // transform.position += move;
            yield return null;
        }
    }

	public void Nest() {
		proximityPlayer.enabled = false;
		areaPlayer.enabled = true;
        interactable.Interact();
		interactable.SetActive(false);
	}

    public void Land() {
        nestCooldown = false;
        StopAllCoroutines();
    }
}