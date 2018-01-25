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

    public Animator anim;

    public Image hackBar;
    public Image hackMessage;
	public Text hackPercentage;

    public Image multi;
    public Sprite[] multiText;

    [Header("Linked gamebjects :")]
    public HackSelector hackSelector;
    public MouseController mouseController;
    public Health health;

	private float currentHackPower = 0f;

    // Update is called once per frame
    void Update()
    {
        setMaxHealth(health.getMaxHealth());
        if (health)
            setHealthBar(health.health);
		if (hackSelector) 
		{
			setHackBar (hackSelector.getHackPowerRatio ());
			SetHackPercentage ();
		}
        else if (mouseController)
		{
			setHackBar (mouseController.getHackPowerRatio ());
			SetHackPercentage ();
		}
    }

    //Set the multiplierText
    public void setMulti(int value)
    {
        multi.sprite = multiText[value];
    }

    public void setSegments(int nbSegments)
    {
        
    }
    
    public void setCombo(int value)
    {
        
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
            Instantiate(healthBackPrefab, healthContainer.transform);

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
        hackBar.rectTransform.offsetMax = new Vector2(-250 + value * 260, 12);
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

	//set the percentage of the hack bar
	void SetHackPercentage()
	{
		float per = mouseController.getHackPowerRatio ();
		per *= 100f;
		per = Mathf.RoundToInt (per);
		if(per != currentHackPower)
		{
			currentHackPower = Mathf.Lerp (currentHackPower, per, Time.deltaTime*7f);
			hackPercentage.text = Mathf.RoundToInt (currentHackPower).ToString () + " %";
		}
	}
}
