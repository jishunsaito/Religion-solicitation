using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UIを使うとき追加

public class FadeController : MonoBehaviour
{
    [SerializeField] private CanvasGroup a;//CanvasGroup型の変数aを宣言　あとでCanvasGroupをアタッチする
     
    
    void Start()
    {
        a.alpha = 0.0f;//変数aのalpha値を変更
        InvokeRepeating(nameof(FadeIn), 0.5f, 0.0f);
    }

    void FadeIn()
    {
        if (a.alpha <= 1)
        {
            a.alpha += 0.1f;

        }
        else
        {
            CancelInvoke();
        }
    }
}