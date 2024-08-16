using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;//UI���g���Ƃ��ǉ�

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;//CanvasGroup��panel�ϐ�
    [SerializeField] private CanvasGroup result;//CanvasGroup��result�ϐ�
    [SerializeField] private CanvasGroup showresult;//CanvasGroup��showresult�ϐ�
    [SerializeField] AudioSource seAudioSource;//���Đ��p�̕ϐ�

   
    void Start()
    {
        panel.alpha = 0.0f;//�ϐ�panel��alpha�l��ύX
        result.alpha = 0.0f;//�ϐ�result��alpha�l��ύX
        showresult.alpha = 0.0f;//�ϐ�showresult��alpha�l��ύX
        

        InvokeRepeating(nameof(Panel), 1.5f, 0.1f);
        InvokeRepeating(nameof(Result), 3.0f, 0.1f);
        InvokeRepeating(nameof(Showresult), 4f, 0.1f);
    }

    void Panel()//Panel�̃t�F�[�h�C��
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

    void Result()//���ʃe�L�X�g�̕\��
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

     void Showresult()//�X�R�A�̕\��
    {
        if (showresult.alpha < 1)
        {
            showresult.alpha += 1f;
            seAudioSource.Play();
        }
    }

}
    
