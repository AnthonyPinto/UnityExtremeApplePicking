using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    public void OnScoreChanged(int score)
    {
        tmp.text = "" + score;
    }
}
