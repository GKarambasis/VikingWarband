using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class WarbandPickup : MonoBehaviour
{
    private Transform spawnPoint;
    private ScoreManager scoreManager;
    private GameManager theManager;
    private Character Character;
    [SerializeField] int warbandAmount;
    [SerializeField] GameObject DeathVFX;
    private Vector3 Body;
    private int followerSelector;

    // Start is called before the first frame update
    void Start()
    {
        Character = gameObject.GetComponent<Character>();
        spawnPoint = GameObject.Find("WarbandSpawner").transform;
        theManager = FindObjectOfType<GameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        Body = gameObject.transform.GetChild(0).transform.position;
        Character.Animator.SetBool("Ready", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 pos = spawnPoint.position;

        if (other.tag == "Player")
        {
            Character.SetState(CharacterState.DeathB);
            gameObject.GetComponent<Collider2D>().enabled = false;
            other.GetComponent<PlayerController>().isSlashing = true;

            if (DeathVFX != null)
            {
                Instantiate(DeathVFX, Body, Quaternion.identity);
            }

            if (warbandAmount > 0)
            {
                followerSelector = Random.Range(0, theManager.warbandFollowers.Count);
                Instantiate(theManager.warbandFollowers[followerSelector], theManager.spawnPosition, Quaternion.identity);

            }
            scoreManager.AddSkullPoints(warbandAmount);

            Destroy(gameObject, 3f);
        }
    }
}
