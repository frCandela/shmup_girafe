using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When the player 
public class Virus : Ship
{
    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
