using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	Controller2D controller;
	Vector2 input;
	public Vector2 velocity;

	void Start() {
		controller = GetComponent<Controller2D>();
	}
	
	void Update () {
		controller.Move(velocity * Time.deltaTime, input);
	}

	public void SetDirectionalInput(Vector2 directionalInput) {
		input = directionalInput;
	}
}
