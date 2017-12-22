using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float VerticalSpeed = 0.5f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += VerticalSpeed * Time.fixedDeltaTime;
        transform.position = newPosition;

    }
}
