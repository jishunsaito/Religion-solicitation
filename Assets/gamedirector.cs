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
        public Sprite image; // �摜
        public bool flag; //�^���l
    }
    public int n = 0;//�|��������̐�
    public Enemy[] enemy;//Enemy�^�̔z��
    private GameObject enemyObject;//�o������l
    private Vector2 start_pos;//�l�̊J�n�n�_
    private Vector2 target;//�l�̒�~�n�_
    private float speed;//�o���X�s�[�h 1.8
    private float back = -0.7f;//�ޏꂷ�鑬�x
    private float grad = 0.85f;//�X�s�[�h�̌��z 0.85
    private bool can_ans = false;//�L�[�{�[�h�������邩
    private bool exit = false;
    int r;//�o������l�����߂闐���l;

    //�G�̏o��
    void Show_enemy()
    {
        r = Random.Range(0, 2);//0�܂���1
        speed = 1.8f;
        enemyObject = new GameObject("Enemy");
        SpriteRenderer sr = enemyObject.AddComponent<SpriteRenderer>();
        sr.sprite = enemy[r].image;//�z��enemy��r�Ԗڂ̗v�f�̉摜
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
            Debug.Log("����");
            n++;
            exit = true;
        }
        else
        {
            Debug.Log("�s����");
        }

        
    }

    void Update()
    {

        //Object������Ƃ��Ɉړ�
        if (enemyObject != null)
        {
            speed *= grad;
            enemyObject.transform.position = Vector2.MoveTowards(
                enemyObject.transform.position,
                target,
                speed
            ) ;
        }
        //�ޏ�̉��o
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
 
        //�L�[����
        if ((Vector2)enemyObject.transform.position == target)
        {
            can_ans = true;//�G�̒�~���m�F
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
