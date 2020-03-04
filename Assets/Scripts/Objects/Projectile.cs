﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.Universal;

public class Projectile : MonoBehaviour
{
	PhotonView photonView;
	Rigidbody rigidbody;
	public int damage;
	public float force;
	public bool isMyProjectile;
	Vector3 _direction;

	[SerializeField]
	private ParticleSystem sparks; // For when bouncing off walls

	void Start()
	{
		photonView = GetComponent<PhotonView>();
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddForce(force * transform.forward);
		StartCoroutine("DelayedDestroy");
	}


	IEnumerator DelayedDestroy()
	{
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.GetComponentInParent<PlayerCombat>())
		{
			if (isMyProjectile)
			{
				if (collision.GetComponentInParent<PhotonView>().IsMine)
				{
					return;
				}
			}

			if (collision.GetComponentInParent<PhotonView>())
			{
				if (collision.GetComponentInParent<PhotonView>().IsMine)
				{
					collision.GetComponentInParent<PlayerCombat>().TakeDamage(damage);

					print("hit by enemy");
				}
			}
		}


		if (collision.gameObject.CompareTag("Wall"))
		{
			if (sparks != null)
			{
				sparks.Play();

			}
		}


		Destroy(gameObject);
	}

	public void SetEmissionColour(Color color)
	{
		Material material = GetComponent<MeshRenderer>().material;
		material.SetColor("_EmissionColor", color);
	}
}