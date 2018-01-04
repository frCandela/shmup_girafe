using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When the player 
public class VirusShip : Ship
{
    private void Update()
    {
         if ( ! isPossessed() )
        {
            this.enabled = false;
        }
            
    }

    private void OnEnable()
    {
        //Set the VirusShip at the camera position
        Vector3 newPosition = GameManager.MainCameraController.transform.position;
        newPosition.z = 0;
        transform.position = newPosition;


        GetComponent<SpriteRenderer>().enabled = true;
        foreach(Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = true;
        
    }

    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (Collider2D collider in GetComponents<Collider2D>())
            collider.enabled = false;
    }
}
