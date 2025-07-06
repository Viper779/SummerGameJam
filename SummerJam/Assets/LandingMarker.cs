using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LandingMarker : MonoBehaviour
{
    public Transform ball;
    public Rigidbody2D ballrb;
    public GameObject ballObject;
    public BallCode ballCode;

    public bool isCalculating = false;

    public float currentCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (ballObject != null)
        {
            ballrb = ballObject.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ball == null || ballrb == null || ballCode == null)
            return;

        if (ballCode.hits == 0)
        {
            currentCount = ballCode.hits;
        }

        if (currentCount < ballCode.hits)
        {
            StartCoroutine(CalculatePath(1f));
        }

        if (ballCode.canReset)
        {
            transform.position = new Vector3(-6, 0.5f, 0);
        }


    }

    IEnumerator CalculatePath(float time)
    {
        if (isCalculating) yield break;

        isCalculating = true;

        yield return new WaitForSeconds(time);

        try
        {
            Vector2 velocity = ballrb.velocity;
            float gravity = Mathf.Abs(Physics2D.gravity.y);
            float yPos = ball.position.y;

            float sqrtArg = velocity.y * velocity.y + 2 * gravity * yPos;
            if (sqrtArg < 0f || gravity == 0f || velocity.magnitude < 0.1f)
                yield break;

            float timeToLand = (-velocity.y + Mathf.Sqrt(sqrtArg)) / gravity;
            float predictedX = ball.position.x + velocity.x * timeToLand;

            transform.position = new Vector3(predictedX, 1f, 0f);
            currentCount = ballCode.hits;

            //Debug.Log(predictedX);
        }
        finally
        {
            isCalculating = false;
        }
    }


}
