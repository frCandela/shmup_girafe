using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When the player 
public class Virus : Ship
{
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
