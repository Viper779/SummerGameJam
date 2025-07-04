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

    private float charge2 = 0;
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
            rb.AddForce(Vector2.right * player1.shotCharge, ForceMode2D.Impulse);
        }

            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        
        Debug.Log(hits);
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
            rb.AddForce(Vector2.right * player2.shotCharge, ForceMode2D.Impulse);
        }

        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);


        Debug.Log(hits);
    }

    IEnumerator BounceLeft()
    {
        yield return new WaitForSeconds(0.03f);
        if (isReturning1)
        {
            rb.AddForce(Vector2.left * return1, ForceMode2D.Impulse);
            //Debug.Log("Shot Returned");
        }
        //rb.AddForce(Vector2.left * return1, ForceMode2D.Impulse);
        if (isReturning2)
        {
            rb.AddForce(Vector2.left * return2, ForceMode2D.Impulse);
            //Debug.Log("Shot Returned");
        }


        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        //Debug.Log(hits);
    }

    void PointScored()
    {
        if (transform.position.x < 0)
        {
            lives = lives - 1;
        }
        else
        {
            score = score + 20;
        }

        canReset = true;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }


}
