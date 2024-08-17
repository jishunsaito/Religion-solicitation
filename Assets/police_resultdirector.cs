using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class police_resultdirector : MonoBehaviour
{
    GameObject Score;
    GameObject Result;
    GameObject ScoreText;
    GameObject Restart_button;
    GameObject Backstart_button;

    // AudioMixerGroup��ݒ肷�邽�߂̃t�B�[���h
    public AudioMixer audioMixer; // AudioMixer���h���b�O���h���b�v�Őݒ肵�܂�
    public AudioMixerGroup SEAudioMixerGroup; // SE�p��AudioMixerGroup

    private AudioSource SEaudiosource;
    public AudioClip display_SE;
    public AudioClip display2_SE;
    public AudioClip charge_SE;

    // ���ʐݒ�p�̕ϐ�
    [Range(0f, 1f)]
    public float display_SEVolume = 0.1f; // display_SE�̉���
    [Range(0f, 1f)]
    public float display2_SEVolume = 0.1f; // display2_SE�̉���
    [Range(0f, 1f)]
    public float charge_SEVolume = 0.1f; // charge_SE�̉���

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
        SEaudiosource.outputAudioMixerGroup = SEAudioMixerGroup; // AudioMixerGroup��ݒ�
        SEaudiosource.volume = display_SEVolume; // �f�t�H���g���ʂ�ݒ�

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
            PlaySE(charge_SE, charge_SEVolume);
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
        PlaySE(display_SE, display_SEVolume);
        Result.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        PlaySE(display_SE, display_SEVolume);
        Score.gameObject.SetActive(true);
        yield return StartCoroutine(Increase_Score());
        PlaySE(display2_SE, display2_SEVolume);
        ScoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Restart_button.gameObject.SetActive(true);
        Backstart_button.gameObject.SetActive(true);
    }

    void PlaySE(AudioClip clip, float volume)
    {
        SEaudiosource.clip = clip;
        SEaudiosource.volume = volume; // ���ʂ�ݒ�
        SEaudiosource.loop = false;
        SEaudiosource.Play();
    }
}
