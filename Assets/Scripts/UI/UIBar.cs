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

    public Image hackBar;
    public Image hackMessage;

    public Image multi;
    public Sprite[] multiText;

    [Header("Linked gamebjects :")]
    public HackSelector hackSelector;
    public MouseController mouseController;
    public Health health;

    // Update is called once per frame
    void Update()
    {
        setMaxHealth(health.getMaxHealth());
        if (health)
            setHealthBar(health.health);
        if (hackSelector)
            setHackBar(hackSelector.getHackPowerRatio());
        else if (mouseController)
            setHackBar(mouseController.getHackPowerRatio());
    }

    //Set the multiplierText
    public void setMulti(int value)
    {
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
    }

    //Set the hack bar, value must be between 0F and 1F
    public void setHackBar(float value)
    {
        hackBar.rectTransform.offsetMax = new Vector2(-250 + value * 260, 12);
        if(value >= 1f)
            hackMessage.enabled = true;
        else
            hackMessage.enabled = false;
    }
}
