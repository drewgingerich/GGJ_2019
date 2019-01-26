using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BirdController))]
public class BirdInput : MonoBehaviour {

	BirdController bird;
	Vector2 move;

	// Use this for initialization
	void Start () {
		bird = GetComponent<BirdController>();
	}
	
	// Update is called once per frame
	void Update () {
		move = Vector2.zero;
		move.x += Input.GetAxis("Horizontal");
		move.y += Input.GetAxis("Vertical");
		bird.Move(move.normalized);

		if (Input.GetButtonDown("Fire1")) {
			bird.Interact();
		}
	}
}
