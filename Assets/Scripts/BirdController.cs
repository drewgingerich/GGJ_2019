using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BirdController : MonoBehaviour {

	public bool animating;
	bool foraging = false;
	bool grounded;
	float speed = 1;
	NestItem chosenBit;
	[SerializeField]
	Transform forageScreenEntryPoint;

	[SerializeField]
	Transform nestScreenEntryPoint;
	[SerializeField]
	CutscenePlayer cutscenePlayer;

	// Animator parameter string keys
	const string stoppedBool = "Stopped";
	const string groundedBool = "Grounded";
	const string pickUpTrigger = "PickUp";
	const float pickUpAnimationTime = 0.2f;


	[System.NonSerialized]
	public static BirdController activeController;

	[SerializeField]
	HotBird hotBird;

	[SerializeField]
	float airSpeed = 5;
	[SerializeField]
	float groundSpeed = 2;
	[SerializeField]
	float minPickupDistance = 0.2f;
	[SerializeField]
	float edgePadding = 0.7f;

	[SerializeField]
	Transform beak;
	[SerializeField]
	Animator anim;

	[SerializeField]
	ForestFloor stage;
	[SerializeField]
	Camera forageCam;
	[SerializeField]
	CameraBounds forageCamBounds;
	[SerializeField]
	Camera nestCam;
	[SerializeField]
	CameraBounds nestCamBounds;

	CameraBounds activeCameraBounds;

	private bool gameHasStarted = false;

	void Awake() {
		activeController = this;
	}

	void Start() {
		Fly();
		StartCoroutine(Nest());
		gameHasStarted = true;
	}

	public void InputMove(Vector2 direction) {
		if (animating) {
			return;
		}
		Move(direction);
	}

	public void Move(Vector2 direction) {
		if (direction == Vector2.zero) {
			anim.SetBool(stoppedBool, true);
			return;
		} else {
			anim.SetBool(stoppedBool, false);
		}

		Vector3 move = (Vector3)direction * Time.deltaTime;
		Vector3 newPosition = transform.position + move * speed;

		Bounds currentBounds = activeCameraBounds.GetBoundsWorldSpace();
		newPosition.x = Mathf.Clamp(newPosition.x, currentBounds.min.x, currentBounds.max.x);
		newPosition.y = Mathf.Clamp(newPosition.y, forageCamBounds.GetBoundsWorldSpace().min.y, nestCamBounds.GetBoundsWorldSpace().max.y - 1);

		transform.position = newPosition;

		if (foraging) {
			HandleForaging();
		} else {
			HandleNesting();
		}
	}

	void HandleForaging() {
		Vector3 position = transform.position;

		if (position.y <= stage.groundLevel) {
			position.y = stage.groundLevel;
			Land();
		} else {
			Fly();
		}

		ConstrainToCameraHorizontal();

		if (position.y > activeCameraBounds.GetBoundsWorldSpace().max.y) {
			StartCoroutine(Nest());
		}
	}

	void Fly() {
		speed = airSpeed;
		grounded = false;
		anim.SetBool(groundedBool, false);
	}

	void Land() {
		speed = groundSpeed;
		grounded = true;
		anim.SetBool(groundedBool, true);
	}

	void ConstrainToCameraHorizontal() {
		Vector2 diff = Vector2.zero;

		Bounds bounds = activeCameraBounds.GetBoundsWorldSpace();

		if (transform.position.x < bounds.min.x + edgePadding) {
			diff.x = bounds.min.x + edgePadding - transform.position.x;
		}
		if (transform.position.x > bounds.max.x - edgePadding) {
			diff.x = bounds.max.x - edgePadding - transform.position.x;
		}

		transform.position += (Vector3)diff;
	}

	void HandleNesting(){
		Bounds bounds = activeCameraBounds.GetBoundsWorldSpace();
		if (transform.position.y < bounds.min.y) {
			StartCoroutine(Forage());
		} else {
			ConstrainToCameraHorizontal();
		}
	}

	IEnumerator Forage() {
		yield return StartCoroutine(cutscenePlayer.ScreenWipe((int) Image.OriginVertical.Top));
		nestCam.gameObject.SetActive(false);

		transform.position = (Vector2)forageScreenEntryPoint.position;
		Vector3 forageCamPosition = forageCam.transform.position;
		forageCamPosition.y = transform.position.y;

		forageCam.gameObject.SetActive(true);
		activeCameraBounds = forageCamBounds;

		yield return StartCoroutine(cutscenePlayer.ScreenUnwipe((int) Image.OriginVertical.Bottom));

		foraging = true;
		yield return null;
	}

	IEnumerator Nest() {
		if (gameHasStarted)
			yield return StartCoroutine(cutscenePlayer.ScreenWipe((int) Image.OriginVertical.Bottom));
		forageCam.gameObject.SetActive(false);
		nestCam.gameObject.SetActive(true);
		if (gameHasStarted)
			yield return StartCoroutine(cutscenePlayer.ScreenUnwipe((int) Image.OriginVertical.Top));


		Vector3 position = forageScreenEntryPoint.position;
		position.x = transform.position.x;
		forageScreenEntryPoint.position = position;

		activeCameraBounds = nestCamBounds;

		transform.position = (Vector2)nestScreenEntryPoint.position;
		foraging = false;
	}

	public void Interact() {
		if (null != chosenBit) {
			DropItem();
		} else {
			float distance = minPickupDistance;
			NestItem selectedBit = null;
			foreach (NestItem item in NestItem.ActiveItems) {
				Vector2 diff = (transform.position - item.transform.position);
				float newDistance = diff.magnitude;
				if (newDistance <= distance) {
					selectedBit = item;
					distance = newDistance;
				}
			}
			if (selectedBit != null) {
				StartCoroutine(PickUpRoutine(selectedBit));
				return;
			}
		}

		if (IsNearHotBird()) {
			cutscenePlayer.PlaySampleSongCutscene();
		}
	}

	public void DropItem() {
		if (null == chosenBit) {
			return;
		}
		chosenBit.isHeld = false;
		if (grounded) {
			chosenBit.transform.localPosition = Vector3.forward * 0.5f;
		} 
		chosenBit.transform.SetParent(null);
		chosenBit.Fall();
		chosenBit = null;
	}

	private void Pickup(NestItem cruft) {
		chosenBit = cruft;
		chosenBit.isHeld = true;
		chosenBit.transform.SetParent(beak);
		chosenBit.transform.localPosition = Vector3.back;
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

	public bool IsNearHotBird(){
		if (hotBird == null) return false;
		return Vector3.Distance(hotBird.transform.position, transform.position) < 2.4;
	}

	

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		UnityEditor.Handles.color = Color.green;
		UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, minPickupDistance);
	}
#endif

}