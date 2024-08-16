using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class GameResult : MonoBehaviour
{
    [SerializeField] private CanvasGroup button;
    [SerializeField] private CanvasGroup taiho;
    [SerializeField] AudioSource countup;
    [SerializeField] AudioSource finish;
    TextMeshProUGUI Score;
    int result = gamedirector.n;
    private int i = 0;

    


    void Start()
    {

        button.alpha = 0.0f;//変数buttonのalpha値を変更
        taiho.alpha = 0.0f;


            if (i <= result)
            {
                InvokeRepeating(nameof(RepeatMsg), 4.3f, 0.1f);

            }



    }

    public void RepeatMsg()
    {
        if (i <= result)
        {
            Score = GetComponent<TextMeshProUGUI>();

            Score.text = i.ToString() + "人がえし！";

            i++;
            countup.Play();

        }
        else
        {
            CancelInvoke();
            
            InvokeRepeating(nameof(Taiho), 1f, 0.1f);
        }
    }

    void Taiho()
    {
        if (taiho.alpha < 1)
        {
            taiho.alpha += 0.2f; 
            
        }

        else
        {
            finish.Play();
            CancelInvoke();
            InvokeRepeating(nameof(Button), 1f, 0.1f);
        }
    }
    private void Button()
    {
        if (button.alpha < 1)
        {
            button.alpha += 1;
        }

        else
        {
            CancelInvoke();
        }
    }
}

