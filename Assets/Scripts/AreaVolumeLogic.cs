using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaVolumeLogic : MonoBehaviour
{

	private Dictionary<string, object> panelTimeContainer;
	private string panelName;
	private float timeEntered, timeLeft;
	[SerializeField] public float timeSpent;

	private void Start()
	{
		panelName = gameObject.name;
		panelTimeContainer = AnalyticsTracker.instance.areaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (AnalyticsTracker.instance != null && other.gameObject.CompareTag("Player"))
		{
			timeEntered = Time.time;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (AnalyticsTracker.instance != null && other.gameObject.CompareTag("Player"))
		{
			timeLeft = Time.time;
			timeSpent += timeLeft - timeEntered;

			object value = null;
			if (panelTimeContainer.TryGetValue(panelName, out value))
			{
				// Change value for time spent
				panelTimeContainer[gameObject.name.ToString()] = timeSpent;
			}
			else
			{
				// Create entry with value if one doesn't exist
				panelTimeContainer.Add(gameObject.name.ToString(), timeSpent);
			}
		}
	}
}
