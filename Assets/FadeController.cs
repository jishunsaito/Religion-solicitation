using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UIを使うとき追加

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup a;
    [SerializeField] private CanvasGroup b;//CanvasGroup型の変数aを宣言　あとでCanvasGroupをアタッチする


    void Start()
    {
        a.alpha = 0.0f;
        b.alpha = 0.0f;//変数aのalpha値を変更
        InvokeRepeating(nameof(Fadein), 3.0f, 0.1f);
        InvokeRepeating(nameof(FadeOut), 1.5f, 0.1f);
    }

    void Fadein()
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

    void FadeOut()
    {
        if (b.alpha <= 1)
        {
            b.alpha += 0.1f;

        }
        else
        {
            CancelInvoke();
        }
    }
}