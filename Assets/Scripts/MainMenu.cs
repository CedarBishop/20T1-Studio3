using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainParent;
    public GameObject settingsParent;
    public GameObject shopParent;

    void Start()
    {
        SetMenuType(1);
    }

    public void Quit  ()
    {
        Application.Quit();
    }

    public void SetMenuType(int menuType)
    {
        switch (menuType)
        {
            case 1:
                mainParent.SetActive(true);
                settingsParent.SetActive(false);
                shopParent.SetActive(false);
                break;
            case 2:
                mainParent.SetActive(false);
                settingsParent.SetActive(true);
                shopParent.SetActive(false);
                break;
            case 3:
                mainParent.SetActive(false);
                settingsParent.SetActive(false);
                shopParent.SetActive(true);
                break;
            default:
                break;
        }
    }
}
