using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float m_fTimer;
    public float CurrentTime { get { return m_fTimer; } }

    public bool m_bActive = false;

    // êßå¿éûä‘Çí«â¡
    public float m_fMaxTime;

    private void Update()
    {
        if (m_bActive)
        {
            m_fTimer += Time.deltaTime;
        }
    }

    public void OnStart()
    {
        m_bActive = true;
    }

    public void OnStop()
    {
        m_bActive = false;
    }

    public void OnReset()
    {
        m_fTimer = 0f;
    }

    public void SetMaxTime(float maxTime)
    {
        m_fMaxTime = maxTime;
    }
}
