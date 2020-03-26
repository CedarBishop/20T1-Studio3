using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsTracker : MonoBehaviour
{
	// Singleton initialization
	public static AnalyticsTracker instance = null;

	public ActiveSkills currentActive;
	public PassiveSkills currentPassive;

	private float roundDelayTime;

	private void Start()
	{
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy(gameObject);

		roundDelayTime = GameManager.instance.roundStartDelay;

		// Assign action delegates
		GameManager.BeginRoundEvent += BeginRoundEvent;
		GameManager.EndRoundEvent += EndRoundEvent;
	}

	/* Event to fire at the beginning of round */
	public void BeginRoundEvent()
	{
		// End of round collection of stats
		Analytics.CustomEvent(
			"abilities_used",
			new Dictionary<string, object>
			{
				{ "active_ability", currentActive.ToString() },
				{ "passive_ability", currentPassive.ToString() }
			}
		);
	}

	/* Event to fire at end of round */
	public void EndRoundEvent()
	{
		float totalRoundTime = Time.timeSinceLevelLoad - roundDelayTime;
		Debug.Log("Round time: " + totalRoundTime);

		// Add to the total amount of games played
		Analytics.CustomEvent("round_played");
		// End of round collection of stats
		Analytics.CustomEvent(
			"end_of_round",
			new Dictionary<string, object>
			{
				{ "time_elapsed", totalRoundTime }
			}
		);
	}

	private void OnDisable()
	{
		GameManager.BeginRoundEvent -= BeginRoundEvent;
		GameManager.EndRoundEvent -= EndRoundEvent;
	}
}
