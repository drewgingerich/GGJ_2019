using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

	void Start () {
		
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

		transform.position = newPosition;

	}

	//pick up
	public void Interact() {
		if (null != chosenBit) {
			Drop();
		} else {
			var distance = NestItem.ActiveItems.Min(item => Math.Abs(transform.position.x - item.transform.position.x));
			if (distance <= minPickupDistance) {
				var cruft = NestItem.ActiveItems.FirstOrDefault(item => Math.Abs(transform.position.x - item.transform.position.x) == distance);
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
		chosenBit.isHeld = true;
		chosenBit = cruft;
		chosenBit.transform.SetParent(transform);
		chosenBit.transform.localPosition = beak.localPosition;
	}
}
