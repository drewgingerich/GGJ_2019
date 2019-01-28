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
	NestItem chosenBit;
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
		Debug.Log("flying");
		speed = airSpeed;
		grounded = false;
		anim.SetBool(groundedBool, false);
	}

	void Land() {
		Debug.Log("landed");
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
		forageCam.gameObject.SetActive(true);

		transform.position = forageScreenEntryPoint.position;
		Vector3 forageCamPosition = forageCam.transform.position;
		forageCamPosition.y = transform.position.y;

		activeCameraBounds = forageCamBounds;
		StartCoroutine(ForageEntranceRoutine());
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

		forageScreenEntryPoint.position = transform.position;

		activeCameraBounds = nestCamBounds;

		transform.position = nestScreenEntryPoint.position;
		foraging = false;
	}

	public void Interact() {
		Debug.Log("interacting!");
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
			Debug.Log("starting to sing");
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
		Debug.Log(grounded);
		if (grounded) {
			animating = true;
			anim.SetTrigger(pickUpTrigger);
			yield return new WaitForSeconds(pickUpAnimationTime);
			animating = false;
		}
		Pickup(cruft);
	}

	public bool IsNearHotBird(){
		Debug.Log(hotBird == null);
		Debug.Log(Vector3.Distance(hotBird.transform.position, transform.position).ToString());
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