using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    const int TIME_BEFORE_DEATH = 300;

    int death_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate is called at regular intervals
    private void FixedUpdate()
    {
        death_timer++;
        if (death_timer > TIME_BEFORE_DEATH)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
