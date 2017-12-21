using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0, 100)] public int health = 100;

    public void takeDamage( int damage )
    {
        print("BOOM");
        health -= damage;
        if (health <= 0)
            Destroy(this.gameObject);
    }
}
