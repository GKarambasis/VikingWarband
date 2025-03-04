using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopScript : MonoBehaviour
{
    [SerializeField] string vikingID;
    [SerializeField] int cost;
    [SerializeField] string currency;
    [SerializeField] int isbought;
    [SerializeField] int istoggled;

    private Button button;

    private TextMeshProUGUI text;

    private MainMenu menuScript;
    
    void Start()
    {
        menuScript = FindObjectOfType<MainMenu>();
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (PlayerPrefs.HasKey("isBought" + vikingID))
        {
            isbought = PlayerPrefs.GetInt("isBought" + vikingID);
        }


        else
        {
            isbought = 0;
        }

        if (gameObject.GetComponent<Button>() != null)
        {
            button = gameObject.GetComponent<Button>();
            DisableButton();
        }

        text.text = cost.ToString();
    }

    private void DisableButton()
    {
        if (isbought == 0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void Buy()
    {
        int bank = menuScript.ReturnCurrency(currency);

        if (bank >= cost)
        {
            UnlockCharacter();
            menuScript.Transaction(currency, cost);
        }
        else
        {
            ErrorMessage();
        }
    }

    private void UnlockCharacter()
    {
        isbought = 1;
        PlayerPrefs.SetInt("isBought" + vikingID, isbought);
        DisableButton();
    }

    private void ErrorMessage()
    {
        Debug.Log("not enough skrilla");
        menuScript.NoticePanel();
    }
}
