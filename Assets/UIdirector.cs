using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIdirector : MonoBehaviour
{
    private gamedirector gameDirector;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI countText;
    private float span = 1.0f;
    private float delta = 0;
    private bool showtext = false;
    private int p_score = -1;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("mainsystem").GetComponent<gamedirector>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        countText = GameObject.Find("countdown").GetComponent<TextMeshProUGUI>();
        scoreText.gameObject.SetActive(false);
    }
    IEnumerator Waited()
    {
        countText.text = "START";
        yield return new WaitForSeconds(0.4f);
        countText.gameObject.SetActive(false);
        gamedirector.start_count = -1;

    }

    // Update is called once per frame
    void Update()
    {
        if (gamedirector.start_count > 0)
        {
            countText.text = gamedirector.start_count.ToString();
            countText.gameObject.SetActive(true); // カウントダウンのUIを表示
        }
        else if(gamedirector.start_count == 0)
        {
            StartCoroutine(Waited());
        }

        if(gamedirector.n % 5 == 0 && gamedirector.n != 0 && !showtext && gamedirector.n != p_score)
        {
            delta = 0;
            showtext = true;
            p_score = gamedirector.n;
            scoreText.gameObject.SetActive(true); // テキストを表示
            scoreText.text = gamedirector.n.ToString() + "人がえし";
        }
        if(showtext)
        {
            delta += Time.deltaTime;
            if(delta > span)
            {
                showtext = false;
                scoreText.gameObject.SetActive(false);
            }
        }
        
    }
}

