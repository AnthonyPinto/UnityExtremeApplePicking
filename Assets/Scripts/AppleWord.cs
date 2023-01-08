using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

 class AppleWord : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    public void OnScoreChanged(int score)
    {
        if (score == 1)
        {
            tmp.text = "APPLE";
        } else
        {
            tmp.text = "APPLES";
        }
    }
}
