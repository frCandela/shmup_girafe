using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour {

    [Range(0, 100)] public int damage = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != this.tag)
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health)
            {
                health.takeDamage(damage);
                Destroy(this.gameObject);
            }
            Pawn pawn = collision.gameObject.GetComponent<Pawn>();
            if (pawn)
            {
                pawn.takeDamage(damage);
            }
        }


    }
}
