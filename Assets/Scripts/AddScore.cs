using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour {

    public void AddPoints(int points)
    {
        GameManager.score += points;
    }

}
