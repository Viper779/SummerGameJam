using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class En1Movement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float health = 100f;
    public float stun = 1.0f;
    public float servePwr = 1.0f;
    public float jump = 1.0f;
    public float maxJmp = 2.5f;

    public bool isStunned = false;
    public bool isDown = false;
    public bool isJumping = false;
    public bool isShooting = false;
    public bool hasHit = false;
    public float jumpChrg = 0f;
    public float shotCharge = 0f;
    public float lastHit = 0;

    public float rightBound = 0.5f;
    public float leftBound = 0.5f;

    public Transform marker;

    Rigidbody2D rb;

    public BallCode ball;
    public En2Movement en2;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(lastHit);

        if (marker.position.x > 0 && !hasHit && !ball.canReset)
        {
            if (en2.hasHit)
            {
                StartCoroutine(wideAOR());
            }
            else
            {
                leftBound = 0;
                rightBound = 4;
            }

            if (marker.position.x > leftBound && marker.position.x < rightBound )
            {
            if (transform.position.x - marker.position.x > 0)
            {
                rb.AddForce(Vector3.left * moveSpeed * 2);
            }
            else
            {
                rb.AddForce(Vector3.right * moveSpeed * 2);
            }

            shotCharge = Mathf.Clamp((shotCharge + (4 * Time.deltaTime)), 0f, 4f);

            //Debug.Log(shotCharge);
            }
        }
        else
        {
            if (transform.position.x > 2)
            {
                rb.AddForce(Vector3.left * moveSpeed * 2);
            }
            else
            {
                rb.AddForce(Vector3.right * moveSpeed * 2);
            }
        }

        if (!isJumping)
        {
            if (rb.rotation > 0f && rb.angularVelocity > -20)
            {
                rb.AddTorque(-0.03f * rb.rotation);
                //Debug.Log(rb.angularVelocity);
            }

            if (rb.rotation < 0f && rb.angularVelocity < 20)
            {
                rb.AddTorque(-0.03f * rb.rotation);
                //Debug.Log(rb.angularVelocity);
            }
        }

        if (hasHit)
        {
            StartCoroutine(ResetHit());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null)
        {
            Debug.LogWarning("Collision object does not have a collider.");
            return; // Exit if there's no collider
        }

        if (collision.gameObject.CompareTag("Ball"))
        {
            if (Random.value < 0.8f)
            {
                StartCoroutine(MakeShot(0.2f));
            }
            else
            {
                isStunned = true;

                isStunned = false;
            }

            hasHit = true;
            lastHit = ball.hits;

        }
    }

    IEnumerator MakeShot(float time)
    {
        //Debug.Log("Return");
        isShooting = true;
        yield return new WaitForSeconds(time);
        isShooting = false;
        shotCharge = 0f;
    }

    IEnumerator ResetHit()
    {
        if (ball.hits > lastHit + 1)
        {
            //Debug.Log("  " + ball.hits + "  " + lastHit);

            yield return new WaitForSeconds(0.2f);
            //Debug.Log("Reset");
            hasHit = false;
            lastHit = 0;
        }
    }

    IEnumerator wideAOR()
    {
        yield return new WaitForSeconds(0.7f);
        leftBound = 0;
        rightBound = 8;
    }
}
