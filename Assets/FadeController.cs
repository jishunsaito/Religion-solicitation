using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI���g���Ƃ��ǉ�

public class FadeController : MonoBehaviour
{
    [SerializeField] private CanvasGroup a;//CanvasGroup�^�̕ϐ�a��錾�@���Ƃ�CanvasGroup���A�^�b�`����
     
    
    void Start()
    {
        a.alpha = 0.0f;//�ϐ�a��alpha�l��ύX
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