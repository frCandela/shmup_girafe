using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent( typeof(SpriteRenderer))]
//[RequireComponent(typeof(Animator))]
public class ExplosionPopup : MonoBehaviour
{
    Animator animator;
    Animation anim;
    SpriteRenderer animRenderer;

    // Use this for initialization
    void Start ()
    {
        /*animator = GetComponent<Animator>();
        animRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animation>();

        animRenderer.enabled = false;

        if (anim)
            print("test");
        else
            print("nope");*/
    }
	
	// Update is called once per frame
	void Update ()
    {
		if( Input.GetKeyDown( KeyCode.Space) )
        {
            animRenderer.enabled = true;
            anim.Play();
        }

        //if( anim.play)
    }
}
