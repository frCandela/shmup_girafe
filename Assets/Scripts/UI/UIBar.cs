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
	public GameObject outerGlow;
	public Animator inGlow;

    public Image multi;
    public Image multiBefore;
    public Image multiAfter;
    public Sprite[] multiText;

    [Header("Linked gamebjects :")]
    public MouseController mouseController;
    public Health health;

	private float currentHackPower = 0f;
	private Image hackGlow;
	private Animator outGlowAnim;

	void Start()
	{
		outGlowAnim = outerGlow.GetComponent<Animator> ();
		hackGlow = outerGlow.GetComponent<Image> ();
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
        multiBefore.enabled = true;
        if (array > 0)
            multiBefore.sprite = multiText[array - 1];
        else
            multiBefore.enabled = false;

        multi.sprite = multiText[array];

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
}
