using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class scoreText : MonoBehaviour
{
    public BallCode ball;
    public TMP_Text scoreTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreTxt.text = $"{ball.score}";
    }
}
