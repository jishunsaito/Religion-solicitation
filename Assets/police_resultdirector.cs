using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class police_resultdirector : MonoBehaviour
{
    GameObject Score;
    GameObject Result;
    GameObject ScoreText;
    GameObject Restart_button;
    GameObject Backstart_button;

    private AudioSource SEaudiosource;
    public AudioClip display_SE;
    public AudioClip display2_SE;
    public AudioClip charge_SE;
    int n = gamedirector.n;
    // Start is called before the first frame update
    void Start()
    {
        this.Score = GameObject.Find("Score");
        this.Result = GameObject.Find("Result");
        this.ScoreText = GameObject.Find("ScoreText");
        this.Restart_button = GameObject.Find("Restartbutton");
        this.Backstart_button = GameObject.Find("Startbutton");
        SEaudiosource = gameObject.AddComponent<AudioSource>();
        Score.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(false);
        Result.gameObject.SetActive(false);
        Restart_button.gameObject.SetActive(false);
        Backstart_button.gameObject.SetActive(false);
        StartCoroutine(DisplayUI());
    }
    IEnumerator Increase_Score()
    {
        if(n == 0)
        {
            this.Score.GetComponent<TextMeshProUGUI>().text = "0";
        }
        else
        {
            PlaySE(charge_SE, 1.0f);
            float interval = 2f / Mathf.Max(n, 1);
            for (int i = 0; i <= n; i++)
            {
                this.Score.GetComponent<TextMeshProUGUI>().text = i.ToString();
                yield return new WaitForSeconds(interval);
            }
        }

    }


    IEnumerator DisplayUI()
    {
        yield return new WaitForSeconds(2.0f);
        PlaySE(display_SE, 0.5f);
        Result.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        PlaySE(display_SE, 0.5f);
        Score.gameObject.SetActive(true);
        yield return StartCoroutine(Increase_Score());
        PlaySE(display2_SE, 0.5f);
        ScoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Restart_button.gameObject.SetActive(true);
        Backstart_button.gameObject.SetActive(true);
    }

    void PlaySE(AudioClip clip, float volume)
    {
        SEaudiosource.clip = clip;
        SEaudiosource.volume = volume; // âπó Çê›íË
        SEaudiosource.loop = false;
        SEaudiosource.Play();
    }
}
