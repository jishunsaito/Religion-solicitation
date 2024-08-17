using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resultbuttoncontoroller : MonoBehaviour
{
    public void restart_button_down()
    {
        FadeManager.Instance.LoadScene("Gamescene", 0.4f);
    }
    public void backstart_button()
    {
        FadeManager.Instance.LoadScene("StartScenes", 0.4f);
    }
}
