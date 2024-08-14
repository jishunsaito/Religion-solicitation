using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchcontoroller : MonoBehaviour
{
    private Animator animetor;
    
    // Start is called before the first frame update
    void Start()
    {
        animetor = this.gameObject.GetComponent<Animator>();

    }

    void OnZkeypressed()
    {
        GetComponent<Animator>().SetTrigger("punchAnim");
        GetComponent<AudioSource>().Play();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnZkeypressed();
        }
    }
}
