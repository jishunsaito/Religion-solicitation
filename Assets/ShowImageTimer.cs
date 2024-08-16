using UnityEngine;
using UnityEngine.UI;

public class ShowImageTimer : MonoBehaviour
{
    public GameTimer m_gameTimer;
    public Image m_imgTarget;

    public gamedirector gameDirector; // GameDirector�X�N���v�g�̎Q�Ƃ�ǉ�
    public bool m_bIncremental = true;

    void Start()
    {
        // ���x���Ɋ�Â��ă^�C�}�[��ݒ�
        float currentLevelTime = gameDirector.mode[gameDirector.level_idx].ans_time;
        m_gameTimer.SetMaxTime(currentLevelTime);
        m_gameTimer.OnStart(); // �^�C�}�[���J�n
    }

    void Update()
    {
        if (m_bIncremental)
        {
            m_imgTarget.fillAmount = m_gameTimer.CurrentTime / m_gameTimer.m_fMaxTime;
        }
        else
        {
            m_imgTarget.fillAmount = (m_gameTimer.m_fMaxTime - m_gameTimer.CurrentTime) / m_gameTimer.m_fMaxTime;
        }
    }
}
