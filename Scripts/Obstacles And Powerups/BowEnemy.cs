using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class BowEnemy : MonoBehaviour
{
    //Object is instantiated through the obstacle generator

    //When the Object reaches a certain point in the camera, execute the Shoot Function
    //The Shoot function should trigger the shoot animation and fire an arrow. 
    [SerializeField] GameObject ArrowPrefab;
    [SerializeField] Transform ShootPoint;
    [SerializeField] float timeBeforeShootMin, timeBeforeShootMax;

    private Character Character;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Character = gameObject.GetComponent<Character>();
        Character.Animator.SetInteger("Charge", 1);
        Character.Animator.SetBool("Ready", true);
        Invoke("ShootArrow", Random.Range(timeBeforeShootMin, timeBeforeShootMax));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShootArrow()
    {
        Character.Animator.SetInteger("Charge", 2);
        Instantiate(ArrowPrefab, ShootPoint.position, Quaternion.identity);
    }
}
