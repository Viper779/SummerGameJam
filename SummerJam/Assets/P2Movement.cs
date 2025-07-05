using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class P2Movement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float health = 100f;
    public float stun = 1.0f;
    public float servePwr = 1.0f;
    public float jump = 1.0f;
    public float maxJmp = 2.5f;
    public float leftBound = -4f;
    public float rightBound = 0f;

    public bool isStunned = false;
    public bool isDown = false;
    public bool isJumping = false;
    public bool isShooting = false;
    public bool isControlled = false;
    public bool hasHit = false;

    public float jumpChrg = 0f;
    public float shotCharge = 0f;

    Rigidbody2D rb;
    public Transform marker;
    public BallCode ball;
    public P1Movement p1;

    void Start()
    {
        isControlled = false;
        rb = GetComponent<Rigidbody2D>();
        //Change vars if different character
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isControlled = !isControlled;

            //Debug.Log("isActive is now: " + isActive);
        }

        if (isControlled)
        {
            if (Input.GetKey(KeyCode.A)) rb.AddForce(Vector3.left * moveSpeed * 2);

            if (Input.GetKey(KeyCode.D)) rb.AddForce(Vector3.right * moveSpeed * 2);

            if (Input.GetKey(KeyCode.W))
            {
                if(!isJumping)
                {
                    rb.AddForce(Vector2.up * jump * 7, ForceMode2D.Impulse);
                }
                isJumping = true;
                //jumpChrg = Mathf.Clamp((jumpChrg + (2 * jump * Time.deltaTime)), 0f, 2.5f);  // Smooth charging
                                                                                          //Debug.Log($"Charging: {jumpChrg}");
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                
                //Debug.Log($"Jump! Power: {jumpChrg}");
                jumpChrg = 0f;
                StartCoroutine(Delay(1.5f));
                //isJumping = false;
            }



            if (Input.GetMouseButton(0))
            {
                shotCharge = Mathf.Clamp((shotCharge + (6 * Time.deltaTime)), 0f, 4f);
                //Debug.Log($"Charging: {shotCharge}");
            }

            if (Input.GetMouseButtonUp(0))
            {
                isShooting = true;
                //Debug.Log("Shot Attempted");
                if (!isJumping)
                {
                    StartCoroutine(Delay(0.3f));
                }
                else
                {
                    StartCoroutine(Delay(1f));
                }

            }

        }

        if (!isControlled)
        {
            if (marker.position.x < 0 && !hasHit && !ball.canReset)
            {
                if (p1.hasHit)
                {
                    Debug.Log("En2 Hit");
                    leftBound = -8;
                    rightBound = 0;
                }
                else
                {
                    leftBound = -4;
                    rightBound = 0;
                }

                if (marker.position.x > leftBound && marker.position.x < rightBound)
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
                if (transform.position.x > -2)
                {
                    rb.AddForce(Vector3.left * moveSpeed * 2);
                }
                else
                {
                    rb.AddForce(Vector3.right * moveSpeed * 2);
                }
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
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        isJumping = false;
        isShooting = false;
        shotCharge = 0f;
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null)
        {
            Debug.LogWarning("Collision object does not have a collider.");
            yield return new WaitForSeconds(0.1f); ; // Exit if there's no collider
        }

        if (collision.gameObject.CompareTag("Ball") && !isShooting)
        {
            if (!isControlled)
            {
                yield return new WaitForSeconds(0.3f);
                isShooting = false;
                shotCharge = 0f;
            }
            isStunned = true;

            isStunned = false;
        }
    }
}
