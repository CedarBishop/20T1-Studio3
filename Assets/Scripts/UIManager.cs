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
        
        for (int i = 0; i < players.Length; i++)
        {
           UIGroup uI = Instantiate(uIGroupPrefab,layoutGroup.transform);
            uIGroups.Add(uI);
            uI.playerNumberText.text = "P" + (i + 1).ToString();
            uI.playerNumber = i + 1;
            uI.playerNumberText.color = new Color(Random.Range(0,1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f),1.0f);
        }
    }

    public void HealthUpdate (int health, int playerNumber)
    {
        uIGroups[playerNumber - 1].health = health;
        uIGroups[playerNumber - 1].healthText.text =  health.ToString() + "%";
        int playerRemaining = 0;
        int possibleWinningPlayerNumber = 0;
        for (int i = 0; i < uIGroups.Count; i++)
        {
            if (uIGroups[i].health > 0)
            {
                playerRemaining++;
                possibleWinningPlayerNumber = i + 1;
            }
        }
        if (playerRemaining == 1)
        {
            winText.text = "Player " + possibleWinningPlayerNumber + " Wins!";
        }
    }
}
