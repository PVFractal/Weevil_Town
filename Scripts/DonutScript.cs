using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutScript : MonoBehaviour
{

    public GameObject explosion;

    public bool isFood;

    public void die()
    {
        //Making it turn to dust
        GameObject donutEaten = Instantiate(explosion);
        donutEaten.transform.position = transform.position;

        //Getting rid of the donut
        Destroy(gameObject);
    }
}
