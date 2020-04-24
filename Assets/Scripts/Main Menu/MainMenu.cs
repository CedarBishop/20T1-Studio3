using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainParent;
    public GameObject settingsParent;
    public GameObject shopParent;
    public GameObject skillParent;

    public Button logoutButton;

    public Text passionCountText;
    public Text goldCountText;

    public Shop shop;
    private Currency currency;

    public Slider musicSlider;
    public Slider sfxSlider;

    public InputField roomNumberInputField;
    public InputField nicknameInputField;

    private int characterNumber;

    public GameObject[] currentCharacterDisplayObjects;
    public GameObject lockedCharacterDisplay;
    public string[] cosmeticKeyNames;
    bool currentCharacterIsUnlocked;
    
    void Start()
    {
        SetMenuType(1);
        currency = GetComponent<Currency>();
        InitText();
        characterNumber = 0;
        ActivateCharacterDisplay();
        currentCharacterIsUnlocked = true;

#if UNITY_ANDROID || UNITY_IOS

        logoutButton.gameObject.SetActive(false);
#else
        logoutButton.gameObject.SetActive(true);

#endif

    }

    public void Logout  ()
    {
        EasyProfile.EasyProfileManager.Instance.LogOut();
        SoundManager.instance.PlaySFX("Button");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }

    public void SetMenuType(int menuType)
    {
        switch (menuType)
        {
            case 1:
                mainParent.SetActive(true);
                settingsParent.SetActive(false);
                shopParent.SetActive(false);
                skillParent.SetActive(false);
                ActivateCharacterDisplay();
                break;
            case 2:
                mainParent.SetActive(false);
                settingsParent.SetActive(true);
                shopParent.SetActive(false);
                skillParent.SetActive(false);
                DeactivateCharacterDisplay();
                break;
            case 3:
                mainParent.SetActive(false);
                settingsParent.SetActive(false);
                shopParent.SetActive(true);
                skillParent.SetActive(false);
                DeactivateCharacterDisplay();
                break;
            case 4:
                mainParent.SetActive(false);
                settingsParent.SetActive(false);
                shopParent.SetActive(false);
                skillParent.SetActive(true);
                DeactivateCharacterDisplay();
                break;
            default:
                break;
        }
        SoundManager.instance.PlaySFX("Button");
    }

    public void UpdateCurrencyUI ()
    {
        passionCountText.text = currency.GetPassion().ToString();
        goldCountText.text = currency.GetGold().ToString();
    }

    public void SetMusicVolume()
    {
        SoundManager.instance.SetMusicVolume(musicSlider.value);
    }

    public void SetSFXVolume()
    {
        SoundManager.instance.SetSFXVolume(sfxSlider.value);
    }

    public void GetMoney ()
    {
        currency.EarnPassion(10);
        currency.EarnGold(10);
    }

    public void ClearMoneyAndSkins ()
    {
        currency.SpendPassion(currency.GetPassion());
        currency.SpendGold(currency.GetGold());

        for (int i = 0; i < cosmeticKeyNames.Length; i++)
        {
            PlayerPrefs.SetInt(cosmeticKeyNames[i],0);
        }
        shop.InitShop();
    }

    void InitText()
    {
        if (PlayerPrefs.HasKey("Passion"))
        {
            passionCountText.text = PlayerPrefs.GetInt("Passion", 0).ToString();

        }
        if (PlayerPrefs.HasKey("Gold"))
        {
            goldCountText.text = PlayerPrefs.GetInt("Gold",0).ToString();
        }
    }

    public void NextCharacter ()
    {
        SoundManager.instance.PlaySFX("Button");
        characterNumber++;
        if (characterNumber > currentCharacterDisplayObjects.Length - 1)
        {
            characterNumber = 0;
        }
    
     
       
        ActivateCharacterDisplay();

        if (characterNumber > 0)
        {
            if (shop.shopItems[characterNumber - 1].owned == false)
            {
                return;
            }
        }

        if (PlayerInfo.instance != null)
        {
            PlayerInfo.instance.selectedCharacter = characterNumber;
            PlayerPrefs.SetInt(PlayerInfo.instance.selectedCharacterKey, characterNumber);

        }
    }


    void ActivateCharacterDisplay ()
    {
        if (currentCharacterDisplayObjects == null)
        {
            return;
        }

        // Deativate all character display objects
        for (int i = 0; i < currentCharacterDisplayObjects.Length; i++)
        {
            currentCharacterDisplayObjects[i].SetActive(false);     

        }
        lockedCharacterDisplay.SetActive(false);

        if (characterNumber > 0)
        {
            if (shop.shopItems[characterNumber - 1].owned)
            {
                currentCharacterDisplayObjects[characterNumber].SetActive(true);
            }
            else
            {
                lockedCharacterDisplay.SetActive(true);
            }
        }
        else
        {
            currentCharacterDisplayObjects[characterNumber].SetActive(true);
        }

        
        
    }

    void DeactivateCharacterDisplay ()
    {
        if (currentCharacterDisplayObjects == null)
        {
            return;
        }
        for (int i = 0; i < currentCharacterDisplayObjects.Length; i++)
        {
            currentCharacterDisplayObjects[i].SetActive(false);
        }
        lockedCharacterDisplay.SetActive(false);
    }

    public void OnRoomCodeChange ()
    {
        PhotonRoom.instance.roomNumberString = roomNumberInputField.text;
        PhotonLobby.photonLobby.roomNumberString = roomNumberInputField.text;
    }

    public void SetNickname()
    {
        string str = nicknameInputField.text;
        if (!string.IsNullOrEmpty(str))
        {
            PlayerPrefs.SetString("PlayerNickname", str);
            PhotonRoom.instance.nickName = str;
        }
        else
        {
            PlayerPrefs.SetString("PlayerNickname", "");
            PhotonRoom.instance.nickName = "???";
        }
    }


    public void LoadTimeTrial ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TimeTrial");
    }
}
