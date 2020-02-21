using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public UIGroup uIGroupPrefab;
    public LayoutGroup layoutGroup;
    private Player[] players;
    private List<UIGroup> uIGroups = new List<UIGroup>();
    public Text winText;
    int roundNumber = 1;
    public Text roundNumberText;


    // Make Script Singleton
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
    }


    private IEnumerator Start()
    {
        winText.text = "";
        yield return new WaitForSeconds(0.24f);
        players = PhotonNetwork.PlayerList;
        roundNumber = 1;
        roundNumberText.text = "Round " + roundNumber.ToString();
        
        // Insatiate the UI Group for each player and initialize with room number
        for (int i = 0; i < players.Length; i++)
        {
           UIGroup uI = Instantiate(uIGroupPrefab,layoutGroup.transform);
            uIGroups.Add(uI);
            uI.SetPlayerNumber(i + 1);
            if (i == 0)
            {
                uI.transform.SetAsFirstSibling();
            }
        }
        
    }


    // Function that is called from player combat to 
    public void HealthUpdate (int health, int playerNumber)
    {
        uIGroups[playerNumber - 1].SetHealth(health);
        if (health <= 0)
        {
            DisplayWinText(playerNumber);
        }
    }

    public void DisplayWinText (int playerNumber)
    {
        string displayText = "";
        if (playerNumber == 2)
        {
            displayText = "Player One Wins Round " + roundNumber.ToString();
            if (uIGroups[0].IncrementRoundWins())
            {
                displayText = "Player One Wins";
                StartCoroutine("CoEndMatch");
            }

        }
        else
        {
            
            displayText = "Player Two Wins Round " + roundNumber.ToString();
            if (uIGroups[1].IncrementRoundWins())
            {
                displayText = "Player Two Wins";
                StartCoroutine("CoEndMatch");
            }
        }

        winText.text = displayText;
        
    }

    public void ClearWinText()
    {
        roundNumber++;
        roundNumberText.text = "Round " + roundNumber.ToString();
        winText.text = "";
    }

    IEnumerator CoEndMatch ()
    {
        yield return new WaitForSeconds(3);
        PhotonRoom.photonRoom.EndMatch();
    }

    public void PlayerForfeited (int playerNumber)
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
        StartCoroutine("CoEndMatch");
        winText.text = displayText;
    }


}