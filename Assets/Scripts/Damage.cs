using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

    [Range(0, 100)] public int damage = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("BOOM");
        Health health = collision.gameObject.GetComponent<Health>();
        if (health && collision.gameObject.tag != this.tag)
        {
            health.takeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
