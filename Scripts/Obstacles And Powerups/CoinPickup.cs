using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinPickup : MonoBehaviour
{
    public int coinValue;

    private ScoreManager theScoreManager;
    private AudioSource sfx;

    [SerializeField] GameObject vfx;
    
    

    // Start is called before the first frame update
    void Start()
    {
        theScoreManager = FindObjectOfType<ScoreManager>();
        sfx = theScoreManager.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            theScoreManager.AddCoinPoints(coinValue);
            Instantiate(vfx, transform.position, Quaternion.identity);
            sfx.Play();
            gameObject.SetActive(false);
        }
    }

}
