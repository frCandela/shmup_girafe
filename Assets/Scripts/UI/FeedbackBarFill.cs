using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackBarFill : MonoBehaviour 
{
	[SerializeField] private Vector3 target;
	private float speed;
	public bool master = true;

	void Start()
	{
		target = GameManager.instance.barPosition;
		speed = Random.Range (20f, 25f);
		transform.localScale = new Vector3 (Random.Range (0.2f, 0.6f), Random.Range (0.2f, 0.6f), Random.Range (0.2f, 0.6f));

		if(master)
		{
			Vector3 spawnPosition = transform.position;
			StartCoroutine (Spawn (spawnPosition));
		}
	}

	IEnumerator Spawn(Vector3 spawnPosition)
	{
		int i = 0;
		while(i < 4)
		{
			GameObject fb = Instantiate (GameManager.instance.hackFillerPrefab, spawnPosition, Quaternion.identity);
			fb.GetComponent<FeedbackBarFill> ().master = false;
			yield return new WaitForSeconds (0.2f);
		}

	}
	void Update()
	{
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
			if (transform.position == target)
				Destroy (gameObject);
	}
}
