using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



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
        public int r_range;
    }
    public Level[] mode;
    private int level_idx = 0;
    public static int n = 0;//倒した相手の数
    public Enemy[] enemy;//Enemy型の配列
    private GameObject enemyObject;//出現する人
    private Vector2 start_pos;//人の開始地点
    private Vector2 target;//人の停止地点
    private float speed;//出現スピード 1.8
    private float back = -1.5f;//退場する速度
    private float grad = 0.85f;//スピードの勾配 0.85
    private float countdown;//制限時間
    private bool can_ans = false;//キーボードを押せるか
    private bool exit = false;//退場中
    private bool answering = false;//回答中
    private bool timeup = false;//時間切れ
    private AudioSource SEaudiosource;
    private AudioSource BGMaudiosource;
    private Animator punching;
    private Animator waving;
    public AudioClip punch_SE;
    public AudioClip waving_SE;
    public AudioClip BGM;




    int r;//出現する人を決める乱数値;

    //敵の出現
    void Show_enemy()
    {
        Debug.Log(level_idx);
        if(n%10 == 0 && n <= 40 && n!= 0)
        {
            level_idx++;
            SEaudiosource.pitch = mode[level_idx].speed_up;
            BGMaudiosource.pitch = mode[level_idx].speed_up;
        }
        r = Random.Range(0, mode[level_idx].r_range);//0または1
        speed = 1.8f* mode[level_idx].speed_up;
        enemyObject = new GameObject("Enemy");
        SpriteRenderer sr = enemyObject.AddComponent<SpriteRenderer>();
        sr.sprite = enemy[r].image;//配列enemyのr番目の要素の画像
        enemyObject.transform.position = start_pos;
        enemyObject.transform.localScale += Vector3.right * 1.05f;
        enemyObject.transform.localScale += Vector3.up * 1.05f;
        can_ans = false;
        exit = false;
        countdown = mode[level_idx].ans_time;
        
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        start_pos = new Vector2(10.0f, 1.5f);
        target = new Vector2(0.0f, 1.5f);
        punching = GameObject.Find("punching1").GetComponent<Animator>();
        waving = GameObject.Find("hand1").GetComponent<Animator>();

        SEaudiosource = gameObject.AddComponent<AudioSource>();
        SEaudiosource.volume = 0.05f;

        BGMaudiosource = gameObject.AddComponent<AudioSource>();
        BGMaudiosource.clip = BGM;
        BGMaudiosource.loop = true;
        BGMaudiosource.volume = 0.05f;
        BGMaudiosource.pitch = mode[0].speed_up;
        SEaudiosource.pitch = mode[0].speed_up;
        BGMaudiosource.Play();
        Show_enemy();
    }

    IEnumerator OnXkeypressed()
    {
        answering = true;
        punching.SetTrigger("punchAnim");
        PlaySE(punch_SE);
        yield return new WaitForSeconds(0.25f/ mode[level_idx].speed_up);
        StartCoroutine(Shake(0.1f,0.2f));
        yield return new WaitForSeconds(0.1f/ mode[level_idx].speed_up);
        Check(true);
    
        
    }

    IEnumerator OnZkeypressed()
    {
        PlaySE(waving_SE);
        waving.SetTrigger("handAnim");
        answering = true;
        yield return new WaitForSeconds(0.2f / mode[level_idx].speed_up);
        Check(false);
        

    }
    IEnumerator Shake(float span,float degree)
    {
        Vector2 n_pos = enemyObject.transform.position;
        float delta = 0f;
        while(delta < span)
        {
            float shake_x = Random.Range(-degree, degree);
            float shake_y = Random.Range(-degree, degree);
            enemyObject.transform.position = new Vector2(n_pos.x+shake_x,n_pos.y+shake_y);
            delta += Time.deltaTime;
            yield return null;
        }
        enemyObject.transform.position = n_pos;
    }

    void PlaySE(AudioClip clip)
    {
        SEaudiosource.clip = clip;
        SEaudiosource.loop = false;
        SEaudiosource.Play();
    }


    void Check(bool ans)
    {
        if (ans == enemy[r].flag)
        {
            n++;
            Debug.Log(n);
            exit = true;
            
        }
        else
        {
            StartCoroutine(Loose());
            timeup = true;
        }
        can_ans = false;
        

        
    }
    IEnumerator Loose()
    {
        //不正解の時の敵の行動0.5秒後にプレイヤーに近づく
        answering = true;
        yield return new WaitForSeconds(1.0f);
        enemyObject.transform.localScale += Vector3.right*4;
        enemyObject.transform.localScale += Vector3.up * 4;
        enemyObject.transform.Translate(0,-15,0);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene("ResultScene");

    }

    void Update()
    {
       

        //Objectがあるときに移動
        if (enemyObject != null)
        {
            
            speed *= grad;
            enemyObject.transform.position = Vector2.MoveTowards(
                enemyObject.transform.position,
                target,
                speed
            ) ;
        }
        //退場の演出
        if (exit && !enemy[r].flag)
        {
            enemyObject.transform.Translate(back, 0, 0);
            if (enemyObject.transform.position.x < -20.0f)
            {
                answering = false;
                Destroy(enemyObject);
                Show_enemy();
            }

        }

        else if(exit && enemy[r].flag)
        {
            enemyObject.transform.Translate(back, 0.7f, 0,Space.World);
            enemyObject.transform.Rotate(0, 0, 30);
            if(enemyObject.transform.position.x < -20.0f)
            {
                answering = false;
                Destroy(enemyObject);
                Show_enemy();
            }
        }


        //キー操作
        if (enemyObject.transform.position.x < 1.0f && enemyObject.transform.position.x >= 0.0f)
        {
            can_ans = true;//敵の停止を確認
            countdown -= Time.deltaTime;
            if (countdown < 0 && !timeup)
            {
                Debug.Log("時間切れ");
                timeup = true;
                StartCoroutine(Loose());
            }
            
        }
        if (can_ans && !answering)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            { 
                StartCoroutine(OnZkeypressed());
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(OnXkeypressed());
            }

        }
    }
}
