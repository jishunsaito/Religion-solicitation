using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class gamedirector : MonoBehaviour
{
    [System.Serializable]
    public class Enemy
    {
        public Sprite image; // 画像
        public bool flag; //真理値
    }
    public int n = 0;//倒した相手の数
    public Enemy[] enemy;//Enemy型の配列
    private GameObject enemyObject;//出現する人
    private Vector2 start_pos;//人の開始地点
    private Vector2 target;//人の停止地点
    private float speed;//出現スピード 1.8
    private float back = -0.7f;//退場する速度
    private float grad = 0.85f;//スピードの勾配 0.85
    private bool can_ans = false;//キーボードを押せるか
    private bool exit = false;
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
        can_ans = false;
        exit = false;
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        start_pos = new Vector2(10.0f, 2.0f);
        target = new Vector2(0.0f, 2.0f);
        
        
        Show_enemy();
    }

    IEnumerator OnZkeypressed()
    {
        yield return new WaitForSeconds(0.25f);
        Check(true);
    }

    IEnumerator OnXkeypressed()
    {
        yield return new WaitForSeconds(0.2f);
        Check(false);
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
        }

        
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
            if (enemyObject.transform.position.x < -10.0f)
            {
                Destroy(enemyObject);
                Show_enemy();
            }

        }
        else if(exit && enemy[r].flag)
        {
            enemyObject.transform.Translate(back, 0.4f, 0,Space.World);
            enemyObject.transform.Rotate(0, 0, 30);
            if(enemyObject.transform.position.x < -10.0f)
            {
                Destroy(enemyObject);
                Show_enemy();
            }
            

        }
 
        //キー操作
        if ((Vector2)enemyObject.transform.position == target)
        {
            can_ans = true;//敵の停止を確認
        }
        if (can_ans)
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
