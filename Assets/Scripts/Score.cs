using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

	public void AddScore(int point) {
        GameManager.instance.addScore(point);
    }

}
