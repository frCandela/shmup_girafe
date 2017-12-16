using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//The player controller that can possess pawns
public class PlayerContoller : Controller
{

    // Use this for initialization
    void Start ()
    {
		
	}
	

	void FixedUpdate ()
    {
        snapInCameraView();

        //Handle player actions if a pawn is possessed
        if (isPossessingPawn())
        {
            PossessedPawn.MoveHorizontal(Input.GetAxis("Horizontal"));
            PossessedPawn.MoveVertical(Input.GetAxis("Vertical"));

            if (Input.GetButton("Fire1"))
                PossessedPawn.Fire();

            
        }


	}

    private void snapInCameraView()
    {
        Camera camera = GameManager.MainCamera;
        float vSize =  camera.orthographicSize;
        float hSize = vSize * Screen.width / Screen.height;
        Vector2 playerPosition = PossessedPawn.transform.position;

        Vector2 diff = playerPosition - new Vector2(camera.transform.position.x, camera.transform.position.y);

        //if the player is out of the camera frame
        if (diff.x  > hSize )
            PossessedPawn.transform.position = new Vector2(camera.transform.position.x + hSize, playerPosition.y);
        else if(diff.x < - hSize)
            PossessedPawn.transform.position = new Vector2(camera.transform.position.x - hSize, playerPosition.y);
        if(diff.y > vSize)
            PossessedPawn.transform.position = new Vector2(playerPosition.x, camera.transform.position.y + vSize);
        else if (diff.y < - vSize)
            PossessedPawn.transform.position = new Vector2(playerPosition.x, camera.transform.position.y - vSize);
    }
}
