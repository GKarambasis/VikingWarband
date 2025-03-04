using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MainMenu : MonoBehaviour
{

    [Header("Currency Texts")]
    [SerializeField] TextMeshProUGUI coinBankText;
    [SerializeField] TextMeshProUGUI skullBankText;
    [SerializeField] TextMeshProUGUI highScoreText;

    private int coinBank;
    private int skullBank;
    private float highScore;

    //Settings
    [Header("Settings Menu")]
    bool isSettingsOpen;
    bool isTutorialOpen;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject tutorialMenu;
    bool isNoticeOpen;
    [SerializeField] GameObject noticePanel;
    
    [Header("Audio Mixers & Sliders")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Menu Vikings")]
    [SerializeField] GameObject Odin;
    [SerializeField] GameObject Thor;
    [SerializeField] GameObject FireGod;

    void Start()
    {
        coinBank = PlayerPrefs.GetInt("CoinBank");
        skullBank = PlayerPrefs.GetInt("SkullBank");
        highScore = PlayerPrefs.GetFloat("HighScore");

        coinBankText.text = "" + coinBank;
        skullBankText.text = "" + skullBank;
        highScoreText.text = "" + Mathf.Round(highScore);

        GodUpdate();
    }


    //Shop Functions
    public int ReturnCurrency(string currency)
    {
        if (currency == "CoinBank")
        {
            return coinBank;
        }
        else if (currency == "SkullBank")
        {
            return skullBank;
        }
        else if (currency == "HighScore")
        {
            return (int)Mathf.Round(highScore);
        }
        else
        {
            return 0;
        }
    }
    public void Transaction(string currency, int cost)
    {
        if (currency == "CoinBank")
        {
            coinBank -= cost;
            PlayerPrefs.SetInt("CoinBank", coinBank);
            coinBankText.text = "" + coinBank;
        }
        else if (currency == "SkullBank")
        {
            skullBank -= cost;
            PlayerPrefs.SetInt("SkullBank", skullBank);
            skullBankText.text = "" + skullBank;
        }
        else if (currency == "HighScore")
        {
            GodUpdate();
        }
    }


    //Play And Quit Buttons
    public void PlayGame()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            TutorialMenu();
            PlayerPrefs.SetInt("Tutorial", 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
    public void QuitGame()
    {

        Application.Quit();
        
    }


    //Settings Menu
    public void SettingsMenu()
    {
        if (!isSettingsOpen)
        {
            settingsMenu.SetActive(true);
            isSettingsOpen = true;
            LoadSoundSettings();
        }
        else
        {
            settingsMenu.SetActive(false);
            isSettingsOpen = false;
        }
    }
    public void TutorialMenu()
    {
        if (!isTutorialOpen)
        {
            tutorialMenu.SetActive(true);
            isTutorialOpen = true;
        }
        else
        {
            tutorialMenu.SetActive(false);
            isTutorialOpen = false;
        }
    }
    public void NoticePanel()
    {
        if (isNoticeOpen)
        {
            noticePanel.SetActive(false);
            isNoticeOpen = false;
        }
        else if (!isNoticeOpen)
        {
            noticePanel.SetActive(true);
            isNoticeOpen = true;
        }
    }

    //Shop Settings
    public void ResetShop()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt("isBought" + i.ToString(), 0);
        }

        PlayerPrefs.SetInt("isBought" + "FireGod", 0);
        PlayerPrefs.SetInt("isBought" + "Thor", 0);
        PlayerPrefs.SetInt("isBought" + "Odin", 0);
    }
    public void AddMoney()
    {
        Transaction("CoinBank", -1000);
    }
    
    //Audio Settings
    public void BGMVolumeChange()
    {
        float volume = bgmSlider.value;
        audioMixer.SetFloat("bgm", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("bgm", volume);

    }
    public void SFXVolumeChange()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfx", volume);
    }
    private void LoadSoundSettings()
    {
        if (PlayerPrefs.HasKey("bgm"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("bgm");
            audioMixer.SetFloat("bgm", Mathf.Log10(bgmSlider.value) * 20);
        }
        else
        {
            bgmSlider.value = 0.500f;
            audioMixer.SetFloat("bgm", Mathf.Log10(bgmSlider.value) * 20);
        }

        if (PlayerPrefs.HasKey("sfx"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfx");
            audioMixer.SetFloat("sfx", Mathf.Log10(sfxSlider.value) * 20);
        }
        else
        {
            sfxSlider.value = 0.500f;
            audioMixer.SetFloat("sfx", Mathf.Log10(sfxSlider.value) * 20);
        }
    }

    private void GodUpdate()
    {
        if (PlayerPrefs.GetInt("isBought" + "Odin") == 1)
        {
            Odin.SetActive(true);
        }

        if (PlayerPrefs.GetInt("isBought" + "Thor") == 1)
        {
            Thor.SetActive(true);
        }

        if (PlayerPrefs.GetInt("isBought" + "FireGod") == 1)
        {
            FireGod.SetActive(true);
        }
    }

}
