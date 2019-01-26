using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdController : MonoBehaviour {

	bool animating;
	bool grounded;
	float speed = 1;
	NestItem chosenBit;

	// Animator parameter string keys
	const string stoppedBool = "Stopped";
	const string groundedBool = "Grounded";
	const string leftTrigger = "TurnLeft";
	const string rightTrigger = "TurnRight";
	const string landTrigger = "Land";
	const float landAnimationTIme = 0.5f;
	const string takeoffTrigger = "Takeoff";
	const float takeoffAnimationTime = 0.5f;
	const string pickUpTrigger = "PickUp";
	const float pickUpAnimationTime = 0.2f;

	[System.NonSerialized]
	public static BirdController activeController;

	[SerializeField]
	float airSpeed = 5;

	[SerializeField]
	float groundSpeed = 2;

	[SerializeField]
	Transform beak;

	[SerializeField]
	Animator anim;

	[SerializeField]
	LandscapeConstants LandscapeConstants;

	[SerializeField]
	float minPickupDistance = 0.2f;

	void Awake() {
		activeController = this;
	}

	public void Move(Vector2 direction) {
		if (animating) {
			return;
		}

		if (direction == Vector2.zero) {
			anim.SetBool(stoppedBool, true);
			return;
		} else {
			anim.SetBool(stoppedBool, false);
		}

		Vector3 move = (Vector3)direction * Time.deltaTime;

		Vector3 newPosition = transform.position + move * speed;
		if (newPosition.y <= LandscapeConstants.GroundThreshhold) {
			if (!grounded) {
				StartCoroutine(LandRoutine());
			}
			grounded = true;
			speed = groundSpeed;
			newPosition.y = LandscapeConstants.GroundThreshhold;
		} else {
			speed = airSpeed;
			if (grounded) {
				StartCoroutine(TakeoffRoutine());
			}
			grounded = false;
		}

		transform.position = newPosition;
	}

	public void Interact() {
		if (null != chosenBit) {
			Drop();
		} else {
			var distance = NestItem.ActiveItems.Min(item => Math.Abs(transform.position.x - item.transform.position.x));
			if (distance <= minPickupDistance) {
				var cruft = NestItem.ActiveItems.FirstOrDefault(item => Math.Abs(transform.position.x - item.transform.position.x) == distance);
				StartCoroutine(PickUpRoutine(cruft));
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
		chosenBit.isHeld = true;
		chosenBit = cruft;
		chosenBit.transform.SetParent(transform);
		chosenBit.transform.localPosition = beak.localPosition;
	}

	IEnumerator LandRoutine() {
		animating = true;
		anim.SetTrigger(landTrigger);
		yield return new WaitForSeconds(landAnimationTIme);
		animating = false;
	}

	IEnumerator TakeoffRoutine() {
		animating = true;
		anim.SetTrigger(takeoffTrigger);
		yield return new WaitForSeconds(takeoffAnimationTime);
		animating = false;	
	}

	IEnumerator PickUpRoutine(NestItem cruft) {
		if (grounded) {
			animating = true;
			anim.SetTrigger(pickUpTrigger);
			yield return new WaitForSeconds(pickUpAnimationTime);
			animating = false;
		}
		Pickup(cruft);
	}

}
