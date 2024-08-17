using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class police_resultcontoroller : MonoBehaviour
{

    private float handmove = 0.1f;
    void Update()
    {
        Vector2 pos = transform.position;
        if (pos.y <= -2.2f)
        {
            pos.y += handmove;

            transform.position = pos;
        }
        if(pos.y > -2.2f && -2.1f > pos.y)
        {
            FindObjectOfType<darker_contoroller>().StartDarkening();
        }
    }
}
