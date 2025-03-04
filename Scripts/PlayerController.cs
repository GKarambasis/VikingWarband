using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask WhatIsGround;
    [Header("Required Script")]
    //Game manager script
    public GameManager theGameManager;
    private Character Character;

    
    //For Movement
    private float boostSpeed;
    //Declares that this private is using a rigidbody
    private Rigidbody2D MyRigidbody;

    //To detect if player is able to jump
    [Header("Jump Mechanic")]
    public float jumpForce;
    public bool Grounded;
    private Collider2D myCollider;
    public Transform feetPos;
    public float checkRadius;

    //for jumping while holding the key
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    //Warband Mechanic
    [Header("Warband Mechanic")]
    public int warbandPosition;
    [SerializeField] bool isAlive = true;
    [SerializeField] bool isStartingPlayer;

    [Header("VFX & SFX")]
    public bool isSlashing = false;
    private AudioSource audioSource;
    [SerializeField] AudioClip slashSFX, spawnSFX;
    [SerializeField] AudioClip[] deathSFX;
    [SerializeField] GameObject slashVFX;
    [SerializeField] GameObject spawnVFX, deathVFX;
    

    // Start is called before the first frame update
    void Start()
    {
        feetPos = gameObject.transform.GetChild(0);
        
        Character = gameObject.GetComponent<Character>();

        audioSource = gameObject.GetComponent<AudioSource>();

        MyRigidbody = GetComponent<Rigidbody2D>();

        theGameManager = FindObjectOfType < GameManager > ();

        theGameManager.warbandMembers.Add(gameObject.GetComponent<PlayerController>());

        myCollider = GetComponent<Collider2D>();

        Character.Animator.SetBool("Ready", true);

        if (!isStartingPlayer)
        {
            Instantiate(spawnVFX, transform.position, Quaternion.identity);
            if (spawnSFX != null)
            {
                audioSource.clip = spawnSFX;
                audioSource.Play();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            MovementSpeed();

            Jump();

            CatchUpCheck();


            Animation();

            Slash();
            
        }

        
    }

    private void MovementSpeed()
    {
        MyRigidbody.velocity = new Vector2(theGameManager.moveSpeed + boostSpeed, MyRigidbody.velocity.y);
    }

    private void CatchUpCheck()
    {
        if (warbandPosition > 0)
        {
            if (gameObject.transform.position.x < theGameManager.warbandMembers[warbandPosition - 1].transform.GetChild(2).transform.position.x - 0.5)
            {
                CatchUp(2);

            }
            else if (gameObject.transform.position.x > theGameManager.warbandMembers[warbandPosition - 1].transform.GetChild(2).transform.position.x + 0.5)
            {
                CatchUp(1);
            }
            else if (theGameManager.warbandMembers[warbandPosition - 1].transform.GetChild(2).transform.position.x - 0.5 <= gameObject.transform.position.x && gameObject.transform.position.x <= theGameManager.warbandMembers[warbandPosition - 1].transform.GetChild(2).transform.position.x + 0.5)
            {
                CatchUp(0);
            }
            else
            {

            }
        }
        else
        {
            CatchUp(0);
        }

    }

    public void CatchUp(int boost)
    {
        switch (boost)
        {
            case 2:
                boostSpeed = theGameManager.moveSpeed / 2;
                break;

            case 1:
                boostSpeed = -theGameManager.moveSpeed / 2;
                break;

            case 0:
                boostSpeed = 0;
                break;
        }
    }

    private void Jump()
    {
        float delay = (float)warbandPosition/10;
        //Grounded = Physics2D.IsTouchingLayers(myCollider, WhatIsGround);
        Grounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, WhatIsGround);

        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Invoke("DelayedJump", delay);

        }

        //
        //Hold = jump higher
        if ((Input.GetKey(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)) && isJumping == true)
        {

            if (jumpTimeCounter > 0)
            {
                MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, jumpForce);
                //starting to substract from the timer value until it hits 0 and stops triggering this if statement
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }

        }

        if (Input.GetKeyUp(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            Invoke("DelayedRelease", delay);
        }
    }

    private void DelayedJump()
    {
        if (Grounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, jumpForce);
        }
    }
    private void DelayedRelease()
    {
        isJumping = false;
    }

    private void Animation()
    {
        if (Grounded)
        {
            Character.SetState(CharacterState.Run);
        }
        else
        {
            Character.SetState(CharacterState.Jump);
        }
    }

    private void Slash()
    {
        if (isSlashing)
        {
            Character.Slash();
            if (slashVFX != null)
            {
                Instantiate(slashVFX, new Vector3(transform.GetChild(3).position.x + 3, transform.GetChild(3).position.y, transform.GetChild(3).position.z), Quaternion.identity);

            }
            if (slashSFX != null)
            {
                audioSource.clip = slashSFX;
                audioSource.Play();
            }
            isSlashing = false;
        }

    }

    //Collision with other players + Collision with killbox
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "killbox")
        {
            Invoke("Check", (float)warbandPosition / 10);
            Destroy(gameObject, 3f);
            Character.SetState(CharacterState.DeathF);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }

        if (other.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<CapsuleCollider2D>(), gameObject.GetComponent<CapsuleCollider2D>());
        }
    }

    private void Check()
    {
        if (theGameManager.warbandMembers[warbandPosition] != null)
        {
            if (isAlive)
            {
                theGameManager.warbandMembers.Remove(theGameManager.warbandMembers[warbandPosition]);
                isAlive = false;
            }
        }
        else
        {
            Invoke("LateCheck", 0.5f);
        }
    }
    private void LateCheck()
    {
        if (isAlive)
        {
            theGameManager.warbandMembers.Remove(theGameManager.warbandMembers[warbandPosition]);
            isAlive = false;
        }
    }

    //Death
    public void Death()
    {
        if (isAlive)
        {
            isAlive = false;
            theGameManager.warbandMembers.Remove(theGameManager.warbandMembers[warbandPosition]);

        }
        
        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.GetChild(3).GetChild(0).transform.position, Quaternion.identity);
        }
        if (deathSFX != null)
        {
            int randomSound = Random.Range(0, deathSFX.Length);
            audioSource.clip = deathSFX[randomSound];
            audioSource.Play();
        }
        Character.SetState(CharacterState.DeathF);

        Destroy(gameObject, 3f);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

}
