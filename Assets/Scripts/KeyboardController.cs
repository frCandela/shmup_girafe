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
        

        //Handle player actions if a pawn is possessed
        if (isPossessingPawn())
        {
            PossessedPawn.MoveHorizontal(Input.GetAxis("Horizontal"));
            PossessedPawn.MoveVertical(Input.GetAxis("Vertical"));

            if (Input.GetButton("Fire"))
                PossessedPawn.Fire();

            snapInCameraView();
        }


    }

    private void snapInCameraView()
    {
        //Calculates the width and height of the plane cutting the camera frustum
        Camera camera = GameManager.MainCamera;
        Vector3 playerPosition = PossessedPawn.transform.position;

        float cameraZ = Mathf.Abs(camera.transform.position.z - playerPosition.z);
        float heightCamera = cameraZ * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
        float widthCamera = heightCamera * Screen.width / Screen.height;

        bool vSnapped = true;
        //Vertical snap
        if (playerPosition.y < camera.transform.position.y - heightCamera)
            PossessedPawn.transform.position = new Vector3(playerPosition.x, camera.transform.position.y - heightCamera + 0.1F, playerPosition.z);
        else if (playerPosition.y > camera.transform.position.y + heightCamera)
            PossessedPawn.transform.position = new Vector3(playerPosition.x, camera.transform.position.y + heightCamera, playerPosition.z);
        else
            vSnapped = false;

        //Horizontal snap
        bool hSnapped = true;
        if (playerPosition.x < camera.transform.position.x - widthCamera)
            PossessedPawn.transform.position = new Vector3(camera.transform.position.x - widthCamera, playerPosition.y, playerPosition.z);
        else if (playerPosition.x > camera.transform.position.x + widthCamera)
            PossessedPawn.transform.position = new Vector3(camera.transform.position.x + widthCamera, playerPosition.y, playerPosition.z);
        else
            hSnapped = false;

        /*Rigidbody2D rbPawn =  PossessedPawn.GetComponent<Rigidbody2D>();
        if (rbPawn)
        {
            Vector2 newVelocity = rbPawn.velocity;
            if (vSnapped)
                newVelocity.y = 0;
            if (hSnapped)
                newVelocity.x = 0;
            rbPawn.velocity = newVelocity;
        }*/
    }
}
