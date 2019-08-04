using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Score : MonoBehaviour {
    public int timeLeft;
    public Text timer;
    public Text Points;
    //public GameObject gameOver;
    public int minutesLeft;
    public int secondsLeft;
    public int points = 0;
    public int levelsCleared = 0;
    public int totalTime = 0;
    private int highScore = 0;
    public static Score instance;

    public Text TotalTime;
    public Text LevelsCleared;
    public Text FinalPoints;
    public Text NewBest;
    public Text PR;
    public GameObject ScorePanel;
    public GameObject Restart;
    public GameObject PlayUI;

    public AudioSource TickTock;
    public AudioSource Beeping;

    public static int HighScore;
    
    
    // Use this for initialization
    void Start ()
    {
        highScore = PlayerPrefs.GetInt("HighScore", highScore);
        instance = this;
        secondsLeft = 60;
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
        TickTock.Play();
        while (secondsLeft > 0)
        {
            secondsLeft--;
            totalTime++;
            float minutes = Mathf.Floor(secondsLeft / 60);
            int seconds = Mathf.RoundToInt(secondsLeft%60);
           if (seconds > 10)
            timer.text = minutes + ":" + seconds;
           else
           {
               timer.text = minutes + ":0" + seconds;
           }
            Beeping.Play();
            yield return wait;
        }
        TickTock.Stop();
        yield return new WaitForSeconds(.5f);
        StartCoroutine(ScoreScreen());
    }

    IEnumerator ScoreScreen()
    {
        PlayUI.SetActive(false);
        ScorePanel.SetActive(true);
        Restart.SetActive(true);
        int hiScoreCounter = highScore;
        PR.text = "Best: " + hiScoreCounter;
        
        yield return new WaitForSeconds(.2f);
        LevelsCleared.gameObject.SetActive(true);

        int levelCounter = 0;
        while (levelCounter < levelsCleared)
        {
            levelCounter++;
            LevelsCleared.text = "Stages Cleared: " + levelCounter;
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.2f);
        TotalTime.gameObject.SetActive(true);
        int timeCounter = 0;
        while (timeCounter < totalTime)
        {
            timeCounter++;
            TotalTime.text = "Total Time: " + timeCounter;
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(.2f);
        FinalPoints.gameObject.SetActive(true);
        int pointCounter = 0;
        while (pointCounter < points)
        {
            pointCounter+= 5;
            FinalPoints.text = "Points: " + pointCounter;
            yield return 0;
        }

        if (points > HighScore)
        {
            NewBest.gameObject.SetActive(true);
            yield return new WaitForSeconds(.2f);
            highScore = points;
            while (hiScoreCounter < highScore)
            {
                hiScoreCounter+=5;
                PR.text = "Best: " + hiScoreCounter;
                yield return 0;
            }
            PlayerPrefs.SetInt("HighScore",highScore);
            PlayerPrefs.Save();
        }
       
    }
}