using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIdirector : MonoBehaviour
{
    private gamedirector gameDirector;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI countText;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("mainsystem").GetComponent<gamedirector>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        countText = GameObject.Find("countdown").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamedirector.start_count != 0)
        {
            countText.text = gamedirector.start_count.ToString();
            countText.gameObject.SetActive(true); // カウントダウンのUIを表示
        }
        else
        {
            countText.gameObject.SetActive(false); // カウントダウンのUIを非表示
        }

        // スコアの更新
        scoreText.text = gamedirector.n.ToString() + "人がえし";
    }
}

