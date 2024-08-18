using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIdirector : MonoBehaviour
{
    private gamedirector gameDirector;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI countText;
    GameObject StartText;
    GameObject TimeupText;
    public RectTransform RectScore;
    public RectTransform Board;
    private float span = 4.0f;
    private float delta = 0;
    private bool showtext = false;
    private int p_score = -1;
    private bool movingUp = false; // �����͉��~�X�^�[�g
    private bool isPaused = false; // ��~�t���O
    private bool finished = false;
    private float pauseTime = 1.0f; // ��~����
    private float pauseTimer = 0; // ��~�^�C�}�[

    void Start()
    {
        gameDirector = GameObject.Find("mainsystem").GetComponent<gamedirector>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        countText = GameObject.Find("countdown").GetComponent<TextMeshProUGUI>();
        Board = GameObject.Find("Board").GetComponent<RectTransform>();
        this.TimeupText = GameObject.Find("TimeUP");
        this.StartText = GameObject.Find("Start");
        scoreText.gameObject.SetActive(false);
        RectScore = scoreText.GetComponent<RectTransform>();

        StartText.gameObject.SetActive(false);
        Board.gameObject.SetActive(false);
        TimeupText.gameObject.SetActive(false);
        finished = false;

        // �����ʒu��80�ɐݒ�
        RectScore.anchoredPosition = new Vector2(RectScore.anchoredPosition.x, 80f);
        Board.anchoredPosition = new Vector2(Board.anchoredPosition.x, 80f);
    }

    IEnumerator Waited()
    {
        StartText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        StartText.gameObject.SetActive(false);
        gamedirector.start_count = -1;
    }
    IEnumerator Finish()
    {
        TimeupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        TimeupText.gameObject.SetActive(false);
    }

    IEnumerator PauseAtBottom()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        isPaused = false;
        movingUp = true; // ��~��ɏ㏸�J�n
    }

    void Update()
    {

        if (gamedirector.timeup && !finished)
        {
            finished = true;
            StartCoroutine(Finish());
        }
        if (gamedirector.start_count > 0)
        {
            countText.text = gamedirector.start_count.ToString();
            countText.gameObject.SetActive(true); // �J�E���g�_�E��UI��\��
        }
        else if (gamedirector.start_count == 0)
        {
            countText.gameObject.SetActive(false);
            StartCoroutine(Waited());
        }

        if (gamedirector.n % 5 == 0 && gamedirector.n != 0 && !showtext && gamedirector.n != p_score)
        {
            delta = 0;
            showtext = true;
            p_score = gamedirector.n;
            Board.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true); // �e�L�X�g��\��
            scoreText.text = gamedirector.n.ToString() + "�l";
            movingUp = false; // �����͉��~�X�^�[�g
            isPaused = false; // ��~�t���O�����Z�b�g
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
                    RectScore.anchoredPosition += new Vector2(0, 4f);
                    Board.anchoredPosition += new Vector2(0, 4f);

                    if (RectScore.anchoredPosition.y >= 80f) // 80�܂ŏオ�������~
                    {
                        showtext = false;
                        isPaused = false;
                        scoreText.gameObject.SetActive(false);
                        Board.gameObject.SetActive(false);
                    }
                }
                else
                {
                    RectScore.anchoredPosition -= new Vector2(0, 4f);
                    Board.anchoredPosition -= new Vector2(0, 4f);

                    if (RectScore.anchoredPosition.y <= -80f) // -80�܂ŉ����������~����1�b��ɏ㏸�J�n
                    {
                        StartCoroutine(PauseAtBottom());
                    }
                }
            }

            if (delta > span)
            {
                showtext = false;
                scoreText.gameObject.SetActive(false);
                Board.gameObject.SetActive(false);
                TimeupText.gameObject.SetActive(false);
            }
        }
    }
}
