using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed;
	private FixedJoystick joystick;
	private PhotonView photonView;
	private Rigidbody rigidbody;
	private Vector3 movementDirection;

	void Start()
	{
		photonView = GetComponent<PhotonView>();
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = false;
#if UNITY_IPHONE || UNITY_ANDROID
        joystick = GameObject.Find("Left Joystick").GetComponent<FixedJoystick>();
#endif
	}

	void FixedUpdate()
	{
		if (photonView.IsMine)
		{
			BasicMovement();
		}

		BasicMovement();
	}

	void BasicMovement()
	{
#if UNITY_IPHONE || UNITY_ANDROID
        movementDirection.x = joystick.Horizontal;
        movementDirection.y = 0;
        movementDirection.z = joystick.Vertical;
#elif UNITY_EDITOR || UNITY_STANDALONE
		movementDirection.x = Input.GetAxis("Horizontal");
		movementDirection.y = 0;
		movementDirection.z = Input.GetAxisRaw("Vertical");
#endif

		movementDirection.x = Input.GetAxis("Horizontal");
		movementDirection.y = 0;
		movementDirection.z = Input.GetAxisRaw("Vertical");

		movementDirection = movementDirection.normalized;

		Vector3 movementVelocity = movementDirection * movementSpeed * Time.fixedDeltaTime;
		rigidbody.velocity = movementVelocity;
	}
}
