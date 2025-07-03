using UnityEngine;
using System.Collections;
using TMPro;

public class P1Movement : MonoBehaviour
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
    public float jumpChrg = 0f;
    public float shotCharge = 0f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Change vars if different character
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.A)) rb.AddForce(Vector3.left * moveSpeed * 2);

        if (Input.GetKey(KeyCode.D)) rb.AddForce(Vector3.right * moveSpeed * 2);

        if (Input.GetKey(KeyCode.W))
        {
            isJumping = true;
            jumpChrg = Mathf.Clamp((jumpChrg + (2 * jump * Time.deltaTime)), 0f, 2.5f);  // Smooth charging
            //Debug.Log($"Charging: {jumpChrg}");
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpChrg * 5, ForceMode2D.Impulse);
            //Debug.Log($"Jump! Power: {jumpChrg}");
            jumpChrg = 0f;
            StartCoroutine(Delay(1f));
            isJumping = false;
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

        if (Input.GetMouseButton(0))
        {
            shotCharge = Mathf.Clamp((shotCharge + (6 * Time.deltaTime)), 0f, 4f);
            //Debug.Log($"Charging: {shotCharge}");
        }

        if (Input.GetMouseButtonUp(0))
        {
            isShooting = true;
            //Debug.Log("Shot Attempted");
            StartCoroutine(Delay(0.3f));
            //isShooting = false;
            //shotCharge = 0f;

        }

    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        isShooting = false;
        shotCharge = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null)
        {
            Debug.LogWarning("Collision object does not have a collider.");
            return; // Exit if there's no collider
        }

        if (collision.gameObject.CompareTag("Ball") && !isShooting)
        {
            isStunned = true;

            isStunned = false;
        }
    }
}
