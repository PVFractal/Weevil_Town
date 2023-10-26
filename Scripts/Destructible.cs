using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{

    public GameObject explosion;

    

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        //This means it has hit a bullet
        if (collision.gameObject.layer == 10)
        {
            //Making it turn to dust
            GameObject newExplo = Instantiate(explosion);
            newExplo.transform.position = transform.position;

            Destroy(gameObject);
        }
    }
}
