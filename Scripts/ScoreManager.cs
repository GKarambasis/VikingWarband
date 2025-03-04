using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Scores")]
    public float scoreCount;
    public float hiScoreCount { get; private set; }
    public float pointsPerSecond;
    public bool scoreIncreasing;
    [SerializeField] GameObject BootIcon;
    [SerializeField] GameObject crownIcon;

    [Header("Collectibles")]
    [SerializeField] int coinCount;
    [SerializeField] int coinBank;
    [SerializeField] int skullCount;
    private GameManager manager;
           
    [Header("HUD Text Screens")]
    [SerializeField] TextMeshProUGUI warbandText;
    //[SerializeField] TextMeshProUGUI hiScoreText;
    [SerializeField] TextMeshProUGUI[] scoreText;
    [SerializeField] TextMeshProUGUI[] coinText;
    [SerializeField] TextMeshProUGUI[] skullsText;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();

        if (PlayerPrefs.HasKey("HighScore"))
        {
            hiScoreCount = PlayerPrefs.GetFloat("HighScore");
        }

    }

    // Update is called once per frame
    void Update()
    {
        IncreaseScore();

        UpdateTextScreens();
        
    }

    public void AddCoinPoints (int pointsToAdd)
    {
        coinCount += pointsToAdd;
        manager.ScoreSave(pointsToAdd, 0);
    }

    public void AddSkullPoints(int pointsToAdd)
    {
        skullCount += pointsToAdd;
        manager.ScoreSave(0, pointsToAdd);
    }

    private void IncreaseScore()
    {
        if (scoreIncreasing)
        {
            scoreCount += pointsPerSecond * Time.deltaTime;
        }

        if (scoreCount > hiScoreCount)
        {
            hiScoreCount = scoreCount;
            crownIcon.SetActive(true);
            BootIcon.SetActive(false);
        }
    }

    private void UpdateTextScreens()
    {
        //hiScoreText.text = "" + Mathf.Round(hiScoreCount);

        for (var i = 0; i < coinText.Length; i++)
        {
            scoreText[i].text = "" + Mathf.Round(scoreCount);
            coinText[i].text = "" + coinCount;
            skullsText[i].text = "" + skullCount;
        }

        warbandText.text = "" + manager.warbandMembers.Count;


    }

    public void ScoreReset()
    {
        coinCount = 0;
        skullCount = 0;

        scoreCount = 0;
        scoreIncreasing = true;

        crownIcon.SetActive(false);
        BootIcon.SetActive(true);
    }

}
