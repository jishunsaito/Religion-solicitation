using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class darker_contoroller : MonoBehaviour
{
    public Image darkenPanel; // �p�l����Image�R���|�[�l���g���A�^�b�`
    public float darkenDuration = 4.0f; // �Â�����܂ł̎��ԁi�b�j

    private float currentAlpha = 0f; // ���݂̃A���t�@�l
    private bool isDarkening = false;

    void Start()
    {
        // �p�l�������S�ɓ����ɐݒ�
        darkenPanel.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (isDarkening)
        {
            // �A���t�@�l�����Ԃɉ����đ���
            currentAlpha += Time.deltaTime / darkenDuration;

            // �A���t�@�l��0����1�̊Ԃɐ���
            currentAlpha = Mathf.Clamp(currentAlpha,0f,0.7f);

            // �p�l���̐F�ɃA���t�@�l��K�p
            darkenPanel.color = new Color(0, 0, 0, currentAlpha);

            // �A���t�@�l��1�ɒB������A�Â�����̂��~
            if (currentAlpha >= 0.7f)
            {
                isDarkening = false;
            }
        }
    }

    // �O������Ăяo���֐��i�Â����鏈�����J�n�j
    public void StartDarkening()
    {
        isDarkening = true;
    }
}
