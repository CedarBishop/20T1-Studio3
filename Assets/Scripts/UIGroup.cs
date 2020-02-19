using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGroup : MonoBehaviour
{
    public Text healthText;
    public Text playerNumberText;
    public Text roundWinText;
    public int health;
    public int playerNumber;
    public int roundWins;


    public void SetHealth(int newHealth)
    {
        health = newHealth;
        healthText.text = health.ToString() + "%";
    }

    public void SetPlayerNumber (int num)
    {         
        playerNumber = num;
        playerNumberText.text = "P" + playerNumber.ToString();
        playerNumberText.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 1.0f);
    }

    public void IncrementRoundWins()
    {
        roundWins++;
        roundWinText.text = roundWins.ToString();
    }
}
