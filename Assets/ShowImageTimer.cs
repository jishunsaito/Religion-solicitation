using UnityEngine;
using UnityEngine.UI;

public class ShowImageTimer : MonoBehaviour
{
    public GameTimer m_gameTimer;
    public Image m_imgTarget;

    public gamedirector gameDirector; // GameDirectorスクリプトの参照を追加
    public bool m_bIncremental = true;

    void Start()
    {
        // レベルに基づいてタイマーを設定
        float currentLevelTime = gameDirector.mode[gameDirector.level_idx].ans_time;
        m_gameTimer.SetMaxTime(currentLevelTime);
        m_gameTimer.OnStart(); // タイマーを開始
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
