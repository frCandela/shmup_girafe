using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0, 100)] public int health = 100;
    public bool immortal = false;

    private int maxHealth;

    private void Start()
    {
        if (immortal)
        {
            health = 0;
            maxHealth = 0;
        }
        maxHealth = health;
    }

    //Returns the health scaled between 0 and 1
    public float getHealthRatio()
    {
        if (maxHealth != 0F)
            return (float)health / maxHealth;
        else
            return 0;
    }

    public void takeDamage( int damage )
    {
        //Immortal objects don't take damage
        if( ! immortal )
        {
            health -= damage;
            if (health <= 0)
                Destroy(this.gameObject);
        }    
    }
}
