using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class FinalBossLoot : MonoBehaviour
{
	public string battleMvp = "Holtzmann";
	public int mvpLevel = 34;
	public float battleDurationInMinutes = 43.05f;

	private void Start()
	{
		Analytics.CustomEvent("final_boss_defeated");
		Analytics.CustomEvent("battle_mvp_selected", new Dictionary<string, object>()
		{
			{ "battle_mvp", battleMvp },
			{ "mvp_level", mvpLevel },
			{ "battle_time", battleDurationInMinutes }
		});

	}
}
