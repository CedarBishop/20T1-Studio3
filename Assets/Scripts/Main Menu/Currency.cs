using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyProfile;

public class Currency : MonoBehaviour
{
    // runtime currency variables
    private int passion;
    private int gold;


    // Player prefs keys and value ID's for profile system
    private string passionKey = "Passion";
    private string goldKey = "Gold";

    private MainMenu mainMenu;

    private void Start()
    {
        mainMenu = GetComponent<MainMenu>();

        //initialising runtime variables with values that were previously saved if any

#if UNITY_IPHONE || UNITY_ANDROID
        
        if (PlayerPrefs.HasKey(passionKey))
        {
            passion = PlayerPrefs.GetInt(passionKey, 0);
        }

        if (PlayerPrefs.HasKey(goldKey))
        {
            gold = PlayerPrefs.GetInt(goldKey, 0);
        }
#else


        EasyProfileManager.Instance.GetCustomValue(passionKey, OnGetPassionComplete);
        EasyProfileManager.Instance.GetCustomValue(goldKey, OnGetGoldComplete);


#endif


        if (PlayerInfo.instance.passionEarnedThisMatch > 0)
        {
            EarnPassion(PlayerInfo.instance.passionEarnedThisMatch);
            mainMenu.UpdateCurrencyUI();
        }


        PlayerInfo.instance.ResetStats();
    }


    // Passion Functions

    public void EarnPassion (int value)
    {
        // add to runtime passion and save it locally 
        passion += value;
#if UNITY_IPHONE || UNITY_ANDROID
        PlayerPrefs.SetInt(passionKey,passion);
#else
        EasyProfileManager.Instance.SetCustomValue( passionKey, passion, OnSetPassionComplete);
#endif
        mainMenu.UpdateCurrencyUI(); // update main menu UI
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
#if UNITY_IPHONE || UNITY_ANDROID
            PlayerPrefs.SetInt(passionKey, passion); // save new passion value locally
#else
            EasyProfileManager.Instance.SetCustomValue(passionKey, passion, OnSetPassionComplete);
#endif
            mainMenu.UpdateCurrencyUI(); // update main menu UI
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
#if UNITY_IPHONE || UNITY_ANDROID
        PlayerPrefs.SetInt(goldKey, gold);
#else
        EasyProfileManager.Instance.SetCustomValue(goldKey, gold, OnSetGoldComplete);
#endif
        mainMenu.UpdateCurrencyUI(); // update main menu UI
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
#if UNITY_IPHONE || UNITY_ANDROID
            PlayerPrefs.SetInt(goldKey, gold); // save new gold value locally
#else
            EasyProfileManager.Instance.SetCustomValue(goldKey, gold, OnSetGoldComplete);
#endif
            //mainMenu.UpdateCurrencyUI(); // update main menu UI
            return true;
        }
        else // cant afford item
        {
            return false;
        }
    }

    private void OnGetPassionComplete(CallbackGetUserCustomValue msg)
    {
        if (msg.IsSuccess)
        {
            if (msg.ValueID == passionKey)
            {
                passion = msg.CustomValue.IntValue;
            }
            mainMenu.UpdateCurrencyUI();
            print("Successfully Got " + msg.ValueID);
        }
        else
        {
            Debug.Log("Failed to get user custom value");
            EasyProfileManager.Instance.SetCustomValue(passionKey, 0, OnSetPassionComplete);
        }
    }

    private void OnGetGoldComplete(CallbackGetUserCustomValue msg)
    {
        if (msg.IsSuccess)
        {
            if (msg.ValueID == goldKey)
            {
                gold = msg.CustomValue.IntValue;
            }
            mainMenu.UpdateCurrencyUI();
            print("Successfully Got " + msg.ValueID);
        }
        else
        {
            Debug.Log("Failed to get user custom value");
            EasyProfileManager.Instance.SetCustomValue(goldKey, 0, OnSetGoldComplete);
        }
    }

    private void OnSetPassionComplete(CallbackSetUserCustomValue msg)
    {
        if (msg.IsSuccess)
        {
            if (msg.ValueID == passionKey)
            {
                Debug.Log("Successfully saved passion");
                EasyProfileManager.Instance.GetCustomValue(passionKey, OnGetPassionComplete);

            }
        }
        else
        {
            Debug.Log("Failed to Set passion");
        }
    }

    private void OnSetGoldComplete(CallbackSetUserCustomValue msg)
    {
        if (msg.IsSuccess)
        {
            if (msg.ValueID == passionKey)
            {
                Debug.Log("Successfully saved gold");
                EasyProfileManager.Instance.GetCustomValue(goldKey,OnGetGoldComplete);
            }
        }
        else
        {
            Debug.Log("Failed to Set gold");
        }
    }
}
