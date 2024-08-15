using UnityEngine;
using UnityEngine.UI;  // UIコンポーネントを使用するために必要です

public class PlaySoundOnAnimationEndWithStartButton : MonoBehaviour
{
    public AudioSource audioSource;    // 音声を再生するためのAudioSource
    public AudioClip soundClip;        // 再生する効果音のAudioClip
    public Animator animator;          // アニメーションを制御するAnimator
    public Button startButton;         // スタートボタン

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
            // アニメーションの再生が終了しているかを確認
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("YourAnimationStateName") &&
                !animator.IsInTransition(0))
            {
                // アニメーションが終了しているとき
                if (audioSource != null && soundClip != null && !audioSource.isPlaying)
                {
                    // 効果音を再生
                    audioSource.PlayOneShot(soundClip);

                    // 効果音が再生された後、アニメーションの終了フラグをリセット
                    animationStarted = false;
                }
            }
        }
    }

    private void StartAnimation()
    {
        if (animator != null)
        {
            // アニメーションを開始するトリガー
            animator.SetTrigger("StartAnimationTrigger");
            animationStarted = true;
        }
    }
}

