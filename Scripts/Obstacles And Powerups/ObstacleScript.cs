using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObstacleScript : MonoBehaviour
{
    [SerializeField] GameObject obstacleVFX;
    [SerializeField] AudioClip obstacleSFX;

    private AudioSource audioSource;
    private Character Character;

    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = obstacleSFX;        
    }

    void Start()
    {
        if (gameObject.GetComponent<Character>() != null)
        {
            Character = gameObject.GetComponent<Character>();
            Character.Animator.SetBool("Ready", true);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponent<Collider2D>().enabled = false;

            if (Character != null)
            {
                audioSource.Play();
                Character.SetState(CharacterState.DeathB);
                Destroy(gameObject, 3f);
            }
            else
            {
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 100, transform.position.z);
                audioSource.Play();
                
                Destroy(gameObject, 3f);  
            }

            collision.GetComponent<PlayerController>().Death();

            if (obstacleVFX != null)
            {
                Instantiate(obstacleVFX, transform.position, Quaternion.identity);
            }
        }
    }
}
