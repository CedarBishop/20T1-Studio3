using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
	public static PlayerInfo instance = null;
	public int selectedCharacter;
	public GameObject[] allCharacters;

	[HideInInspector] public int passionEarnedThisMatch;
	[HideInInspector] public int totalBulletsFired;
	[HideInInspector] public int totalBulletsLanded;
	[HideInInspector] public float totalTimeTrialTime;
	[HideInInspector] public int timeTrialScore;
	[HideInInspector] public int timeTrialRound;

	[HideInInspector] public string selectedCharacterKey = "SelectedCharacter";


	public void ResetStats ()
	{
		passionEarnedThisMatch = 0;
		totalBulletsFired = 0;
		totalBulletsLanded = 0;
		totalTimeTrialTime = 0;
		timeTrialScore = 0;
		timeTrialRound = 0;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}


	void Start()
	{
		if (PlayerPrefs.HasKey(selectedCharacterKey))
		{
			selectedCharacter = PlayerPrefs.GetInt(selectedCharacterKey);
		}
		else
		{
			selectedCharacter = 0;
			PlayerPrefs.SetInt(selectedCharacterKey, selectedCharacter);
		}
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		// Instantiate(Resources.Load("PhotonPrefabs/Room Controller"));
	}
}
