using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameResult : MonoBehaviour
{
    TextMeshProUGUI Score;

    int result = 123; //�X�R�A�ϐ��҂�
    void Start()
    {
        Score = GetComponent<TextMeshProUGUI>();

        Score.text = result.ToString();
    }

    void Update()
    {
        
    }
}
