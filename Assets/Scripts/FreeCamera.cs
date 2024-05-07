using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Camera) )]
public class FreeCamera : MonoBehaviour {
    public float acceleration = 50; // how fast you accelerate
	public float accSprintMultiplier = 4; // how much faster you go when "sprinting"
	public float lookSensitivity = 1; // mouse look sensitivity
	public float dampingCoefficient = 5; // how quickly you break to a halt after you stop your input
	private Vector3 velocity; // current velocity

	void Update() {
		// Position
		velocity += GetAccelerationVector() * Time.deltaTime;

		// Physics
		velocity = Vector3.Lerp( velocity, Vector3.zero, dampingCoefficient * Time.deltaTime );
		transform.position += velocity * Time.deltaTime;
	}

	Vector3 GetAccelerationVector() {
		Vector3 moveInput = default;

		void AddMovement( KeyCode key, Vector3 dir ) {
			if( Input.GetKey( key ) ) {
				moveInput += dir;
            }
		}

		AddMovement(KeyCode.W, Vector3.up);
		AddMovement(KeyCode.S, Vector3.down);
		AddMovement(KeyCode.D, Vector3.right);
		AddMovement(KeyCode.A, Vector3.left);
		AddMovement(KeyCode.E, Vector3.back);
		AddMovement(KeyCode.Q, Vector3.forward);
		Vector3 direction = transform.TransformVector(moveInput.normalized);

		if(Input.GetKey(KeyCode.LeftShift)) {
			return direction * (acceleration * accSprintMultiplier); // "sprinting"
        }

		return direction * acceleration; // "walking"
	}
}
