using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class darker_contoroller : MonoBehaviour
{
    public Image darkenPanel; // パネルのImageコンポーネントをアタッチ
    public float darkenDuration = 4.0f; // 暗くするまでの時間（秒）

    private float currentAlpha = 0f; // 現在のアルファ値
    private bool isDarkening = false;

    void Start()
    {
        // パネルを完全に透明に設定
        darkenPanel.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (isDarkening)
        {
            // アルファ値を時間に応じて増加
            currentAlpha += Time.deltaTime / darkenDuration;

            // アルファ値を0から1の間に制限
            currentAlpha = Mathf.Clamp(currentAlpha,0f,0.7f);

            // パネルの色にアルファ値を適用
            darkenPanel.color = new Color(0, 0, 0, currentAlpha);

            // アルファ値が1に達したら、暗くするのを停止
            if (currentAlpha >= 0.7f)
            {
                isDarkening = false;
            }
        }
    }

    // 外部から呼び出す関数（暗くする処理を開始）
    public void StartDarkening()
    {
        isDarkening = true;
    }
}
