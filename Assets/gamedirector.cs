using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using unityroom.Api;



public class gamedirector : MonoBehaviour
{
    [System.Serializable]
    public class Enemy
    {
        public Sprite image; // 画像
        public bool flag; //真理値
    }
    [System.Serializable]
    public class Level
    {
        public float ans_time;
        public float speed_up;
        public int r_range_max;
        public int r_range_min;
    }
    public Level[] mode;
    public int level_idx = 0;
    public static int start_count;
    public static int n = 0;//倒した相手の数
    public Enemy[] enemy;//Enemy型の配列
    private GameObject enemyObject;//出現する人
    private GameObject effect;
    private Vector2 start_pos;//人の開始地点
    private Vector2 target;//人の停止地点
    private float speed;//出現スピード 1.8
    private float back = -1.5f;//退場する速度
    private float grad = 0.85f;//スピードの勾配 0.85
    private float timeBuffer = 0.2f;
    private float countdown;//制限時間
    private bool can_ans = false;//キーボードを押せるか
    private bool exit = false;//退場中
    private bool answering = false;//回答中
    public static  bool timeup = false;//時間切れ

    // AudioMixerGroupを設定するためのフィールド
    public AudioMixer audioMixer; // AudioMixerをドラッグ＆ドロップで設定します
    public AudioMixerGroup SEAudioMixerGroup; // SE用のAudioMixerGroup
    public AudioMixerGroup BGMAudioMixerGroup; // BGM用のAudioMixerGroup
    private AudioSource SEaudiosource;
    private AudioSource BGMaudiosource;
    private Animator punching;
    private Animator waving;
    public AudioClip punch_SE;
    public AudioClip bigpunch_SE;
    public AudioClip waving_SE;
    public AudioClip surprised_SE;
    public AudioClip countdown_SE;
    public AudioClip TimeUP_SE;
    public AudioClip BGM;
    private ShowImageTimer showImageTimer;

    // 音量設定用の変数
    [Range(0f, 1f)]
    public float punchSEVolume = 0.1f; // PunchSEの音量
    [Range(0f, 1f)]
    public float wavingSEVolume = 0.1f; // WavingSEの音量
    [Range(0f, 1f)]
    public float bigpunchSEVolume = 0.1f; // BigPunchSEの音量
    [Range(0f, 1f)]
    public float surprisedSEVolume = 0.5f; // surprisedSEの音量
    [Range(0f, 1f)]
    public float countdownSEVolume = 0.1f; // countdownSEの音量

    int r;//出現する人を決める乱数値;

    //敵の出現
    void Show_enemy()
    {
        if (n % 15 == 0 && n <= 45 && n != 0)
        {
            level_idx++;
            SEaudiosource.pitch = mode[level_idx].speed_up * 1.6f;
            BGMaudiosource.pitch = mode[level_idx].speed_up;
            punching.speed = mode[level_idx].speed_up;

            showImageTimer.m_gameTimer.SetMaxTime(mode[level_idx].ans_time);

        }
        r = Random.Range(mode[level_idx].r_range_min, mode[level_idx].r_range_max);//0または1
        speed = 1.8f * mode[level_idx].speed_up;
        enemyObject = new GameObject("Enemy");
        SpriteRenderer sr = enemyObject.AddComponent<SpriteRenderer>();
        sr.sprite = enemy[r].image;//配列enemyのr番目の要素の画像
        enemyObject.transform.position = start_pos;
        enemyObject.transform.localScale += Vector3.right * 1.05f;
        enemyObject.transform.localScale += Vector3.up * 1.05f;
        can_ans = false;
        exit = false;
        showImageTimer.m_gameTimer.OnReset();
        showImageTimer.m_gameTimer.OnStart();
        countdown = mode[level_idx].ans_time;

    }

    void Start()
    {
        Application.targetFrameRate = 60;
        n = 0;
        start_count = 3;
        timeup = false;
        start_pos = new Vector2(10.0f, 1.5f);
        target = new Vector2(0.0f, 1.5f);
        punching = GameObject.Find("punching1").GetComponent<Animator>();
        waving = GameObject.Find("hand1").GetComponent<Animator>();
        effect = GameObject.Find("punch_effect");
        effect.SetActive(false);


        SEaudiosource = gameObject.AddComponent<AudioSource>();
        SEaudiosource.outputAudioMixerGroup = SEAudioMixerGroup; // AudioMixerGroupを設定
        SEaudiosource.volume = punchSEVolume; // デフォルト音量を設定

        showImageTimer = FindObjectOfType<ShowImageTimer>();
        showImageTimer.m_gameTimer.SetMaxTime(mode[level_idx].ans_time);

        StartCoroutine(StartSequence());



    }
    IEnumerator StartSequence()
    {
        yield return StartCoroutine(GameStart());
        BGMaudiosource = gameObject.AddComponent<AudioSource>();
        BGMaudiosource.clip = BGM;
        BGMaudiosource.loop = true;
        BGMaudiosource.volume = 0.12f;
        BGMaudiosource.outputAudioMixerGroup = BGMAudioMixerGroup; // AudioMixerGroupを設定
        BGMaudiosource.pitch = mode[0].speed_up;
        BGMaudiosource.Play();
        yield return new WaitForSeconds(0.5f);
        Show_enemy();
        yield return new WaitForSeconds(0.9f);
        SEaudiosource.pitch = mode[0].speed_up * 1.6f;
    }
    IEnumerator GameStart()
    {
        PlaySE(countdown_SE, countdownSEVolume);
        for (int i = 0; i < 3; i++)
        {
            start_count = 3 - i;
            yield return new WaitForSeconds(1.0f);
        }
        start_count = 0;
    }


    IEnumerator OnXkeypressed()
    {
        answering = true;
        punching.SetTrigger("punchAnim");
        if (enemy[r].flag)
        {
            PlaySE(punch_SE, punchSEVolume);
        }
        else
        {
            PlaySE(bigpunch_SE, bigpunchSEVolume);
        }

        // タイマーの停止
        showImageTimer.m_gameTimer.OnStop();

        // 遅延処理
        yield return new WaitForSeconds(0.25f / mode[level_idx].speed_up);
        effect.SetActive(true);
        StartCoroutine(Shake(0.1f, 0.4f));
        yield return new WaitForSeconds(0.1f / mode[level_idx].speed_up);

        // 正解判定
        Check(true);

        effect.SetActive(false);
    }

    IEnumerator OnZkeypressed()
    {
        PlaySE(waving_SE, wavingSEVolume);
        waving.SetTrigger("handAnim");
        answering = true;

        // タイマーの停止
        showImageTimer.m_gameTimer.OnStop();

        // 遅延処理
        yield return new WaitForSeconds(0.2f / mode[level_idx].speed_up);

        // 正解判定
        Check(false);
    }
    IEnumerator Shake(float span, float degree)
    {
        Vector2 n_pos = enemyObject.transform.position;
        float delta = 0f;
        while (delta < span)
        {
            float shake_x = Random.Range(-degree, degree);
            float shake_y = Random.Range(-degree, degree);
            enemyObject.transform.position = new Vector2(n_pos.x + shake_x, n_pos.y + shake_y);
            delta += Time.deltaTime;
            yield return null;
        }
        enemyObject.transform.position = n_pos;
    }

    void PlaySE(AudioClip clip, float volume)
    {
        SEaudiosource.clip = clip;
        SEaudiosource.volume = volume; // 音量を設定
        SEaudiosource.loop = false;
        SEaudiosource.Play();
    }


    void Check(bool ans)
    {
        if (!timeup) // タイマーが切れていない場合のみ処理
        {
            if (ans == enemy[r].flag)
            {
                n++;
                exit = true;
                timeup = false;
            }
            else
            {
                StartCoroutine(Loose());
                timeup = true;
              
            }
            can_ans = false;
        }
    }
    IEnumerator Loose()
    {
        UnityroomApiClient.Instance.SendScore(1, n, ScoreboardWriteMode.HighScoreDesc);
        //不正解の時の敵の行動0.5秒後にプレイヤーに近づく
        answering = true;
        BGMaudiosource.Stop();
        showImageTimer.m_gameTimer.OnStop();
        yield return new WaitForSeconds(0.2f);
        PlaySE(TimeUP_SE, 1.0f);
        yield return new WaitForSeconds(2.0f);
        enemyObject.transform.localScale += Vector3.right * 4;
        enemyObject.transform.localScale += Vector3.up * 4;
        enemyObject.transform.Translate(0, -15, 0);
        SEaudiosource.pitch = 1.0f;
        SEaudiosource.volume = 1.0f;
        PlaySE(surprised_SE, surprisedSEVolume);
        yield return new WaitForSeconds(1.5f);
        if (enemy[r].flag)
        {
            FadeManager.Instance.LoadScene("Cult_Result", 0.4f);
        }
        else
        {
            FadeManager.Instance.LoadScene("Police_Result", 0.4f);
        }

    }

    void Update()
    {
        // Objectがあるときに移動
        if (enemyObject != null)
        {
            speed *= grad;
            enemyObject.transform.position = Vector2.MoveTowards(
                enemyObject.transform.position,
                target,
                speed
            );
        }

        // 退場の演出
        if (exit && !enemy[r].flag)
        {
            enemyObject.transform.Translate(back * mode[level_idx].speed_up, 0, 0);
            if (enemyObject.transform.position.x < -20.0f)
            {
                answering = false;
                Destroy(enemyObject);
                Show_enemy();
            }
        }
        else if (exit && enemy[r].flag)
        {
            enemyObject.transform.Translate(back * mode[level_idx].speed_up, 0.7f, 0, Space.World);
            enemyObject.transform.Rotate(0, 0, 30);
            if (enemyObject.transform.position.x < -20.0f)
            {
                answering = false;
                Destroy(enemyObject);
                Show_enemy();
            }
        }

        // キー操作
        if (enemyObject != null && enemyObject.transform.position.x < 1.0f && enemyObject.transform.position.x >= 0.0f)
        {
            can_ans = true; // 敵の停止を確認
            countdown -= Time.deltaTime;

            // ボタンを押した時に時間切れ判定を保留
            if (countdown < timeBuffer && !timeup && !answering)
            {
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
                {
                    countdown = timeBuffer; // 猶予時間を確保
                }
            }

            // 時間切れ判定
            if (countdown < 0 && !timeup)
            {
                if (!answering) // 入力がない場合のみ時間切れ処理
                {
                    showImageTimer.m_gameTimer.OnStop();
                    Debug.Log("時間切れ");
                    timeup = true;
                    can_ans = false;
                    StartCoroutine(Loose());
                }
            }
        }

        if (can_ans && !answering && !timeup)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StopCoroutine(Loose());
                StartCoroutine(OnZkeypressed());
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                StopCoroutine(Loose());
                StartCoroutine(OnXkeypressed());
            }
        }
    }
}
