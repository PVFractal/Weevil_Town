using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutScript : MonoBehaviour
{

    public GameObject explosion;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If it collides with a weevil
        if (collision.gameObject.layer == 6)
        {
            //Making it turn to dust
            GameObject donutEaten = Instantiate(explosion);
            donutEaten.transform.position = transform.position;

            //Getting rid of the donut
            Destroy(gameObject);
        }
    }
}
