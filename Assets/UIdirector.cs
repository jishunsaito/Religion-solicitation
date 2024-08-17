using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIdirector : MonoBehaviour
{
    private gamedirector gameDirector;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI countText;
    public RectTransform RectScore;
    private float span = 3.0f;
    private float delta = 0;
    private bool showtext = false;
    private int p_score = -1;
    private bool movingUp = true; // 移動方向を追跡
    private bool isPaused = false; // 停止フラグ
    private float pauseTime = 1.0f; // 停止時間
    private float pauseTimer = 0; // 停止タイマー

    void Start()
    {
        gameDirector = GameObject.Find("mainsystem").GetComponent<gamedirector>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        countText = GameObject.Find("countdown").GetComponent<TextMeshProUGUI>();
        scoreText.gameObject.SetActive(false);
        RectScore = scoreText.GetComponent<RectTransform>();
    }

    IEnumerator Waited()
    {
        countText.text = "START";
        yield return new WaitForSeconds(0.4f);
        countText.gameObject.SetActive(false);
        gamedirector.start_count = -1;
    }

    void Update()
    {
        if (gamedirector.start_count > 0)
        {
            countText.text = gamedirector.start_count.ToString();
            countText.gameObject.SetActive(true); // カウントダウンUIを表示
        }
        else if (gamedirector.start_count == 0)
        {
            StartCoroutine(Waited());
        }

        if (gamedirector.n % 5 == 0 && gamedirector.n != 0 && !showtext && gamedirector.n != p_score)
        {
            delta = 0;
            showtext = true;
            p_score = gamedirector.n;
            scoreText.gameObject.SetActive(true); // テキストを表示
            scoreText.text = gamedirector.n.ToString() + "人がえし";
            movingUp = true; // 初期は上に移動
            isPaused = false; // 停止フラグをリセット
        }

        if (showtext)
        {
            delta += Time.deltaTime;

            if (isPaused)
            {
                pauseTimer += Time.deltaTime;
                if (pauseTimer >= pauseTime)
                {
                    isPaused = false;
                    pauseTimer = 0;
                }
            }
            else
            {
                if (movingUp)
                {
                    RectScore.position += new Vector3(0, 4f, 0);

                    if (RectScore.anchoredPosition.y >= 120f) // 高すぎる場合は方向転換
                    {
                        movingUp = false;
                    }
                }
                else
                {
                    RectScore.position -= new Vector3(0, 4.0f, 0);

                    if (RectScore.anchoredPosition.y <= -50f) // 低すぎる場合は一時停止
                    {
                        isPaused = true; // 停止フラグを設定
                        movingUp = true; // 方向を上に変更
                    }
                }
            }

            if (delta > span)
            {
                showtext = false;
                scoreText.gameObject.SetActive(false);
            }
        }
    }
}

