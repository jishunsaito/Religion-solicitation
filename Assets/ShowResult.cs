using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameResult : MonoBehaviour
{
    TextMeshProUGUI Score;
    int result = gamedirector.n; 
    int i = 0;
    
  private void Start()
    {
        
        {
            if (i <= result)
            {
                InvokeRepeating(nameof(RepeatMsg), 0.5f, 0.1f);
  
            }

            
        }

       
            
        }

   void RepeatMsg()
        {
        if (i <= result)
        {
            Score = GetComponent<TextMeshProUGUI>();

            Score.text = i.ToString() + "êlî≤Ç´ÅI";

            i++;

        }
        else
            {
                CancelInvoke();
            }

            

    }
}
