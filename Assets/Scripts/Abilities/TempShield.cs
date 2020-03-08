using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShield : MonoBehaviour
{
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Projectile"))
		{
			Destroy(other.gameObject);
		}
	}
}
