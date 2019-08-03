using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Score : MonoBehaviour {
    public int timeLeft;
    public Text timer;
    public Text Points;
    public GameObject gameOver;
    public int minutesLeft;
    public int secondsLeft;
    public int points = 0;
    public static Score instance;
    
    // Use this for initialization
    void Start ()
    {
        instance = this;
        secondsLeft = 10;
        minutesLeft = 0;
        Points.text = "" + points;
        StartCoroutine(countdownTimer());
    }

    void Update()
    {
        Points.text = "" + points;
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(AddTime());
        }
    }

    public IEnumerator AddPoints()
    {
        float t = 0;
        while (t < 5)
        {
            points += 2;
            Points.text = "" + points;
            t += 1;
            yield return 0;
        }

        yield return 0;
    }

    public IEnumerator AddTime()
    {
        float t = 0;
        while (t < 5f)
        {
            secondsLeft += 1;
            float minutes = Mathf.Floor(secondsLeft / 60);
            int seconds = Mathf.RoundToInt(secondsLeft%60);
            if (seconds > 10)
                timer.text = minutes + ":" + seconds;
            else
            {
                timer.text = minutes + ":0" + seconds;
            }

            points += 5;
            Points.text = "" + points;
            t += 1;
            yield return new WaitForSeconds(.1f);
            
        }
    }
    IEnumerator countdownTimer()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (secondsLeft > 0)
        {
            secondsLeft--;
            float minutes = Mathf.Floor(secondsLeft / 60);
            int seconds = Mathf.RoundToInt(secondsLeft%60);
           if (seconds > 10)
            timer.text = minutes + ":" + seconds;
           else
           {
               timer.text = minutes + ":0" + seconds;
           }
            yield return wait;
        }
        
        SceneManager.LoadScene("EndScene");
    }
}