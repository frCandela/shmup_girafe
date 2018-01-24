using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = Camera.main.WorldToScreenPoint(GameManager.instance.PlayerController.PossessedPawn.transform.position);
        GetComponent<RectTransform>().sizeDelta = new Vector2((pos - transform.position).magnitude * 1920 / Screen.width, 3);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(pos.y - transform.position.y, pos.x - transform.position.x) * Mathf.Rad2Deg);
    }
}
