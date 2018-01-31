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
    public float offsetDestroyTunnel = 0;
    GameObject lastTunnel, toDelete;

    private BoxCollider2D cameraTrigger;
    public float trauma;
    public float reduceTrauma = 0.2f;
    public float maxTrauma = 0.5f;
    public float angle = -45;

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
        trauma -= Time.deltaTime * reduceTrauma;
        trauma = Mathf.Clamp(trauma, 0, 1);

        float shake = trauma * trauma * trauma;

        transform.rotation = Quaternion.Euler(new Vector3(
            angle + shake * (Mathf.PerlinNoise(Time.realtimeSinceStartup * 5, 0) - 0.5f) * 15,
            shake * (Mathf.PerlinNoise(Time.realtimeSinceStartup * 5, 1) - 0.5f) * 15,
            shake * (Mathf.PerlinNoise(Time.realtimeSinceStartup * 5, 2) - 0.5f) * 15));

        //Creates a new tunnel after the last one?
        if (transform.position.y > lastTunnel.transform.position.y + offsetDestroyTunnel) {
            if (toDelete)
                Destroy(toDelete);
            toDelete = lastTunnel;
            lastTunnel = Instantiate(tunnelPrefab, lastTunnel.transform.position + new Vector3(0, 39.37f, 0), lastTunnel.transform.rotation);
        }
    }

    public void Shake(float amount) {
		if(trauma < maxTrauma)trauma += amount;
    }

    private void resizeCameraTrigger()
    {
        Camera camera = GetComponent<Camera>();

        //Calculates the width and height of the plane cutting the camera frustum
        float cameraZ = Mathf.Abs(camera.transform.position.z);
        float heightCamera = cameraZ * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
        float widthCamera = heightCamera * Screen.width / Screen.height;

        //Set the size of the camera trigger 
        cameraTrigger.size = new Vector2(2*widthCamera, 5*heightCamera);

        cameraTrigger.offset = new Vector2(0, 2f*heightCamera);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += VerticalSpeed * Time.fixedDeltaTime;
        transform.position = newPosition;
    }

    //Object entering the camera view
    private void OnTriggerEnter2D(Collider2D collision)
    {
		print ("collide");
        //Adds the ship to the shipsInCameraView dictionary
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if(ship)
        {
            shipsInCameraView[ship] = ship;
        }
            

        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet)
            bullet.notInCameraView = false;
    }

    //Object leaving the camera view
    private void OnTriggerExit2D(Collider2D collision)
    {
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if (ship)
            shipsInCameraView.Remove(ship);

        if (collision.gameObject.GetComponent<Bullet>())
            Destroy(collision.gameObject);
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
