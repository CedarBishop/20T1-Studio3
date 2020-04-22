using EasyProfile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    Currency currency;

    public ShopItem[] shopItems;



    void Start()
    {
        currency = GetComponent<Currency>();
        InitShop();
    }


    public void InitShop ()
    {
#if UNITY_IPHONE || UNITY_ANDROID
        InitShopLocal();
#else
        InitShopOnline();
#endif
    }

    public void BuyItem(int itemNumber)
    {
#if UNITY_IPHONE || UNITY_ANDROID
        if (shopItems[itemNumber].requiresPassion)
        {
            //price is in passion
            if (currency.SpendPassion(shopItems[itemNumber].itemPrice))
            {
                PlayerPrefs.SetInt(shopItems[itemNumber].itemKey, 1);
                shopItems[itemNumber].itemButton.interactable = false;
                SoundManager.instance.PlaySFX("Shop");
                shopItems[itemNumber].owned = true;
            }
        }
        else
        {
            // needs gold
            if (currency.SpendGold(shopItems[itemNumber].itemPrice))
            {
                PlayerPrefs.SetInt(shopItems[itemNumber].itemKey, 1);
                shopItems[itemNumber].itemButton.interactable = false;
                SoundManager.instance.PlaySFX("Shop");
                shopItems[itemNumber].owned = true;
            }
        }

#else

        if (shopItems[itemNumber].requiresPassion)
        {
            //price is in passion
            if (currency.SpendPassion(shopItems[itemNumber].itemPrice))
            {
                EasyProfileManager.Instance.SetCustomValue(shopItems[itemNumber].itemKey,1,OnSetShopItemComplete);
                shopItems[itemNumber].itemButton.interactable = false;
                SoundManager.instance.PlaySFX("Shop");
                shopItems[itemNumber].owned = true;
            }
        }
        else
        {
            // needs gold
            if (currency.SpendGold(shopItems[itemNumber].itemPrice))
            {
                EasyProfileManager.Instance.SetCustomValue(shopItems[itemNumber].itemKey, 1, OnSetShopItemComplete);
                shopItems[itemNumber].itemButton.interactable = false;
                SoundManager.instance.PlaySFX("Shop");
                shopItems[itemNumber].owned = true;
            }
        }







#endif
    }

    public void InitShopLocal ()
    {

        for (int i = 0; i < shopItems.Length; i++)
        {
            if (PlayerPrefs.HasKey(shopItems[i].itemKey))
            {
                shopItems[i].owned = (PlayerPrefs.GetInt(shopItems[i].itemKey) == 1) ? true : false;
                shopItems[i].itemButton.interactable = (PlayerPrefs.GetInt(shopItems[i].itemKey) == 1) ? false : true;
            }
            string str = (shopItems[i].requiresPassion) ? " Passion" : " Gold";
            shopItems[i].itemText.text = shopItems[i].itemKey + "\n" + shopItems[i].itemPrice.ToString() + str;
        }
    }

    public void InitShopOnline ()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            EasyProfileManager.Instance.GetCustomValue(shopItems[i].itemKey, OnGetShopItemComplete);
            string str = (shopItems[i].requiresPassion) ? " Passion" : " Gold";
            shopItems[i].itemText.text = shopItems[i].itemKey + "\n" + shopItems[i].itemPrice.ToString() + str;
        }
    }

    private void OnGetShopItemComplete(CallbackGetUserCustomValue msg)
    {
        if (msg.IsSuccess)
        {
            for (int i = 0 ; i < shopItems.Length; i++)
            {
                if (msg.ValueID == shopItems[i].itemKey)
                {
                    shopItems[i].owned = (msg.CustomValue.IntValue == 1) ? true : false ;
                    shopItems[i].itemButton.interactable = (msg.CustomValue.IntValue == 1) ? false : true;
                }
            }
        }
        else
        {
            Debug.Log("Failed to get user custom value");
            foreach (var item in shopItems)
            {
                if (msg.ValueID == item.itemKey)
                {
                    EasyProfileManager.Instance.SetCustomValue(item.itemKey, 0, OnSetShopItemComplete);
                }
            }

        }
    }

    private void OnSetShopItemComplete(CallbackSetUserCustomValue msg)
    {
        if (msg.IsSuccess)
        {
            foreach (var item in shopItems)
            {
                if (msg.ValueID == item.itemKey)
                {
                    
                    EasyProfileManager.Instance.GetCustomValue(item.itemKey, OnGetShopItemComplete);
                }
            }
        }
        else
        {
            Debug.Log("Failed to Set Shop Item");
        }
    }

}

[System.Serializable]
public struct ShopItem
{
    public Button itemButton;
    public string itemKey;
    public int itemPrice;
    public bool requiresPassion;
    public Text itemText;
    public bool owned;
}

