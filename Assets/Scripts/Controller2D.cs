﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller2D : MonoBehaviour {

	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	public void Move(Vector2 velocity, Vector2 input) {
		rb.AddForce(velocity * input);
	}

	public void Jump(int jumpForce) {
		rb.AddForce(Vector2.up * jumpForce);
	}
}
