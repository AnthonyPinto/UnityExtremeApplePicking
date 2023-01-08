using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<int> onScoreChangedEvent;


    int score = 0;
    public void OnAppleCollected()
    {
        score++;
        onScoreChangedEvent.Invoke(score);
    }

    // Start is called before the first frame update
    void Start()
    {
        //foreach(UnityAction<int> listener in onScoreChangedListeners)
        //{
        //    onScoreChangedEvent.AddListener(listener);
        //}
    }

}
