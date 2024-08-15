using UnityEngine;
using UnityEngine.UI;  // UI�R���|�[�l���g���g�p���邽�߂ɕK�v�ł�

public class PlaySoundOnAnimationEndWithStartButton : MonoBehaviour
{
    public AudioSource audioSource;    // �������Đ����邽�߂�AudioSource
    public AudioClip soundClip;        // �Đ�������ʉ���AudioClip
    public Animator animator;          // �A�j���[�V�����𐧌䂷��Animator
    public Button startButton;         // �X�^�[�g�{�^��

    private bool animationStarted = false;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartAnimation);
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (animationStarted)
        {
            // �A�j���[�V�����̍Đ����I�����Ă��邩���m�F
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("YourAnimationStateName") &&
                !animator.IsInTransition(0))
            {
                // �A�j���[�V�������I�����Ă���Ƃ�
                if (audioSource != null && soundClip != null && !audioSource.isPlaying)
                {
                    // ���ʉ����Đ�
                    audioSource.PlayOneShot(soundClip);

                    // ���ʉ����Đ����ꂽ��A�A�j���[�V�����̏I���t���O�����Z�b�g
                    animationStarted = false;
                }
            }
        }
    }

    private void StartAnimation()
    {
        if (animator != null)
        {
            // �A�j���[�V�������J�n����g���K�[
            animator.SetTrigger("StartAnimationTrigger");
            animationStarted = true;
        }
    }
}

