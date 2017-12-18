using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//The player controller that can possess pawns
public class KeyboardController : Controller
{

    // Use this for initialization
    void Start()
    {

    }


    void FixedUpdate()
    {
        snapInCameraView();

        //Handle player actions if a pawn is possessed
        if (isPossessingPawn())
        {
            PossessedPawn.MoveHorizontal(Input.GetAxis("Horizontal"));
            PossessedPawn.MoveVertical(Input.GetAxis("Vertical"));

            if (Input.GetButton("Fire"))
                PossessedPawn.Fire();


        }


    }

    private void snapInCameraView()
    {
        Camera camera = GameManager.MainCamera;
        Vector3 playerPosition = PossessedPawn.transform.position;

        float cameraZ = Mathf.Abs(camera.transform.position.z - playerPosition.z);
        float heightCamera = cameraZ /* Mathf.Rad2Deg*/ * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
        float widthCamera = heightCamera * Screen.width / Screen.height;

        //Vertical snap
        if (playerPosition.y < camera.transform.position.y - heightCamera)
            PossessedPawn.transform.position = new Vector3(playerPosition.x, camera.transform.position.y - heightCamera, playerPosition.z);
        else if (playerPosition.y > camera.transform.position.y + heightCamera)
            PossessedPawn.transform.position = new Vector3(playerPosition.x, camera.transform.position.y + heightCamera, playerPosition.z);
        //Horizontal snap
        if (playerPosition.x < camera.transform.position.x - widthCamera)
            PossessedPawn.transform.position = new Vector3(camera.transform.position.x - widthCamera, playerPosition.y, playerPosition.z);
        else if (playerPosition.x > camera.transform.position.x + widthCamera)
            PossessedPawn.transform.position = new Vector3(camera.transform.position.x + widthCamera, playerPosition.y, playerPosition.z);
    }
}
