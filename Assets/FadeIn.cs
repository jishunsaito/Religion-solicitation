using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;//UIを使うとき追加

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;//CanvasGroupのpanel変数
    [SerializeField] private CanvasGroup result;//CanvasGroupのresult変数
    [SerializeField] private CanvasGroup showresult;//CanvasGroupのshowresult変数
    [SerializeField] AudioSource seAudioSource;//音再生用の変数

   
    void Start()
    {
        panel.alpha = 0.0f;//変数panelのalpha値を変更
        result.alpha = 0.0f;//変数resultのalpha値を変更
        showresult.alpha = 0.0f;//変数showresultのalpha値を変更
        

        InvokeRepeating(nameof(Panel), 1.5f, 0.1f);
        InvokeRepeating(nameof(Result), 3.0f, 0.1f);
        InvokeRepeating(nameof(Showresult), 4f, 0.1f);
    }

    void Panel()//Panelのフェードイン
    {
        if (panel.alpha <= 1)
        {
            panel.alpha += 0.1f;

        }
        else
        {
            CancelInvoke(nameof(Panel));
        }
    }

    void Result()//結果テキストの表示
    {
        if (result.alpha < 1)
        {
            result.alpha += 1f;
            seAudioSource.Play();
        }

        else
        {
            CancelInvoke(nameof(Result));
        }
    }

     void Showresult()//スコアの表示
    {
        if (showresult.alpha < 1)
        {
            showresult.alpha += 1f;
            seAudioSource.Play();
        }
    }

}
    
