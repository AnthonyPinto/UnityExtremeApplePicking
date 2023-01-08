using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UnityEvent<int> onScoreChangedEvent;
    public UnityEvent<int> onStartTimerChangedEvent;
    public UnityEvent<int> onGameTimerChangedEvent;
    public UnityEvent<int> onGameOverEvent;
    public UnityEvent onGameStartEvent;

    float remainingStartTimer = 6;
    float remainingGameTimer = 61;


    int score = 0;
    public void OnAppleCollected()
    {
        score++;
        onScoreChangedEvent.Invoke(score);
    }

    public void OnRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        // Deal with start timer
        if (remainingStartTimer > 0)
        {
            float timerBeforeFrame = remainingStartTimer;
            remainingStartTimer -= Time.deltaTime;
            int rounded = Mathf.FloorToInt(remainingStartTimer);

            // If we passed a second report it
            if (rounded != Mathf.FloorToInt(timerBeforeFrame))
            {
                onStartTimerChangedEvent.Invoke(rounded);
            }

            if (rounded <= 0)
            {
                onGameStartEvent.Invoke();
                remainingStartTimer = 0;
            }

            
        } else if (remainingGameTimer > 0)
        {
            float timeBeforeFrame = remainingGameTimer;
            remainingGameTimer -= Time.deltaTime;
            int rounded = Mathf.FloorToInt(remainingGameTimer);

            if (rounded != Mathf.FloorToInt(timeBeforeFrame))
            {
                onGameTimerChangedEvent.Invoke(rounded);
            }

            if (rounded <= 0)
            {
                onGameOverEvent.Invoke(score);
                remainingGameTimer = 0;
                Time.timeScale = 0;
            }
        }
    }

}
