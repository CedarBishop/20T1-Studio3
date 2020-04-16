using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	// Skill buttons actions
	public static event Action DestroySkillButtons;
	// Analytics actions
	public static event Action BeginRoundEvent;
	public static event Action EndRoundEvent;

	private PhotonView photonView;
	private int roomNumber;

	[Header("Player Info")]
	public PlayerStats statsPrefab;
	public Image playerOnePassiveSkillImage;
	public Image playerOneActionSkillImage;
	public Image playerTwoPassiveSkillImage;
	public Image playerTwoActionSkillImage;
	private Player[] players;
	private List<PlayerStats> playerStats = new List<PlayerStats>();

	[Header("Round Stats")]
	public float roundStartDelay = 0.24f;
	[HideInInspector] public bool isDoubleDamage;
	public Text winText;
	private int roundNumber = 1;
	public Text roundTimerText;
	private float roundTimer;
	private bool roundIsUnderway;
	private bool isRoundIntermission;

	[Header("UI Arrangement")]
	public LayoutGroup layoutGroup;
	public FixedJoystick leftJoystick;
	public FixedJoystick rightJoystick;
	public Button abilityButton;

	[Header("Skills Area")]
	public GameObject skillSelectionParent;
	public GameObject passiveSkillLayout;
	public GameObject activeSkillLayout;
	public GameObject skillButtonPrefab;

	private bool hasSelectedPassive;
	private bool hasSelectedAction;
	private bool isAbleToSelectSkill;

	private void Awake()
	{
		// Make Script Singleton
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		// Show/hide mobile joysticks
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WEBGL
		leftJoystick.gameObject.SetActive(true);
		rightJoystick.gameObject.SetActive(true);
		abilityButton.gameObject.SetActive(true);
#elif UNITY_EDITOR || UNITY_STANDALONE
		leftJoystick.gameObject.SetActive(false);
		rightJoystick.gameObject.SetActive(false);
		abilityButton.gameObject.SetActive(false);
#endif
	}

	private IEnumerator Start()
	{
		AnalyticsTracker.instance.Init();
		playerOnePassiveSkillImage.gameObject.SetActive(false);
		playerOneActionSkillImage.gameObject.SetActive(false);
		playerTwoPassiveSkillImage.gameObject.SetActive(false);
		playerTwoActionSkillImage.gameObject.SetActive(false);

		skillSelectionParent.SetActive(false);
		winText.text = "";
		yield return new WaitForSeconds(roundStartDelay);
		players = PhotonNetwork.PlayerList;
		roundNumber = 1;

		// Instantiate the UI Group for each player and initialize with room number
		for (int i = 0; i < players.Length; i++)
		{
			PlayerStats stats = Instantiate(statsPrefab, layoutGroup.transform);
			playerStats.Add(stats);
			stats.SetPlayerNumber(i + 1);

			if (i == 0)
			{
				stats.transform.SetAsFirstSibling();
			}
		}

		photonView = GetComponent<PhotonView>();
		SetPlayerName();
		StartRoundTimer();
	}

	void SetPlayerName ()
	{
		if (int.TryParse(PhotonNetwork.NickName, out int num))
		{
			string str = PhotonRoom.instance.nickName;
			roomNumber = num;
			photonView.RPC("RPC_SetPlayerName",RpcTarget.All , num, str);
		}

	}

	[PunRPC]
	void RPC_SetPlayerName (int playerNum, string name)
	{
		print(playerNum + name);
		playerStats[playerNum - 1].SetPlayerName(name);

	}

	// Called when a player dies
	public void PlayerDied(int dyingPlayerNumber, int sendingPlayerNumber)
	{
		roomNumber = sendingPlayerNumber;
		string displayText = "";
		//player two dies
		if (dyingPlayerNumber == 2)
		{
			//player one wins the match
			displayText = "Player One Wins The Round ";
			if (playerStats[0].IncrementRoundWins())
			{
				displayText = "Player One Wins";
				if (int.TryParse(PhotonNetwork.NickName, out int num))
				{
					if (dyingPlayerNumber != sendingPlayerNumber)
					{
						EarnPassion(true);
					}
					else
					{
						EarnPassion(false);
					}
				}
				StartCoroutine("CoEndMatch");
			}
			//player one wins the round but not the match
			else
			{
				if (dyingPlayerNumber == sendingPlayerNumber)
				{
					SoundManager.instance.PlaySFX("LoseGame");
					SpawnSkillSelectionButtons();
				}
				else
				{
					SoundManager.instance.PlaySFX("WinGame");
				}

				//Start Intermission between rounds
				Intermission();
			}
		}
		//player one dies
		else
		{
			displayText = "Player Two Wins The Round ";
			//player two wins the match
			if (playerStats[1].IncrementRoundWins())
			{
				if (int.TryParse(PhotonNetwork.NickName, out int num))
				{
					if (dyingPlayerNumber != sendingPlayerNumber)
					{
						EarnPassion(true);
					}
					else
					{
						EarnPassion(false);
					}
				}
				displayText = "Player Two Wins";
				StartCoroutine("CoEndMatch");
			}
			//player two wins the round but not the match
			else
			{
				if (dyingPlayerNumber == sendingPlayerNumber)
				{
					SoundManager.instance.PlaySFX("LoseGame");
					SpawnSkillSelectionButtons();
				}
				else
				{
					SoundManager.instance.PlaySFX("WinGame");
				}

				Intermission();
			}
		}

		print(displayText);
		//Display text of who won the round or match
		winText.text = displayText;
	}

	public void ClearWinText()
	{
		roundNumber++;
		winText.text = "";
	}

	private IEnumerator CoEndMatch()
	{
		yield return new WaitForSeconds(3);
		PhotonRoom.instance.EndMatch();
	}

	public void PlayerForfeited(int playerNumber)
	{
		string displayText = "";
		if (playerNumber == 1)
		{
			displayText = "Player One Forfeited \nPlayer Two Wins";
		}
		else if (playerNumber == 2)
		{
			displayText = "Player Two Forfeited \nPlayer One Wins";
		}
		if (EndRoundEvent != null) EndRoundEvent(); // Send data to analytics (round time)
		EarnPassion(true);
		StartCoroutine("CoEndMatch");
		winText.text = displayText;
	}

	private void FixedUpdate()
	{
		if (LevelManager.instance.isInLobby) // if in lobby dont use timer
		{
			roundTimerText.text = "";
			return;
		}
		if (roundIsUnderway)
		{
			if (roundTimer <= 20.0f)
			{
				if (isDoubleDamage == false)
				{
					isDoubleDamage = true;
					LevelManager.instance.OnDoubleDamageDrop();
					SoundManager.instance.PlaySFX("CrowdCheering");
				}
			}
			if (roundTimer <= 0)
			{
				// Send data to analytics (complete round time)
				if (EndRoundEvent != null) EndRoundEvent();

				if (PhotonNetwork.IsMasterClient)
				{
					roundIsUnderway = false;
					isRoundIntermission = false;
					photonView.RPC("RPC_RoundDraw", RpcTarget.All);
					print("Stopped timer");
				}
			}
			else
			{
				roundTimer -= Time.fixedDeltaTime;
				roundTimerText.text = roundTimer.ToString("F1");
			}
		}
		else if (isRoundIntermission)
		{
			if (roundTimer <= 0)
			{

				if (PhotonNetwork.IsMasterClient)
				{
					isRoundIntermission = false;
					RPC_StartNewRound();
					photonView.RPC("RPC_StartNewRound", RpcTarget.Others);
				}
			}
			else
			{
				roundTimer -= Time.fixedDeltaTime;
				roundTimerText.text = roundTimer.ToString("F1");
			}
		}
	}

	public void StartRoundTimer()
	{
		// Both player are at match point
		if (playerStats[0].roundWins >= LevelManager.instance.requiredRoundsToWinMatch - 1 && playerStats[1].roundWins >= LevelManager.instance.requiredRoundsToWinMatch - 1)
		{
			SoundManager.instance.PlayMusic(false, true);
		}
		else // Not final round
		{
			SoundManager.instance.PlayMusic(false);
		}

		LevelManager.instance.OnBeginingOfRound();
		roundTimer = LevelManager.instance.roundTime;
		roundTimerText.text = roundTimer.ToString("F1");
		roundIsUnderway = true;
		isRoundIntermission = false;
		isAbleToSelectSkill = false;
	}

	[PunRPC]
	private void RPC_StartNewRound()
	{
		
		
		if (isAbleToSelectSkill)
		{
			AssignRandomSkill();
			isAbleToSelectSkill = false;
		}


		StartRoundTimer();
		ClearWinText();

		if (DestroySkillButtons != null)
		{
			DestroySkillButtons();
		}

		
		
		AvatarSetup[] avatarSetups = FindObjectsOfType<AvatarSetup>();
		for (int i = 0; i < avatarSetups.Length; i++)
		{
			if (avatarSetups[i].GetComponent<PhotonView>().IsMine)
			{
				avatarSetups[i].StartNewRound();
			}
		}
	}

	[PunRPC]
	private void RPC_RoundDraw()
	{
		print("RPC_RoundDraw");
		if (playerStats.Count >= 2)
		{
			// Increment both players and check if have caused a tie break
			playerStats[0].IncrementRoundWins();
			playerStats[1].IncrementRoundWins();

			if (playerStats[0].roundWins >= LevelManager.instance.requiredRoundsToWinMatch &&
			    playerStats[1].roundWins >= LevelManager.instance.requiredRoundsToWinMatch)
			{
				// tie break, go to sudden death
				roundTimerText.text = "";
			}

			else if (playerStats[0].roundWins >= LevelManager.instance.requiredRoundsToWinMatch)
			{
				// Player 1 wins match
				if (int.TryParse(PhotonNetwork.NickName, out int num))
				{
					if (num == 1)
					{
						EarnPassion(true);
					}
					else if (num == 2)
					{
						EarnPassion(false);
					}
				}
				if (EndRoundEvent != null) EndRoundEvent(); // Send data to analytics (end of round counter)
				winText.text = "Player One Wins";
				StartCoroutine("CoEndMatch");
			}
			else if (playerStats[1].roundWins >= LevelManager.instance.requiredRoundsToWinMatch)
			{
				// Player Two wins match
				if (int.TryParse(PhotonNetwork.NickName, out int num))
				{
					if (num == 1)
					{
						EarnPassion(false);
					}
					else if (num == 2)
					{
						EarnPassion(true);
					}
				}
				if (EndRoundEvent != null) EndRoundEvent(); // Send data to analytics (end of round counter)
				winText.text = "Player Two Wins";
				StartCoroutine("CoEndMatch");
			}
			else
			{
				// go to next round

				Intermission();
			}
		}
		else
		{
			Intermission();
		}
	}


	private void Intermission()
	{
		SoundManager.instance.StopMusic();
		SoundManager.instance.PlaySFX("Transistion");
		roundTimer = LevelManager.instance.intermissionTime;
		isRoundIntermission = true;
		roundIsUnderway = false;
		roundTimerText.text = "";
		roundTimerText.text = "Intermission";

		AvatarSetup[] avatarSetups = FindObjectsOfType<AvatarSetup>();
		if (avatarSetups == null)
		{
			return;
		}

		for (int i = 0; i < avatarSetups.Length; i++)
		{
			if (avatarSetups[i].GetComponent<PhotonView>().IsMine)
			{
				avatarSetups[i].DisableControls();
			}
		}

	}


	public void SkillSelectButton(bool isPassive, int skillNumber)
	{
		// assign skill to player and set icon on ui
		AssignSkill(isPassive,skillNumber);
		SoundManager.instance.PlaySFX("SkillSelect");
		if (isPassive)
		{
			SkillSelectionHolder.instance.RemovePassiveSkill(skillNumber);
			hasSelectedPassive = true;
		}
		else
		{
			SkillSelectionHolder.instance.RemoveActiveSkill(skillNumber);
			hasSelectedAction = true;
		}
		if (DestroySkillButtons != null)
		{
			DestroySkillButtons();
		}

		skillSelectionParent.SetActive(false);
	}

	private void SpawnSkillSelectionButtons()
	{
		skillSelectionParent.SetActive(true);
		isAbleToSelectSkill = true;
		if (hasSelectedPassive == false)
		{
			PassiveSkills[] passiveSkills = SkillSelectionHolder.instance.GetPassiveSkills();
			for (int i = 0; i < passiveSkills.Length; i++)
			{
				Button button = Instantiate(skillButtonPrefab, passiveSkillLayout.transform).GetComponent<Button>();
				button.GetComponent<SkillButton>().InitialiseButton(true, i);
			}
		}

		if (hasSelectedAction == false)
		{
			ActiveSkills[] activeSkills = SkillSelectionHolder.instance.GetActiveSkills();

			for (int i = 0; i < activeSkills.Length; i++)
			{
				Button button = Instantiate(skillButtonPrefab, activeSkillLayout.transform).GetComponent<Button>();
				button.GetComponent<SkillButton>().InitialiseButton(false,i);
			}
		}
	}

	private void AssignSkill(bool isPassive, int skillNumber)
	{

		isAbleToSelectSkill = false;
		AbilitiesManager[] abilitiesManager = FindObjectsOfType<AbilitiesManager>();
		if (isPassive)
		{
			PassiveSkills[] passives = SkillSelectionHolder.instance.GetPassiveSkills();
			if (passives != null)
			{
				int num = SkillSelectionHolder.instance.GetChosenPassiveSkillSprite(passives[skillNumber]);
				photonView.RPC("RPC_AssignSkillIcon", RpcTarget.All, roomNumber, true, num);

				if (abilitiesManager != null)
				{
					for (int i = 0; i < abilitiesManager.Length; i++)
					{
						if (abilitiesManager[i].GetComponent<PhotonView>().IsMine)
						{
							abilitiesManager[i].AssignPassiveSkill( SkillSelectionHolder.instance.GetPassiveSkills()[skillNumber]);
							SoundManager.instance.PlaySFX("SkillSelect");
							AnalyticsTracker.instance.currentPassive = abilitiesManager[i].passiveSkills;
						}
					}
				}
			}
		}
		else
		{
			ActiveSkills[] actives = SkillSelectionHolder.instance.GetActiveSkills();
			if (actives != null)
			{
				int num = SkillSelectionHolder.instance.GetChosenActiveSkillSprite(actives[skillNumber]);
				photonView.RPC("RPC_AssignSkillIcon", RpcTarget.All, roomNumber, false, num);

				if (abilitiesManager != null)
				{
					for (int i = 0; i < abilitiesManager.Length; i++)
					{
						if (abilitiesManager[i].GetComponent<PhotonView>().IsMine)
						{
							abilitiesManager[i].AssignActiveSkill(SkillSelectionHolder.instance.GetActiveSkills()[skillNumber]);
							AnalyticsTracker.instance.currentActive = abilitiesManager[i].activeSkills;
						}
					}
				}
			}
		}

		if (BeginRoundEvent != null) BeginRoundEvent(); // Send data to analytics (Which skills are chosen)
	}

	void AssignRandomSkill ()
	{
		PassiveSkills[] passives = SkillSelectionHolder.instance.GetPassiveSkills();
		ActiveSkills[] actives = SkillSelectionHolder.instance.GetActiveSkills();
		if (hasSelectedPassive == false && hasSelectedAction == false)
		{

		 	int randBool = UnityEngine.Random.Range(0,2);
			if (randBool == 1)
			{
				int randNum = UnityEngine.Random.Range(0, passives.Length);
				AssignSkill(true,randNum);
				SkillSelectionHolder.instance.RemovePassiveSkill(randNum);
				hasSelectedPassive = true;
			}
			else
			{
				int randNum = UnityEngine.Random.Range(0, actives.Length);
				AssignSkill(false, randNum);
				SkillSelectionHolder.instance.RemoveActiveSkill(randNum);
				hasSelectedAction = true;
			}

		}
		else if (hasSelectedPassive == false)
		{
			int randNum = UnityEngine.Random.Range(0, passives.Length);
			AssignSkill(true, randNum);
			SkillSelectionHolder.instance.RemovePassiveSkill(randNum);
			hasSelectedPassive = true;
		}
		else if (hasSelectedAction == false)
		{
			int randNum = UnityEngine.Random.Range(0, actives.Length);
			AssignSkill(false, randNum);
			SkillSelectionHolder.instance.RemoveActiveSkill(randNum);
			hasSelectedAction = true;
		}
	}

	[PunRPC]
	private void RPC_AssignSkillIcon (int playerNum, bool isPassive, int num)
	{
		print("RPC Skill was called");
		if (playerNum == 1)
		{
			if (isPassive)
			{
				playerOnePassiveSkillImage.gameObject.SetActive(true);
				playerOnePassiveSkillImage.sprite = SkillSelectionHolder.instance.passiveSprites[num];
			}
			else
			{
				playerOneActionSkillImage.gameObject.SetActive(true);
				playerOneActionSkillImage.sprite = SkillSelectionHolder.instance.activeSprites[num];
			}
		}
		else if (playerNum == 2)
		{
			if (isPassive)
			{
				playerTwoPassiveSkillImage.gameObject.SetActive(true);
				playerTwoPassiveSkillImage.sprite = SkillSelectionHolder.instance.passiveSprites[num];
			}
			else
			{
				playerTwoActionSkillImage.gameObject.SetActive(true);
				playerTwoActionSkillImage.sprite = SkillSelectionHolder.instance.activeSprites[num];
			}
		}
	}

	private void EarnPassion(bool wonMatch)
	{
		if (wonMatch)
		{
			PlayerInfo.playerInfo.passionEarnedThisMatch = 10;
		}
		else
		{
			PlayerInfo.playerInfo.passionEarnedThisMatch = 5;
		}
	}

	public void ActionSkillCooldownDisplay (float cooldownTime)
	{
		photonView.RPC("RPC_ActionSkillCooldownDisplay", RpcTarget.All, roomNumber, cooldownTime);
	}

	[PunRPC]
	void RPC_ActionSkillCooldownDisplay (int playerNumber, float cooldown)
	{
		StartCoroutine(CoActionSkillCooldownDisplay(playerNumber,cooldown));
	}

	IEnumerator CoActionSkillCooldownDisplay (int playerNumber, float cooldown)
	{
		float rate = 1;
		if (cooldown != 0)
		{
			rate = (1 / cooldown);
		}

		if (playerNumber == 1)
		{
			playerOneActionSkillImage.fillAmount = 0;

			while (playerOneActionSkillImage.fillAmount < 1.0f)
			{
				playerOneActionSkillImage.fillAmount += rate * Time.deltaTime;
				yield return null;
			}
		}
		else if (playerNumber == 2)
		{
			playerTwoActionSkillImage.fillAmount = 0;

			while (playerTwoActionSkillImage.fillAmount < 1.0f)
			{
				playerTwoActionSkillImage.fillAmount += rate * Time.deltaTime;
				yield return null;
			}
		}

	}
}

