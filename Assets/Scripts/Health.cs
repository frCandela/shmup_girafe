using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0, 100)] public int health = 100;

    public bool immortal = false;

    private void Start()
    {
        if (immortal)
            health = 0;
    }

    public void takeDamage( int damage )
    {
        //Imoortal objects don't take damage
        if( ! immortal )
        {
            health -= damage;
            if (health <= 0)
            {
                //Destroy the object
                Destroy(this.gameObject);
            }
        }

            
    }
}
