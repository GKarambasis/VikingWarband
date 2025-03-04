using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float arrowspeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(-arrowspeed, rb.velocity.y);
    }
}
