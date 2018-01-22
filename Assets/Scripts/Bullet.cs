using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

    [Range(0.0f, 20.0f)]
    public float speed = 1;

    public float lifetime = 3;
    public bool notInCameraView;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0F;

        notInCameraView = true;

        Invoke("destroyIfNotInCameraView", 0.1F);
    }

    //Destroy the bullet if it's not in the camera view
    void destroyIfNotInCameraView()
    {
        if (notInCameraView)
            Destroy(this.gameObject);
    }

    void Update()
    {
		this.transform.position += this.transform.up * Time.deltaTime * speed;

        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);
    }


}
