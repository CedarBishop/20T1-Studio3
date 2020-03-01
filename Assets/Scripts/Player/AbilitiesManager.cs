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
	private Ability currentPassive; 							// Whatever current PASSIVE skill is tied to character this round
	private Ability currentActive; 								// Whatever current ACTIVE skill is tied to character this round

	[Header("Current Stats Section")]
	[SerializeField] private GameObject originalMaterial;		// STEALTH - Current GameObject material we want to change
	public float movementSpeed; 								// SPEED CHANGE - Current velocity/movement speed to increase/decrease

	[Header("Adjustments Section")] [SerializeField]
	private float damageIncreasePercentage; 					// DAMAGE CHANGE - Increases damage by a percentage
	[SerializeField] private float speedIncreasePercentage; 	// SPEED CHANGE - Increases damage by a percentage
	[SerializeField] private int maxBulletBounces; 				// BULLET BOUNCE - Increment how many times a projectable can bounce with trajectory before being destroyed
	private Material currentMaterial; 							// STEALTH - Where to store current material we want to change
	private Material revertMaterial; 							// STEALTH - Where to store original material to go back to
	[SerializeField] private Material stealthMaterial; 			// STEALTH - Assign invisibility shader for Stealth ability

	private void Awake()
	{
		// revertMaterial = originalMaterial.GetComponent<Material>();
		revertMaterial = originalMaterial.GetComponent<SkinnedMeshRenderer>().materials[0]; // TODO: Remove! This is only for ghost example prefab

		// Carry on with passive ability choice IF list is populated
		if (passiveAbilities.Length > 0)
		{
			// Go through list and pick random ability
			currentPassive = passiveAbilities[UnityEngine.Random.Range(0, passiveAbilities.Length - 1)];

			PassiveAbilityProcess(currentPassive);
		}

		// TODO: Hard coded temporarily (STEALTH)
		activeSkills = ActiveSkills.Stealth;
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
				case PassiveSkills.BouncyBullet:
					BouncyBullet(maxBulletBounces);
					break;
				case PassiveSkills.HelperBullet:
					HelperBullet();
					break;
				case PassiveSkills.SlowdownBullet:
					SlowdownBullet();
					break;
				case PassiveSkills.SpeedUp:
					SpeedUp();
					break;
				case PassiveSkills.TriShield:
					TriShield();
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

	private void HelperBullet() {}
	private void SlowdownBullet() {}
	private void SpeedUp() {}
	private void TriShield() {}

	public void ActivateAbility()
	{
		// If pressed and active ability assigned ...
		if (cooldownComplete && currentActive != null)
		{
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
					Stealth();
					break;
				case ActiveSkills.TempShield:
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

	private void DropMine() {}
	private void Rewind() {}
	private void Shotgun() {}

	private void Stealth()
	{
		if (currentMaterial != stealthMaterial)
		{
			currentMaterial = stealthMaterial;
		}
		else
		{
			currentMaterial = revertMaterial;
		}

		originalMaterial.GetComponent<SkinnedMeshRenderer>().material = currentMaterial; // TODO: Adjust! This is to suit dummy ghost prefab

		// Calls itself to change material back to original after set time
		StartCoroutine(StealthSwap());
	}

	IEnumerator StealthSwap()
	{
		yield return new WaitForSeconds(currentActive.cooldownTime);
		Stealth();
	}

	private void TempShield() {}
}
