using UnityEngine;

public class TrialPlayerMovement : MonoBehaviour
{
	public float movementSpeed;
	private FixedJoystick joystick;
	private Rigidbody rigidbody;
	private Vector3 movementDirection;

	void Start()
	{
		Instantiate(PlayerInfo.playerInfo.allCharacters[PlayerInfo.playerInfo.selectedCharacter], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform);


		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = true;
#if UNITY_IPHONE || UNITY_ANDROID
		joystick = GameObject.Find("Left Joystick").GetComponent<FixedJoystick>();
#endif
	}

	void FixedUpdate()
	{
		BasicMovement();
	}

	void BasicMovement()
	{
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WEBGL
		movementDirection.x = joystick.Horizontal;
		movementDirection.y = 0;
		movementDirection.z = joystick.Vertical;
#elif UNITY_EDITOR || UNITY_STANDALONE
		movementDirection.x = Input.GetAxis("Horizontal");
		movementDirection.y = 0;
		movementDirection.z = Input.GetAxisRaw("Vertical");
#endif

		movementDirection = movementDirection.normalized;

		Vector3 movementVelocity = movementDirection * movementSpeed  * Time.fixedDeltaTime;
		rigidbody.velocity = movementVelocity;
	}
}