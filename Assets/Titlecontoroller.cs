using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Titlecontroller : MonoBehaviour
{

    private Animator animetor;

    void Start()
    {
        animetor = GameObject.Find("Title").GetComponent<Animator>();
    }
    public void Startbuttondown()
    {
        animetor.SetTrigger("TitleAnim");
        StartCoroutine(change_scene());
    }
    IEnumerator change_scene()
    {
        yield return new WaitForSeconds(0.5f);
        FadeManager.Instance.LoadScene("Gamescene", 0.7f);
    }
}
