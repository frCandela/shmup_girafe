using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [Range(0.0f, 20.0f)]
    public float speed = 1;

    public float lifetime = 3;

    void Update()
    {
        this.transform.position += this.transform.up * Time.deltaTime * speed;

        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);
    }

}
