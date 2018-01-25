using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowHack : MonoBehaviour {

    public Sprite specDPS, specTank, specFlesh;

    public Image infos;
    public Image spec;
	public GameObject HP;

	private Text amountHP;

	void Start()
	{
		amountHP = HP.GetComponent<Text> ();
	}

    // Update is called once per frame
    void Update () {
        Ship targetShip = GameManager.instance.PlayerController.getTargetHack();
        if (!targetShip)
        {
            infos.enabled = false;
            spec.enabled = false;
			HP.SetActive (false);
        }
        else
        {
            infos.enabled = true;
			HP.SetActive (true);
			amountHP.GetComponent<Text>().text = targetShip.GetComponent<Health> ().health.ToString ();
            if(targetShip.GetType() == typeof(DPSShip))
                spec.sprite = specDPS;
            else if (targetShip.GetType() == typeof(TankShip))
                spec.sprite = specTank;
            else
                spec.sprite = specFlesh;
            spec.enabled = true;
            if (targetShip.GetType() == typeof(Virus))
            {
                infos.enabled = false;
                spec.enabled = false;
				HP.SetActive (false);
            }
        }
        if (!GameManager.instance.PlayerController.hacking())
        {
            GetComponent<Image>().enabled = false;
            infos.enabled = false;
            spec.enabled = false;
			HP.SetActive (false);
        }
        else
        {
            GetComponent<Image>().enabled = true;
            GameManager.instance.PlayerController.hackPointer.GetComponent<SpriteRenderer>().enabled = false;
            Vector3 pos = Camera.main.WorldToScreenPoint(GameManager.instance.PlayerController.hackPointer.transform.position);
            transform.position = pos;
        }
    }
}