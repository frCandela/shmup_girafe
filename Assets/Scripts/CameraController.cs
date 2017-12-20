using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float VerticalSpeed = 0.03f;

    public int shipCount = 0;


    private BoxCollider2D cameraTrigger;

    // Use this for initialization
    void Start ()
    {
        cameraTrigger = gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        cameraTrigger.isTrigger = true;
        resizeCameraTrigger();
    }
	
    private void resizeCameraTrigger()
    {
        //Calculates the width and height of the plane cutting the camera frustum
        Camera camera = GetComponent<Camera>();

        float cameraZ = Mathf.Abs(camera.transform.position.z);
        float heightCamera = cameraZ * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
        float widthCamera = heightCamera * Screen.width / Screen.height;

        cameraTrigger.size = new Vector2(2*widthCamera, 2*heightCamera);
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += VerticalSpeed;
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if(ship)
            shipCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if (ship)
            shipCount--;
    }

}
