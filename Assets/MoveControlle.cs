using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private float handmove = 0.1f;

    private void Start()
    {
        
    }
    
    void Update()
    {
        Vector2 pos = transform.position;
        if (pos.y < -2.2f)
        {
            pos.y += handmove;

            transform.position = pos;
        }
    }
}
