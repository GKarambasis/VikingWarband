using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Warband Manager")]
    public List<PlayerController> warbandMembers;
    [SerializeField] Vector3 playerStartPoint;
    private bool wasInvoked = false;
    public Vector3 spawnPosition { get; private set; }
    public List<GameObject> warbandFollowers;
    [SerializeField] List<GameObject> warbandStoreFollowers;
    [SerializeField] List<GameObject> warbandGodFollowers;
    [SerializeField] List<GameObject> StartingPlayer;

    [Header("Platform Generation")]
    public Transform platformGenerator;
    private Vector3 platformStartPoint;

    private PlatformDestroyer[] platformList;
    private ObstacleDestroyer[] obstacleList;

    //Score
    private ScoreManager theScoreManager;
    [Header("Game Manager")]
    [SerializeField] GameObject theDeathScreen;
    [SerializeField] GameObject thePauseScreen;
    [SerializeField] GameObject Countdown3, Countdown2, Countdown1;
    public float moveSpeed;
    public float speedMultiplier;
    public float speedIncreaseMilestone;
    [SerializeField] float speedIncreaseMilestoneStore;
    [SerializeField] float speedMilestoneCount;
    [SerializeField] float speedMilestoneCountStore;
    [SerializeField] float moveSpeedStore;

    //playerprefssaves
    [Header("Bank")]
    [SerializeField] int coinbank;
    [SerializeField] int skullbank;

    //Audio
    [Header("Audio")]
    [SerializeField] AudioClip gameBGM;
    [SerializeField] AudioClip deathBGM;
    private AudioSource audioSource;

    //Add Store Vikings
    private void Awake()
    {
        for (int i = 0; i <= warbandStoreFollowers.Count; i++)
        {
            if (PlayerPrefs.GetInt("isBought" + i.ToString()) == 1)
            {
                warbandFollowers.Add(warbandStoreFollowers[i]);
            }
        }

        //Player Instantiator
        if (PlayerPrefs.GetInt("isBought" + "FireGod") == 1)
        {
            Instantiate(warbandGodFollowers[0], playerStartPoint, Quaternion.identity);
            StartingPlayer.Add(warbandGodFollowers[0]);
        }

        if (PlayerPrefs.GetInt("isBought" + "Thor") == 1)
        {
            Instantiate(warbandGodFollowers[1], playerStartPoint, Quaternion.identity);
            StartingPlayer.Add(warbandGodFollowers[1]);
        }

        if (PlayerPrefs.GetInt("isBought" + "Odin") == 1)
        {
            Instantiate(warbandGodFollowers[2], playerStartPoint, Quaternion.identity);
            StartingPlayer.Add(warbandGodFollowers[2]);
        }

        if (PlayerPrefs.GetInt("isBought" + "FireGod") != 1 && PlayerPrefs.GetInt("isBought" + "Thor") != 1 && PlayerPrefs.GetInt("isBought" + "Odin") != 1)
        {
            Instantiate(warbandFollowers[0], playerStartPoint, Quaternion.identity);
            StartingPlayer.Add(warbandFollowers[0]);
        }
    }

    void Start()
    {
        platformStartPoint = platformGenerator.position;

        audioSource = gameObject.GetComponent<AudioSource>();

        coinbank = PlayerPrefs.GetInt("CoinBank");
        skullbank = PlayerPrefs.GetInt("SkullBank");

        theScoreManager = FindObjectOfType<ScoreManager>();

        speedMilestoneCount = speedIncreaseMilestone;

        moveSpeedStore = moveSpeed;
        speedMilestoneCountStore = speedMilestoneCount;
        speedIncreaseMilestoneStore = speedIncreaseMilestone;


    }

    void Update()
    {
        WarbandFollow();

        WarbandMoveSpeed();

        if (warbandMembers.Count == 0 && !wasInvoked)
        {
            wasInvoked = true;
            theScoreManager.scoreIncreasing = false;
            Invoke("GameOver", 1);
        }

        SpawnPosition();

        WarbandRemoveNull();

    }

    private void WarbandMoveSpeed()
    {
        if (warbandMembers.Count > 0)
        {

            if (warbandMembers[0].transform.position.x > speedMilestoneCount)
            {
                speedMilestoneCount += speedIncreaseMilestone;

                speedIncreaseMilestone = speedIncreaseMilestone * speedMultiplier;

                moveSpeed = moveSpeed * speedMultiplier;

            }

        }
    }

    public void WarbandFollow()
    {
        for (int i = 0; i < warbandMembers.Count; i++)
        {
            warbandMembers[i].warbandPosition = i;
        }
    }

    private void WarbandRemoveNull()
    {
        warbandMembers.RemoveAll(PlayerController => PlayerController == null);
    }

    // a method that can be called by other scripts
    public void GameOver()
    {
        //Save
        PlayerPrefs.SetInt("CoinBank", coinbank);
        PlayerPrefs.SetInt("SkullBank", skullbank);
        PlayerPrefs.SetFloat ("HighScore", theScoreManager.hiScoreCount);

        //Menu
        theDeathScreen.SetActive(true);

        //Audio
        if(deathBGM != null)
        {
            audioSource.clip = deathBGM;
            audioSource.Play();
        }
    }

    public void ScoreSave(int coinsave, int skullsave)
    {
        coinbank += coinsave;
        skullbank += skullsave;
    }

    public void Reset()
    {
        //Remove Menu
        theDeathScreen.SetActive(false);

        //Remove Platforms
        platformList = FindObjectsOfType<PlatformDestroyer>();
        for (int i = 0; i < platformList.Length; i++)
        {
            platformList[i].gameObject.SetActive(false);
        }

        //Remove Obstacles
        obstacleList = FindObjectsOfType<ObstacleDestroyer>();
        for (int i = 0; i < obstacleList.Length; i++)
        {
            Destroy(obstacleList[i].gameObject);
        }

        //Reset Player Position
        for(int i = 0; i < StartingPlayer.Count; i++)
        {
            Instantiate(StartingPlayer[i], playerStartPoint, Quaternion.identity);
        }
        

        //Reset Platform Generator Position
        platformGenerator.position = platformStartPoint;


        //Score
        theScoreManager.ScoreReset();

        //Speed Reset
        moveSpeed = moveSpeedStore;
        speedMilestoneCount = speedMilestoneCountStore;
        speedIncreaseMilestone = speedIncreaseMilestoneStore;

        //Audio
        if(gameBGM != null)
        {
            audioSource.clip = gameBGM;
            audioSource.Play();
        }

        Invoke("WasInvoked", 1f);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
        thePauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        thePauseScreen.SetActive(false);
        StartCoroutine(CountDown());
    }


    public IEnumerator CountDown()
    {
        Countdown3.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        Countdown3.SetActive(false);
        Countdown2.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        Countdown2.SetActive(false);
        Countdown1.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        Countdown1.SetActive(false);
        Time.timeScale = 1;
    }

    private void WasInvoked()
    {
        wasInvoked = false;

    }

    public void SpawnPosition()
    {
        if (warbandMembers.Count > 0)
        {

            spawnPosition = warbandMembers[0].transform.GetChild(2).transform.position;

        }
    }


}
