using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BirdController : MonoBehaviour {

	public bool animating;
	bool foraging = false;
	bool grounded;
	float speed = 1;
	NestItem carriedItem;

	Interactable selectedInteractable;

	[SerializeField]
	Transform forageScreenEntryPoint;
	[SerializeField]
	float forageEntraceTime;
	[SerializeField]
	float forageEntranceSpeed;
	[SerializeField]
	Transform nestScreenEntryPoint;
	[SerializeField]
	CutscenePlayer cutscenePlayer;
	[SerializeField]
	Transform nestItemParent;

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

	void Awake() {
		activeController = this;
	}

	void Start() {
		Fly();
		Nest();
	}

	void Update() {
		Interactable nearestInteractable = null;
		float squareDistanceToNearestInteractable = Mathf.Infinity;
		foreach (Interactable item in Interactable.activeItems) {
			float squareDistance = (transform.position - item.transform.position).sqrMagnitude;
			if (squareDistance < squareDistanceToNearestInteractable) {
				nearestInteractable = item;
				squareDistanceToNearestInteractable = squareDistance;
			}
		}

		float distance = Mathf.Sqrt(squareDistanceToNearestInteractable);
		if (distance <= minPickupDistance) {
			if (nearestInteractable == selectedInteractable) {
				return;
			} else if (selectedInteractable != null) {
				selectedInteractable.Deselect();
			}
			selectedInteractable = nearestInteractable;
			selectedInteractable.Select();
		} else {
			if (selectedInteractable != null) {
				selectedInteractable.Deselect();
				selectedInteractable = null;
			}
		}
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
			Nest();
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
			Forage();
			return;
		}
		ConstrainToCameraHorizontal();
	}

	void Forage() {
		nestCam.gameObject.SetActive(false);

		transform.position = (Vector2)forageScreenEntryPoint.position;
		Vector3 forageCamPosition = forageCam.transform.position;
		forageCamPosition.y = transform.position.y;

		forageCam.gameObject.SetActive(true);
		activeCameraBounds = forageCamBounds;
		// StartCoroutine(ForageEntranceRoutine());
		foraging = true;
	}

	IEnumerator ForageEntranceRoutine() {
		animating = true;
		float timer = 0;
		while (timer < forageEntraceTime) {
			transform.position += Vector3.down * forageEntranceSpeed * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
		animating = false;
	}

	void Nest() {
		forageCam.gameObject.SetActive(false);
		nestCam.gameObject.SetActive(true);

		Vector3 position = forageScreenEntryPoint.position;
		position.x = transform.position.x;
		forageScreenEntryPoint.position = position;

		activeCameraBounds = nestCamBounds;

		transform.position = (Vector2)nestScreenEntryPoint.position;
		foraging = false;
	}

	public void Interact() {
		if (animating) {
			return;
		}

		if (carriedItem != null) {
			DropItem();	
			return;
		}

		if (selectedInteractable == null) {
			return;
		}

		NestItem selectedItem = selectedInteractable.GetComponent<NestItem>();
		if (selectedItem != null) {
			Pickup(selectedItem);
			return;
		}

		HotBird hotBird = selectedInteractable.GetComponent<HotBird>();
		if (hotBird != null) {
			hotBird.Sing();
			return;
		}
	}

	public void DropItem() {
		Interactable interactable = carriedItem.GetComponent<Interactable>();
		interactable.SetActive(true);

		carriedItem.Fall();
		carriedItem = null;
	}

	public void Pickup(NestItem cruft) {
		carriedItem = cruft;
		selectedInteractable.Interact();
		selectedInteractable.SetActive(false);
		carriedItem.PickUp();
		StartCoroutine(PickUpRoutine());
	}

	IEnumerator PickUpRoutine() {
		if (grounded) {
			animating = true;
			anim.SetTrigger(pickUpTrigger);
			yield return new WaitForSeconds(pickUpAnimationTime);
			animating = false;
		}
		carriedItem.transform.SetParent(beak);
		carriedItem.transform.localPosition = Vector3.back;
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		UnityEditor.Handles.color = Color.green;
		UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, minPickupDistance);
	}
#endif

}