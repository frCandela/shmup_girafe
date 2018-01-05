using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float VerticalSpeed = 1f;
    public ShipCollection shipsInCameraView;
    public GameObject tunnelPrefab;
    GameObject lastTunnel, toDelete;

    private BoxCollider2D cameraTrigger;

    // Use this for initialization
    void Start ()
    {
        cameraTrigger = gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        cameraTrigger.isTrigger = true;
        resizeCameraTrigger();

        shipsInCameraView = new ShipCollection();

        lastTunnel = Instantiate(tunnelPrefab, new Vector3(0, 0, 0), new Quaternion());
    }

    private void Update()
    {
        //Creates a new tunnel after the last one?
        if(transform.position.y > lastTunnel.transform.position.y + 10) {
            if (toDelete)
                Destroy(toDelete);
            toDelete = lastTunnel;
            lastTunnel = Instantiate(tunnelPrefab, lastTunnel.transform.position + new Vector3(0, 3.2F*32.1f, 0), lastTunnel.transform.rotation);
        }
    }

    private void resizeCameraTrigger()
    {
        Camera camera = GetComponent<Camera>();

        //Calculates the width and height of the plane cutting the camera frustum
        float cameraZ = Mathf.Abs(camera.transform.position.z);
        float heightCamera = cameraZ * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
        float widthCamera = heightCamera * Screen.width / Screen.height;

        //Set the size of the camera trigger 
        cameraTrigger.size = new Vector2(2*widthCamera, 2*heightCamera);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += VerticalSpeed * Time.fixedDeltaTime;
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Adds the ship to the shipsInCameraView dictionary
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if(ship)
        {
            shipsInCameraView[ship] = ship;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if (ship)
        {
            shipsInCameraView.Remove(ship);
        }
    }

    public void snapInCameraView(Pawn target)
    {
        //Calculates the width and height of the plane cutting the camera frustum
        Camera camera = GetComponent<Camera>();
        Vector3 targetPosition = target.transform.position;

        float cameraZ = Mathf.Abs(camera.transform.position.z);
        float heightCamera = cameraZ * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
        float widthCamera = heightCamera * Screen.width / Screen.height;

        //Vertical snap
        if (targetPosition.y < camera.transform.position.y - heightCamera)
            targetPosition.y =camera.transform.position.y - heightCamera + 0.001F;
        else if (targetPosition.y > camera.transform.position.y + heightCamera)
            targetPosition.y = camera.transform.position.y + heightCamera;

        //Horizontal snap
        if (targetPosition.x < camera.transform.position.x - widthCamera)
            targetPosition.x = camera.transform.position.x - widthCamera;
        else if (targetPosition.x > camera.transform.position.x + widthCamera)
            targetPosition.x = camera.transform.position.x + widthCamera;

        target.transform.position = targetPosition;
    }
}
