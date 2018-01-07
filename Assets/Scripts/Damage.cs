using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour {

    [Range(0, 100)] public int damage = 100;
    [Range(0, 10)] public float stunDuration = 0F;

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

            Ship ship = collision.gameObject.GetComponent<Ship>();
            if (ship)
            {
                ship.takeDamage(damage);

                //Stun
                if(stunDuration > 0 )
                    ship.Stun(stunDuration);
            }
        }
        if (collision.gameObject.tag == "Killer") {
            Destroy(this.gameObject);
        }
    }
}
