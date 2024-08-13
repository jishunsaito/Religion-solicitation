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

    public Enemy[] enemy;//Enemy�^�̔z��
    private GameObject enemyObject;//�o������l
    private Vector2 start_pos;//�l�̊J�n�n�_
    private Vector2 target;//�l�̒�~�n�_
    private float speed;//�o���X�s�[�h 1.8
    private float grad = 0.85f;//�X�s�[�h�̌��z 0.85
    private bool can_ans = false;//�L�[�{�[�h�������邩
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
    }

    void Start()
    {
        start_pos = new Vector2(10.0f, 2.0f);
        target = new Vector2(0.0f, 2.0f);
        Application.targetFrameRate = 60;
        Show_enemy();   
    }

    void OnZkeypressed()
    {
        Check(true);
    }

    void OnXkeypressed()
    {
        Check(false);
    }

    void Check(bool ans)
    {
        if (ans == enemy[r].flag)
        {
            Debug.Log("����");
        }
        else
        {
            Debug.Log("�s����");
        }

        Destroy(enemyObject);
        Show_enemy();
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
        if ((Vector2)enemyObject.transform.position == target)
        {
            can_ans = true;//�G�̒�~���m�F
        }

        if (can_ans)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnZkeypressed();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                OnXkeypressed();
            }
        }
    }
}
