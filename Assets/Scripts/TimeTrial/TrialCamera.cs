using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialCamera : MonoBehaviour
{
	public Vector3 offsetFromPlayer;
	private TrialPlayerMovement player;

	private void Start()
	{
		player = FindObjectOfType<TrialPlayerMovement>();
	}

	void Update()
	{
		if (player != null)
		{
			transform.position = new Vector3(
				player.transform.position.x + offsetFromPlayer.x,
				offsetFromPlayer.y,
				player.transform.position.z + offsetFromPlayer.z
			);
		}
	}
}
