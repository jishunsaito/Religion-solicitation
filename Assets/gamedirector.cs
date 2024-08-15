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
        public int level;
        public float ans_time;
        public float speed_up;
        public float r_range;
    }
    public Level[] mode;
    public static int n = 999;//倒した相手の数
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
    private AudioSource audiosource;
    private Animator punching;
    private Animator waving;
    public AudioClip punch_SE;
    public AudioClip waving_SE;




    int r;//出現する人を決める乱数値;

    //敵の出現
    void Show_enemy()
    {
        r = Random.Range(0, 2);//0または1
        speed = 1.8f;
        enemyObject = new GameObject("Enemy");
        SpriteRenderer sr = enemyObject.AddComponent<SpriteRenderer>();
        sr.sprite = enemy[r].image;//配列enemyのr番目の要素の画像
        enemyObject.transform.position = start_pos;
        enemyObject.transform.localScale += Vector3.right * 1.05f;
        enemyObject.transform.localScale += Vector3.up * 1.05f;
        can_ans = false;
        exit = false;
        countdown = 2.0f;
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        start_pos = new Vector2(10.0f, 1.5f);
        target = new Vector2(0.0f, 1.5f);
        punching = GameObject.Find("punching1").GetComponent<Animator>();
        waving = GameObject.Find("hand1").GetComponent<Animator>();
        audiosource = gameObject.GetComponent<AudioSource>();
        Show_enemy();
    }

    IEnumerator OnXkeypressed()
    {
        answering = true;
        punching.SetTrigger("punchAnim");
        PlaySE(punch_SE);
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(Shake(0.1f,0.2f));
        yield return new WaitForSeconds(0.1f);
        Check(true);
    
        
    }

    IEnumerator OnZkeypressed()
    {
        PlaySE(waving_SE);
        waving.SetTrigger("handAnim");
        answering = true;
        yield return new WaitForSeconds(0.2f);
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
        audiosource.clip = clip;
        audiosource.Play();
    }


    void Check(bool ans)
    {
        if (ans == enemy[r].flag)
        {
            Debug.Log("正解");
            n++;
            exit = true;
            
        }
        else
        {
            Debug.Log("不正解");
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
        if (enemyObject.transform.position.x < 1.5f && enemyObject.transform.position.x >= 0.0f)
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
