using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Enemy")) // hit by laser
        {
            Destroy(collision.gameObject); 
        }
    }
}
