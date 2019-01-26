using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BirdController : MonoBehaviour {

	private bool grounded;

	float speed = 1;

	[SerializeField]
	float airSpeed = 5;

	[SerializeField]
	float groundSpeed = 2;

	[SerializeField]
	Transform beak;

	[SerializeField]
	NestItem chosenBit;

	[SerializeField]
	LandscapeConstants LandscapeConstants;

	[SerializeField]
	float minPickupDistance = 0.2f;

	[SerializeField]
	UnityEvent onUseSkyCam = new UnityEvent();

	[SerializeField]
	UnityEvent onUseGroundCam = new UnityEvent();

	void Start () {
		onUseGroundCam.Invoke();
		
	}

	public void Move(Vector2 direction) {
		Vector3 move = (Vector3)direction * Time.deltaTime;

		//move!

		Vector3 newPosition = transform.position + move * speed;
		if (newPosition.y <= LandscapeConstants.GroundThreshhold) {
			grounded = true;
			speed = groundSpeed;
			newPosition.y = LandscapeConstants.GroundThreshhold;
		} else {
			speed = airSpeed;
			grounded = false;
		}
		
		if (transform.position.y < LandscapeConstants.SkyThreshhold && newPosition.y > LandscapeConstants.SkyThreshhold) {
			newPosition.x = LandscapeConstants.ResetXForSkyMode;
			onUseSkyCam.Invoke();
		} else if (transform.position.y > LandscapeConstants.SkyThreshhold && newPosition.y < LandscapeConstants.SkyThreshhold) {
			onUseGroundCam.Invoke();
		}
		if (newPosition.y < LandscapeConstants.SkyThreshhold) {
			newPosition.x = Mathf.Clamp(newPosition.x, LandscapeConstants.LeftScreen, LandscapeConstants.RightScreen);
		} else {
			newPosition.x = Mathf.Clamp(newPosition.x, LandscapeConstants.LeftSky, LandscapeConstants.RightSky);
			newPosition.y = Mathf.Clamp(newPosition.y, LandscapeConstants.GroundThreshhold, LandscapeConstants.TopSky);
		}

		transform.position = newPosition;

	}

	//pick up
	public void Interact() {
		if (null != chosenBit) {
			Drop();
		} else {
			var distance = NestItem.ActiveItems.Min(item => (transform.position - item.transform.position).sqrMagnitude);
			if (distance <= minPickupDistance) {
				var cruft = NestItem.ActiveItems.FirstOrDefault(item => (transform.position - item.transform.position).sqrMagnitude == distance);
				Pickup(cruft);
			}
		}
	}

	private void Drop() {
		chosenBit.isHeld = false;
		chosenBit.transform.SetParent(null);
		StartCoroutine(chosenBit.Fall());
		chosenBit = null;
	}

	private void Pickup(NestItem cruft) {
		chosenBit = cruft;
		chosenBit.isHeld = true;
		chosenBit.transform.SetParent(transform);
		chosenBit.transform.localPosition = beak.localPosition;
	}
}