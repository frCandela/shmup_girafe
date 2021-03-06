﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {
    int numberPV = 0;
    public GameObject healthContainer;
    public GameObject healthPrefab;
    public GameObject healthBackPrefab;
    public List<Image> healthPoints;
	public Color _lowHealthColor;

    public Animator anim;

    public Image hackBar;
    public Image hackMessage;
	public Text hackPercentage;
	public GameObject outerGlow;
	public Animator inGlow;

    public Image multi;
	public Image multiMessage;
    public Image multiBefore;
    public Image multiAfter;
	public Image multiBarUp;
	public Image multiBarDown;
    public Sprite[] multiText;
	public Sprite[] multiAlert;

    [Header("Linked GameObjects :")]
    public MouseController mouseController;
    public Health health;
    public GameObject leader;
    public InputField input;
	public GameObject leaderBoard;
	public GameObject newHighScore;

    private float currentHackPower = 0.1f;
	private Animator outGlowAnim;

	private bool _isMaxCombo = false;

	void Start()
	{
		outGlowAnim = outerGlow.GetComponent<Animator> ();
		currentHackPower = 0.01f;
	}

    // Update is called once per frame
    void Update()
    {
        setMaxHealth(health.getMaxHealth());
        if (health)
            setHealthBar(health.health);
        if (mouseController)
            setHackBar(mouseController.getHackPowerRatio());
    }

    //Set the multiplierText
    public void setMulti(int value, int hack)
    {
        int array = value + hack;
        if (value == 3)
            array++;
        if (value == 4)
            array = multiText.Length - 1;
        multiBefore.enabled = true;
        if (array > 0)
            multiBefore.sprite = multiText[array - 1];
        else
            multiBefore.enabled = false;

        multi.sprite = multiText[array];
		if (array == 1 || array == 2 || array == 4)
		{
			_isMaxCombo = false;
			multi.gameObject.GetComponent<Animator> ().SetTrigger ("Flash");
			multiBarUp.gameObject.GetComponent<Animator> ().SetTrigger ("Flash");
			multiBarDown.gameObject.GetComponent<Animator> ().SetTrigger ("Flash");

			//alert middle screen
			anim.SetTrigger ("showMulti");
		} else if (array == 7 && !_isMaxCombo)
		{
			_isMaxCombo = true;

			multi.gameObject.GetComponent<Animator> ().SetTrigger ("Flash");
			multiBarUp.gameObject.GetComponent<Animator> ().SetTrigger ("Flash");
			multiBarDown.gameObject.GetComponent<Animator> ().SetTrigger ("Flash");

			//alert middle screen
			anim.SetTrigger ("showMulti");
		}			

        multiAfter.enabled = true;
        if (array < multiText.Length - 1)
            multiAfter.sprite = multiText[array + 1];
        else
            multiAfter.enabled = false;
    }

    public void setMaxHealth(int value)
    {
        if (value == numberPV)
            return;

        foreach (Transform child in healthContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
        healthPoints.Clear();

        for (int i = 0; i < value; i++)
            healthPoints.Add(Instantiate(healthPrefab, healthContainer.transform).GetComponent<Image>());

		if (value == 0)
			Instantiate (healthBackPrefab, healthContainer.transform);

        numberPV = value;
    }

    bool lowHealthShow = false;
    //Set the health bar, value must be between 0F and 1F
    public void setHealthBar(int value)
    {
        for (int i = 0; i < healthPoints.Count; ++i)
        {
            if (i < value)
                healthPoints[i].color = new Color(1, 1, 1, 1);
            else
                healthPoints[i].color = new Color(1, 1, 1, 0.5f);
        }
			
		if(value ==1) healthPoints [0].color = _lowHealthColor;
			
        if (value == 1 && !lowHealthShow)
        {
            anim.SetTrigger("showHealth");
            lowHealthShow = true;
        }
        else if (value != 1 && lowHealthShow)
        {
            anim.SetTrigger("endHealth");
            lowHealthShow = false;
        }
    }

    bool hackShow = false;

    //Set the hack bar, value must be between 0F and 1F
    public void setHackBar(float value)
    {
		SetHackPercentage (value);
        if (value >= 1f && !hackShow)
        {
            anim.SetTrigger("showHack");
            hackShow = true;
        }
		else if(value < 1f && hackShow)
        {
            anim.SetTrigger("endHack");
            hackShow = false;
        }
    }

	public IEnumerator HideHackMessage()
	{
		hackMessage.enabled = false;
		yield return new WaitForSeconds (5f);
		hackMessage.enabled = true;
	}

	public void ShowHackMessage()
	{
		hackMessage.enabled = true;
	}

	//set the percentage of the hack bar
	void SetHackPercentage(float per)
	{
		if(per != currentHackPower)
		{
			outGlowAnim.SetBool ("isGlowing", true);

			currentHackPower = Mathf.Lerp (currentHackPower, per, Time.deltaTime*7f);
			hackBar.rectTransform.offsetMax = new Vector2(-250 + currentHackPower * 260, 12);
			hackPercentage.text = Mathf.RoundToInt (currentHackPower*100f).ToString () + " %";

			//Full hack glow
			if (currentHackPower > 0.99f)
				inGlow.SetBool ("isGlowing", true);
			else 
				inGlow.SetBool ("isGlowing", false);
		}
	}

    public void showLeaderScreen()
    {
        leader.SetActive(true);
		StartCoroutine (CheckHighScore ());
		GameManager.instance.enabled = false;
    }

	IEnumerator CheckHighScore()
	{
		List<Record> leaders = new List<Record> ();
		yield return StartCoroutine (OnlineScore.GetScores (leaders, 11));
		int higherScore = leaders [0].score;
		if (GameManager.instance.getScore () > higherScore)
			newHighScore.SetActive (true);
	}

    public void sendScore()
    {
		StartCoroutine (SubmitAndMenu ());
    }

	IEnumerator SubmitAndMenu()
	{
		GameManager.instance.saveScore(11);
		yield return StartCoroutine(OnlineScore.SetScores(GameManager.instance.getScores(), input.text));
		leader.SetActive (false);
		leaderBoard.SetActive (true);
	}
}
