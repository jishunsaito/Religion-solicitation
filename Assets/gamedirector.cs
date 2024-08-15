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
        public Sprite image; // �摜
        public bool flag; //�^���l
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
    public static int n = 999;//�|��������̐�
    public Enemy[] enemy;//Enemy�^�̔z��
    private GameObject enemyObject;//�o������l
    private Vector2 start_pos;//�l�̊J�n�n�_
    private Vector2 target;//�l�̒�~�n�_
    private float speed;//�o���X�s�[�h 1.8
    private float back = -1.5f;//�ޏꂷ�鑬�x
    private float grad = 0.85f;//�X�s�[�h�̌��z 0.85
    private float countdown;//��������
    private bool can_ans = false;//�L�[�{�[�h�������邩
    private bool exit = false;//�ޏꒆ
    private bool answering = false;//�񓚒�
    private bool timeup = false;//���Ԑ؂�
    private AudioSource audiosource;
    private Animator punching;
    private Animator waving;
    public AudioClip punch_SE;
    public AudioClip waving_SE;




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
            Debug.Log("����");
            n++;
            exit = true;
            
        }
        else
        {
            Debug.Log("�s����");
            StartCoroutine(Loose());
            timeup = true;
        }
        can_ans = false;
        

        
    }
    IEnumerator Loose()
    {
        //�s�����̎��̓G�̍s��0.5�b��Ƀv���C���[�ɋ߂Â�
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


        //�L�[����
        if (enemyObject.transform.position.x < 1.5f && enemyObject.transform.position.x >= 0.0f)
        {
            can_ans = true;//�G�̒�~���m�F
            countdown -= Time.deltaTime;
            if (countdown < 0 && !timeup)
            {
                Debug.Log("���Ԑ؂�");
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
