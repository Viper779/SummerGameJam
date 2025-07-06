using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BallCode : MonoBehaviour
{
    public int lives = 3;
    public int score = 0;

    public float hits = 0;
    public float bounceForce = 10;

    private float charge1 = 0;
    private float return1 = 0;

    //private float charge2 = 0;
    private float return2 = 0;

    private bool isShooting = false;
    private bool isReturning1 = false;
    private bool isReturning2 = false;
    public bool canReset = false;

    public P1Movement player1;
    public P2Movement player2;

    public En1Movement en1;

    public En2Movement en2;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {

        if (player1 == null)
        {
            Debug.LogWarning("Player1 reference is missing!");
            return;
        }

        charge1 = player1.shotCharge;
        return1 = en1.shotCharge;
        return2 = en2.shotCharge;

        isShooting = player1.isShooting;
        isReturning1 = en1.isShooting;
        isReturning2 = en2.isShooting;

        if (Input.GetMouseButton(0) && canReset)
        {
            transform.position = new Vector3(-6, 4, 0);
            rb.velocity = Vector2.zero;
            canReset = false;
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canReset)
        {


            if (collision.collider == null)
            {
                Debug.LogWarning("Collision object does not have a collider.");
                return; // Exit if there's no collider
            }

            if (collision.gameObject.CompareTag("Player1"))
            {
                hits++;
                StartCoroutine(BounceRight1());
            }

            if (collision.gameObject.CompareTag("Player2"))
            {
                hits++;
                StartCoroutine(BounceRight2());
            }

            if (collision.gameObject.CompareTag("Enemy"))
            {
                hits++;
                StartCoroutine(BounceLeft());


            }

            if (collision.gameObject.CompareTag("Enemy2"))
            {
                hits++;
                StartCoroutine(BounceLeft());


            }

            if (collision.gameObject.CompareTag("Ground"))
            {
                rb.velocity = Vector2.zero;

                PointScored();
            }
        }
    }
    IEnumerator BounceRight1()
    {
        yield return new WaitForSeconds(0.03f);

        if (player1.isControlled)
        {
            if (player1.isShooting)
            {
                rb.AddForce(Vector2.right * charge1, ForceMode2D.Impulse);
                //Debug.Log("Shot Made");
                score = score + 1;
            }
        }
        else
        {
            rb.AddForce(Vector2.right * player1.shotCharge * (0.8f + Random.value / 4) * (player1.transform.position.x / -6), ForceMode2D.Impulse);
        }

        if (player1.isJumping)
        {
            rb.AddForce(Vector2.up * bounceForce * 0.5f, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * 3f, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
        //rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);


        //Debug.Log(hits);
    }

    IEnumerator BounceRight2()
    {
        yield return new WaitForSeconds(0.03f);

        if (player2.isControlled)
        {
            if (player2.isShooting)
            {
                rb.AddForce(Vector2.right * charge1, ForceMode2D.Impulse);
                //Debug.Log("Shot Made");
                score = score + 1;
            }
        }
        else
        {
            rb.AddForce(Vector2.right * player2.shotCharge * (1f + Random.value / 4) * (player2.transform.position.x / -6), ForceMode2D.Impulse);
        }

        if (player2.isJumping)
        {
            rb.AddForce(Vector2.up * bounceForce * 0.3f, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * 1f, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
            //rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);


        Debug.Log(hits);
    }

    IEnumerator BounceLeft()
    {
        yield return new WaitForSeconds(0.03f);
        if (isReturning1)
        {
            rb.AddForce(Vector2.left * (return1+2) * (1.5f + Random.value/2) * (en1.transform.position.x/6), ForceMode2D.Impulse);
            //Debug.Log("Shot Returned");
        }
        //rb.AddForce(Vector2.left * return1, ForceMode2D.Impulse);
        if (isReturning2)
        {
            rb.AddForce(Vector2.left * (return2+2) * (1.5f + Random.value/2) * (en2.transform.position.x/6), ForceMode2D.Impulse);
            //Debug.Log("Shot Returned");
        }


        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        //Debug.Log(hits);
    }

    void PointScored()
    {
        if (transform.position.x < 0)
        {
            if (transform.position.x < -8 && !player1.hasHit && !player2.hasHit)
            {
                score = score + 20;
            }
            else
            {
                lives = lives - 1;
            }
            
        }
        else
        {
            if (transform.position.x > 8 && !en1.hasHit && !en2.hasHit)
            {
                lives = lives - 1;
            }
            else
            {
                score = score + 20;
            }

        }

        canReset = true;
        en2.hasHit = false;
        en1.hasHit = false;
        player1.hasHit = false;
        player2.hasHit = false;
        hits = 0;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }


}
