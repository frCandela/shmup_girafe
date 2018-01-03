using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainBar : MonoBehaviour
{
    public Image[] combo;
    public Image hackBar;
    public Image healthBar;

    // Use this for initialization
    void Start ()
    {
        //set the healthBar
        if (healthBar)
        {
            healthBar.type = Image.Type.Filled;
            healthBar.fillMethod = Image.FillMethod.Horizontal;
            healthBar.fillOrigin = 0;
            healthBar.fillAmount = 1F;
        }

        //set the hackBar
        if (hackBar)
        {
            hackBar.type = Image.Type.Filled;
            hackBar.fillMethod = Image.FillMethod.Horizontal;
            hackBar.fillOrigin = 0;
            hackBar.fillAmount = 1F;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Set the combo images
    public void setCombo(int value)
    {
        for( int i = 0; i < combo.Length;  ++i )
        {
            if (i < value)
                combo[i].enabled = true;
            else
                combo[i].enabled = false;
        }
    }

    //Set the health bar, value must be between 0F and 1F
    public void setHealthBar( float value )
    {
        if(value >= 0F && value <= 1F)
            healthBar.fillAmount = value;
    }

    //Set the hack bar, value must be between 0F and 1F
    public void setHackBar(float value)
    {
        if (value >= 0F && value <= 1F)
            hackBar.fillAmount = value;
    }

}

[CustomEditor(typeof(MainBar))]
public class SliceEditor : Editor
{
    public int setCombo;
    public float setHealth;
    public float setHack;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MainBar myMainBar = (MainBar)target;

        //Combo
        setCombo = EditorGUILayout.IntField("combo", setCombo);
        if (GUILayout.Button("setCombo"))
            myMainBar.setCombo(setCombo);

        //Health
        setHealth = EditorGUILayout.FloatField("health", setHealth);
        if (GUILayout.Button("setHealth"))
            myMainBar.setHealthBar(setHealth);

        //Hack
        setHack = EditorGUILayout.FloatField("hack", setHack);
        if (GUILayout.Button("setHack"))
            myMainBar.setHackBar(setHack);
    }
}