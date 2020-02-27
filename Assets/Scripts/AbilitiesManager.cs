using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class AbilitiesManager : MonoBehaviour
{
	[Header("Abilities Lists")]
	// Enums to work with
	private PassiveSkills passiveSkills; 						// Our access to PASSIVE skills enum
	private ActiveSkills activeSkills; 							// Our access to ACTIVE skills enum
	[Space]
	public Ability[] passiveAbilities;	 						// Where ALL PASSIVE abilities objects live
	public Ability[] activeAbilities; 							// Where ALL ACTIVE abilities objects live

	[Header("Action Button Tracking")]
	public bool cooldownComplete = true; 						// Waiting for skill cooldown? (True by default - implies skill is ready)
	public Ability currentActive; 								// Whatever current ACTIVE skill is tied to character this round
	private Ability currentPassive; 							// Whatever current PASSIVE skill is tied to character this round

	[Header("Current Stats Section")] [SerializeField]
	private float movementSpeed; 								// SPEED CHANGE - Current velocity/movement speed to increase/decrease
	[SerializeField] private GameObject originalMaterial; 		// STEALTH - Current GameObject material we want to change
	private Material currentMaterial; 							// STEALTH - Where to store current material we want to change

	[Header("Adjustments Section")] [SerializeField]
	private float damageIncreasePercentage; 					// DAMAGE CHANGE - Increases damage by a percentage
	[SerializeField] private float speedIncreasePercentage; 	// SPEED CHANGE - Increases damage by a percentage
	[SerializeField] private int maxBulletBounces; 				// BULLET BOUNCE - Increment how many times a projectable can bounce with trajectory before being destroyed
	[SerializeField] private Material stealthMaterial; 			// STEALTH - Assign invisibility shader for Stealth ability

	private void Awake()
	{
		currentMaterial = originalMaterial.GetComponent<Material>();

		// Carry on with passive ability choice IF list is populated
		if (passiveAbilities.Length > 0)
		{
			// Go through list and pick random ability
			currentPassive = passiveAbilities[UnityEngine.Random.Range(0, passiveAbilities.Length - 1)];

			PassiveAbilityProcess(currentPassive);
		}

		// TODO: Hard coded temporarily (STEALTH)
		currentActive = activeAbilities[3];
	}

	private void PassiveAbilityProcess(Ability chosenPassive)
	{
		if (currentPassive != null)
		{
			// Assign enum by current random enum value
			passiveSkills = chosenPassive.passiveSkillId;

			switch (passiveSkills)
			{
				case PassiveSkills.None:
					break;
				case PassiveSkills.AimingForsight:
					AimingForsight();
					break;
				case PassiveSkills.BulletBounce:
					BulletBounce(maxBulletBounces);
					break;
				case PassiveSkills.DamageMultiplier:
					DamageMultiplier(damageIncreasePercentage);
					break;
				case PassiveSkills.SpeedMultiplier:
					SpeedMultiplier(speedIncreasePercentage);
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

	private void AimingForsight() {}

	// Bullets that bounce off the environment XX amount of times but destroy on people
	private void BulletBounce(int bulletBounces)
	{
		bulletBounces++;
		bulletBounces = Mathf.Clamp(bulletBounces, 0, maxBulletBounces);
	}

	private void DamageMultiplier(float multiplier) {}

	// Turn up movement speed slightly
	public void SpeedMultiplier(float multiplier)
	{
		Rigidbody playerRb = GetComponent<Rigidbody>();
		float playerSpeed = playerRb.velocity.magnitude;

		// TODO: Get client, adjust movement speed (probably don't need parameters or return type, just go direct)
		playerSpeed += Mathf.Clamp(playerSpeed, 0, (playerSpeed / 100f) * multiplier);
		Debug.Log(playerSpeed);
	}

	public void ActivateAbility()
	{
		// If pressed and active ability assigned ...
		if (cooldownComplete && currentActive != null)
		{
			switch (activeSkills)
			{
				case ActiveSkills.None:
					break;
				case ActiveSkills.AOEBlast:
					break;
				case ActiveSkills.EnemySlowdown:
					break;
				case ActiveSkills.QuickTransport:
					break;
				case ActiveSkills.Stealth:
					Stealth();
					break;
				default:
					break;
			}

			// Disable active abilities until specific cooldown is complete
			StartCoroutine(AbilityCooldown(currentActive));
		}
	}

	IEnumerator AbilityCooldown(Ability currentActive)
	{
		cooldownComplete = false;
		yield return new WaitForSeconds(currentActive.cooldownTime);
		cooldownComplete = true;
	}

	public void AoeBlast() {}
	public void EnemySlowdown() {}
	public void QuickTransport() {}

	public void Stealth()
	{
		// TODO: Clean up + assign things accordingly to actual project setup
		if (originalMaterial != stealthMaterial)
		{
			currentMaterial = stealthMaterial;
		}
		else
		{
			currentMaterial = originalMaterial.GetComponent<Material>();
		}
	}
}
