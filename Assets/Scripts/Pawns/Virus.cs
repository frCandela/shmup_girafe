using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Virus : Ship
{
    private  float glitchTimer;
    private float glitchDelta;

    private void Start()
    {
        glitchTimer = 0F;
        glitchDelta = 0F;

        transform.position = GameManager.instance.getMouseWorldPosition();
    }

    protected override void Update()
    {
        base.Update();

        if (!isPossessed())
            this.enabled = false;

        //Glitch animation
        glitchTimer += Time.deltaTime;
        if (glitchTimer >= glitchDelta)
        {
            anim.SetTrigger("glitch");
            glitchTimer = 0F;
            glitchDelta = Random.Range(0, 3f);
        }
    }



    //Makes the ship shoot ! 
    public override void Fire(Quaternion angle)
    {
    }

    private void OnEnable()
    {
        //transform.position = GameManager.instance.getMouseWorldPosition();
        GetComponent<SpriteRenderer>().enabled = true;
        foreach(Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = false;
        GetComponent<Blink>().StopBlink();
    }

    public override void takeDamage(int damage)
    {
        Blink blink = GetComponent<Blink>();
        if (blink)
            blink.StartBlink();
        if (controller)
            controller.onTakeDamage.Invoke();
    }

    public override void Destroy()
    {
    }
}
