using System.Collections;
using UnityEngine;

public class TrialPlayerCombat : MonoBehaviour
{
	public Projectile bulletPrefab;
	public float fireRate = 0.1f;
	public float bulletSpawnOffset;

	private FixedJoystick fixedJoystick;
	private TrialPlayerMovement playerMovement;
	private Vector3 joystickDirection;
	private bool canShoot;


	void Start()
	{
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WEBGL
		fixedJoystick = GameObject.Find("Right Joystick").GetComponent<FixedJoystick>();
#endif

		playerMovement = GetComponent<TrialPlayerMovement>();

		joystickDirection = Vector3.forward;
		transform.forward = joystickDirection;
		canShoot = true;

	}

#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WEBGL
	void Update()
	{
		joystickDirection = new Vector3(fixedJoystick.Horizontal, 0, fixedJoystick.Vertical);

		if (Mathf.Abs(joystickDirection.x) > 0.25f || Mathf.Abs(joystickDirection.z) > 0.25f)
		{
			transform.forward = joystickDirection;
			if (canShoot)
			{
				canShoot = false;
				Shoot();
			}
		}
	}

#elif UNITY_EDITOR || UNITY_STANDALONE
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		Physics.Raycast(ray, out hit, 100.0f);

		Vector3 target = new Vector3(hit.point.x, transform.position.y, hit.point.z);
		Vector3 directionToTarget = target - transform.position;

		transform.forward = directionToTarget;

		if (Input.GetButtonDown("Fire1"))
		{
			if (canShoot)
			{
				canShoot = false;
				Shoot();
			}
		}
	}
#endif

	void Shoot()
	{

		Projectile bullet = Instantiate(
			bulletPrefab,
			new Vector3(transform.position.x + (transform.forward.x * bulletSpawnOffset), transform.position.y, transform.position.z + (transform.forward.z * bulletSpawnOffset)),
			transform.rotation
		);
		bullet.ChangeToAllyMaterial();
		bullet.isMyProjectile = true;


		PlayerInfo.instance.totalBulletsFired++;
		Destroy(bullet, 5);


		StartCoroutine("DelayShoot");
	}

	IEnumerator DelayShoot()
	{
		yield return new WaitForSeconds(fireRate);
		canShoot = true;
	}
}
