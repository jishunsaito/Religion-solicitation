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
        public Sprite image; // �摜
        public bool flag; //�^���l
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
    public static int n = 0;//�|��������̐�
    public Enemy[] enemy;//Enemy�^�̔z��
    private GameObject enemyObject;//�o������l
    private GameObject effect;
    private Vector2 start_pos;//�l�̊J�n�n�_
    private Vector2 target;//�l�̒�~�n�_
    private float speed;//�o���X�s�[�h 1.8
    private float back = -1.5f;//�ޏꂷ�鑬�x
    private float grad = 0.85f;//�X�s�[�h�̌��z 0.85
    private float timeBuffer = 0.2f;
    private float countdown;//��������
    private bool can_ans = false;//�L�[�{�[�h�������邩
    private bool exit = false;//�ޏꒆ
    private bool answering = false;//�񓚒�
    public static  bool timeup = false;//���Ԑ؂�

    // AudioMixerGroup��ݒ肷�邽�߂̃t�B�[���h
    public AudioMixer audioMixer; // AudioMixer���h���b�O���h���b�v�Őݒ肵�܂�
    public AudioMixerGroup SEAudioMixerGroup; // SE�p��AudioMixerGroup
    public AudioMixerGroup BGMAudioMixerGroup; // BGM�p��AudioMixerGroup
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

    // ���ʐݒ�p�̕ϐ�
    [Range(0f, 1f)]
    public float punchSEVolume = 0.1f; // PunchSE�̉���
    [Range(0f, 1f)]
    public float wavingSEVolume = 0.1f; // WavingSE�̉���
    [Range(0f, 1f)]
    public float bigpunchSEVolume = 0.1f; // BigPunchSE�̉���
    [Range(0f, 1f)]
    public float surprisedSEVolume = 0.5f; // surprisedSE�̉���
    [Range(0f, 1f)]
    public float countdownSEVolume = 0.1f; // countdownSE�̉���

    int r;//�o������l�����߂闐���l;

    //�G�̏o��
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
        r = Random.Range(mode[level_idx].r_range_min, mode[level_idx].r_range_max);//0�܂���1
        speed = 1.8f * mode[level_idx].speed_up;
        enemyObject = new GameObject("Enemy");
        SpriteRenderer sr = enemyObject.AddComponent<SpriteRenderer>();
        sr.sprite = enemy[r].image;//�z��enemy��r�Ԗڂ̗v�f�̉摜
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
        SEaudiosource.outputAudioMixerGroup = SEAudioMixerGroup; // AudioMixerGroup��ݒ�
        SEaudiosource.volume = punchSEVolume; // �f�t�H���g���ʂ�ݒ�

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
        BGMaudiosource.outputAudioMixerGroup = BGMAudioMixerGroup; // AudioMixerGroup��ݒ�
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

        // �^�C�}�[�̒�~
        showImageTimer.m_gameTimer.OnStop();

        // �x������
        yield return new WaitForSeconds(0.25f / mode[level_idx].speed_up);
        effect.SetActive(true);
        StartCoroutine(Shake(0.1f, 0.4f));
        yield return new WaitForSeconds(0.1f / mode[level_idx].speed_up);

        // ���𔻒�
        Check(true);

        effect.SetActive(false);
    }

    IEnumerator OnZkeypressed()
    {
        PlaySE(waving_SE, wavingSEVolume);
        waving.SetTrigger("handAnim");
        answering = true;

        // �^�C�}�[�̒�~
        showImageTimer.m_gameTimer.OnStop();

        // �x������
        yield return new WaitForSeconds(0.2f / mode[level_idx].speed_up);

        // ���𔻒�
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
        SEaudiosource.volume = volume; // ���ʂ�ݒ�
        SEaudiosource.loop = false;
        SEaudiosource.Play();
    }


    void Check(bool ans)
    {
        if (!timeup) // �^�C�}�[���؂�Ă��Ȃ��ꍇ�̂ݏ���
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
        //�s�����̎��̓G�̍s��0.5�b��Ƀv���C���[�ɋ߂Â�
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
        // Object������Ƃ��Ɉړ�
        if (enemyObject != null)
        {
            speed *= grad;
            enemyObject.transform.position = Vector2.MoveTowards(
                enemyObject.transform.position,
                target,
                speed
            );
        }

        // �ޏ�̉��o
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

        // �L�[����
        if (enemyObject != null && enemyObject.transform.position.x < 1.0f && enemyObject.transform.position.x >= 0.0f)
        {
            can_ans = true; // �G�̒�~���m�F
            countdown -= Time.deltaTime;

            // �{�^�������������Ɏ��Ԑ؂ꔻ���ۗ�
            if (countdown < timeBuffer && !timeup && !answering)
            {
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
                {
                    countdown = timeBuffer; // �P�\���Ԃ��m��
                }
            }

            // ���Ԑ؂ꔻ��
            if (countdown < 0 && !timeup)
            {
                if (!answering) // ���͂��Ȃ��ꍇ�̂ݎ��Ԑ؂ꏈ��
                {
                    showImageTimer.m_gameTimer.OnStop();
                    Debug.Log("���Ԑ؂�");
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
