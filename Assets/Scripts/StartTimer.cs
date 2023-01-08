using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartTimer : MonoBehaviour
{

    public TextMeshProUGUI tmp;

    public void OnStartTimerChanged(int seconds) {
        Debug.Log(seconds);
        if (seconds > 0)
        {
            tmp.text = "" + seconds;
        } else
        {
            gameObject.SetActive(false);
        }
    }
}
