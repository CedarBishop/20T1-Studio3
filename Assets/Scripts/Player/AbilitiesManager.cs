﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement)), DisallowMultipleComponent]
public class AbilitiesManager : MonoBehaviour
{
	public Button abilityButton;

	[Header("Abilities Lists")]
	// Enums to work with
	private PassiveSkills passiveSkills; 					// Our access to PASSIVE skills enum
	private ActiveSkills activeSkills; 						// Our access to ACTIVE skills enum
	[Space]
	public Ability[] passiveAbilities;	 					// Where ALL PASSIVE abilities objects live
	public Ability[] activeAbilities; 						// Where ALL ACTIVE abilities objects live

	private delegate void AbilityDelegate();
	private AbilityDelegate methodToCall = null;

	[Header("Action Button Tracking")]
	public bool cooldownComplete = true; 					// Waiting for skill cooldown? (True by default - implies skill is ready)
	private Ability currentPassive; 						// Whatever current PASSIVE skill is tied to character this round
	private Ability currentActive; 							// Whatever current ACTIVE skill is tied to character this round

	[Header("Current Stats Section")]
	[SerializeField] private GameObject originalMaterial;	// STEALTH - Current GameObject material we want to change
	private float movementSpeed; 							// SPEED UP - Current velocity/movement speed to increase/decrease

	[Header("Adjustments Section")]
	[SerializeField] [Range(0f, 5f)] private float speedIncreasePercentage; // SPEED UP - Increases damage by a percentage
	[SerializeField] private float maxMovementSpeed;		// SPEED UP - Increases damage by a percentage
	[SerializeField] private int maxBulletBounces; 			// BULLET BOUNCE - Increment how many times a projectable can bounce with trajectory before being destroyed
	[Space]
	private Material currentMaterial; 						// STEALTH - Where to store current material we want to change
	private Material revertMaterial; 						// STEALTH - Where to store original material to go back to
	[SerializeField] private Material stealthMaterial; 		// STEALTH - Assign invisibility shader for Stealth ability

	private bool shieldActive;
	[SerializeField] private GameObject shieldEffect;

	private void Awake()
	{
		movementSpeed = GetComponent<PlayerMovement>().movementSpeed;

		// revertMaterial = originalMaterial.GetComponent<Material>();
		revertMaterial = originalMaterial.GetComponent<SkinnedMeshRenderer>().materials[0]; // TODO: Remove! This is only for ghost example prefab

		shieldEffect.SetActive(false);

		// Carry on with passive ability choice IF list is populated
		if (passiveAbilities.Length > 0)
		{
			// Go through list and pick random ability
			// currentPassive = passiveAbilities[UnityEngine.Random.Range(0, passiveAbilities.Length - 1)]; // TODO: End product needs this - allows random passive ability assignment
			currentPassive = passiveAbilities[3];

			PassiveAbilityProcess(currentPassive);
		}

		// TODO: Hard coded temporarily (STEALTH)
		activeSkills = ActiveSkills.TempShield;
		currentActive = activeAbilities[4];
	}

	private void PassiveAbilityProcess(Ability chosenPassive)
	{
		if (chosenPassive != null)
		{
			// Assign enum by current random enum value
			passiveSkills = chosenPassive.passiveSkillId;

			switch (passiveSkills)
			{
				case PassiveSkills.None:
					break;
				case PassiveSkills.BouncyBullet:
					BouncyBullet(maxBulletBounces);
					break;
				case PassiveSkills.HelperBullet:
					break;
				case PassiveSkills.SlowdownBullet:
					break;
				case PassiveSkills.SpeedUp:
					SpeedUp();
					break;
				case PassiveSkills.TriShield:
					break;
				default:
					break;
			}
		}
		else
		{
			throw new NotImplementedException();
		}
	}

	// Bullets that bounce off the environment XX amount of times but destroy on people
	private void BouncyBullet(int bulletBounces)
	{
		bulletBounces++;
		bulletBounces = Mathf.Clamp(bulletBounces, 0, maxBulletBounces);
	}

	private void SpeedUp()
	{
		float moveSpeed = GetComponent<PlayerMovement>().movementSpeed;
		moveSpeed *= speedIncreasePercentage;
		moveSpeed = Mathf.Clamp(moveSpeed, 0f, maxMovementSpeed);
	}

	public void ActivateAbility()
	{
		// If pressed and active ability assigned ...
		if (cooldownComplete && currentActive != null)
		{
			// Disable active abilities until specific cooldown is complete
			switch (activeSkills)
			{
				case ActiveSkills.None:
					break;
				case ActiveSkills.DropMine:
					break;
				case ActiveSkills.Rewind:
					break;
				case ActiveSkills.Shotgun:
					break;
				case ActiveSkills.Stealth:
					methodToCall = Stealth;
					StartCoroutine(AbilityDuration(currentActive, methodToCall));
					StartCoroutine(AbilityCooldown(currentActive, methodToCall));
					break;
				case ActiveSkills.TempShield:
					methodToCall = TempShield;
					StartCoroutine(AbilityDuration(currentActive, methodToCall));
					StartCoroutine(AbilityCooldown(currentActive, methodToCall));
					break;
				default:
					break;
			}

			methodToCall = null;
		}
	}

	IEnumerator AbilityDuration(Ability currentActive, AbilityDelegate methodToCall)
	{
		methodToCall(); // Activate skill
		yield return new WaitForSeconds(currentActive.duration);
		methodToCall(); // Deactivate skill
	}

	IEnumerator AbilityCooldown(Ability currentActive, AbilityDelegate methodToCall)
	{
		cooldownComplete = false; // Deactivate button
		yield return new WaitForSeconds(currentActive.cooldownTime);
		cooldownComplete = true; // Reactivate button
	}

	private void Stealth()
	{
		if (currentMaterial != stealthMaterial)
			currentMaterial = stealthMaterial;
		else
			currentMaterial = revertMaterial;

		originalMaterial.GetComponent<SkinnedMeshRenderer>().material = currentMaterial; // TODO: Adjust! This is to suit dummy ghost prefab
	}

	private void TempShield()
	{
		shieldActive = !shieldActive;
		// Vector3 shieldFullSize = new Vector3(1, 1, 1); // TODO: For use with interpolating between sizes

		if (shieldActive)
		{
			// Instantiate & apply effect (growing for active)
			shieldEffect.SetActive(true);
			Debug.Log("Shield is active");
		}
		else
		{
			// Apply effect & destroy (shrinking then deactivate)
			shieldEffect.SetActive(false);
			Debug.Log("Shield NOT active");
		}
	}
}
