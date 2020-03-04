﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    // runtime currency variables
    private int passion;
    private int gold;


    // Playe prefs keys
    private string passionKey = "Passion";
    private string goldKey = "Gold";

    private void Start()
    {
        // initialising runtime variables with values that were previously saved if any
        if (PlayerPrefs.HasKey(passionKey))
        {
            passion = PlayerPrefs.GetInt(passionKey, 0);
        }
        if (PlayerPrefs.HasKey(goldKey))
        {
            gold = PlayerPrefs.GetInt(goldKey, 0);
        }
    }


    // Passion Functions

    public void EarnPassion (int value)
    {
        // add to runtime passion and save it locally 
        passion += value;
        PlayerPrefs.SetInt(passionKey,passion);
    }

    public int GetPassion()
    {
        // Getter used for the UI
        return passion;
    }

    public bool SpendPassion (int value)
    {
        if (passion >= value) // can afford item
        {
            passion -= value; // spend passion
            PlayerPrefs.SetInt(passionKey, passion); // save new passion value locally
            return true;
        }
        else // cant afford item
        {
            return false;
        }
    }

    // Gold Functions

    public void EarnGold(int value)
    {
        // add to runtime gold and save it locally 
        gold += value;
        PlayerPrefs.SetInt(goldKey, gold);
    }

    public int GetGold()
    {
        // Getter used for the UI
        return gold;
    }

    public bool SpendGold(int value)
    {
        if (gold >= value) // can afford item
        {
            gold -= value; // spend gold
            PlayerPrefs.SetInt(goldKey, gold); // save new gold value locally
            return true;
        }
        else // cant afford item
        {
            return false;
        }
    }


}