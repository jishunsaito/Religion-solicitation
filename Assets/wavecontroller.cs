using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wavecontroller : MonoBehaviour
{
    private Animator animetor;

    void Start()
    {
        animetor = this.gameObject.GetComponent<Animator>();
    }

    void OnXkeypressed()
    {
        GetComponent<Animator>().SetTrigger("handAnim");
        GetComponent<AudioSource>().Play();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnXkeypressed();
        }
    }
}
