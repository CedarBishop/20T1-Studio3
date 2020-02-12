using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo playerInfo = null;

    public int selectedCharacter;

    public GameObject[] allCharacters;

    [HideInInspector]public string selectedCharacterKey = "SelectedCharacter";

    private void Awake()
    {
        if (playerInfo == null)
        {
            playerInfo = this;
        }
        else if (playerInfo != this)
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
}
