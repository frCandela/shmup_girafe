using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class MainBar : MonoBehaviour
{
    [Header("Images :")]
    public Image[] combo;
    public Image[] healthPoints;
    public Image hackBar;
    public Image healthPoint;

    [Header("Linked gamebjects :")]
    public HackSelector hackSelector;
    public MouseController mouseController;
    public Health health;

    // Use this for initialization
    void Start ()
    {
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
        if (health)
            setHealthBar(health.health);
        if (hackSelector)
            setHackBar(hackSelector.getHackPowerRatio());
        else if (mouseController)
            setHackBar(mouseController.getHackPowerRatio());
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
    public void setHealthBar(int value)
    {
        for (int i = 0; i < healthPoints.Length; ++i)
        {
            if (i < value)
                healthPoints[i].enabled = true;
            else
                healthPoints[i].enabled = false;
        }
    }

    //Set the hack bar, value must be between 0F and 1F
    public void setHackBar(float value)
    {
        if (value >= 0F && value <= 1F && value != hackBar.fillAmount)
            hackBar.fillAmount = value;
    }

}
#if UNITY_EDITOR
//Custom editor of the mainbar for debug only
[CustomEditor(typeof(MainBar))]
public class SliceEditor : Editor
{
    public int setCombo;
    public int setHealth;
    public float setHack;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MainBar myMainBar = (MainBar)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Editor :", EditorStyles.boldLabel);

        //Combo
        setCombo = EditorGUILayout.IntField("combo", setCombo);
        if (GUILayout.Button("setCombo"))
            myMainBar.setCombo(setCombo);
        
        //Health
        setHealth =(int) EditorGUILayout.Slider("setHealth", setHealth, 0,1);
        if (GUILayout.Button("setHealth"))
            myMainBar.setHealthBar(setHealth);

        //Hack
        setHack = EditorGUILayout.Slider("setHack", setHack, 0, 1);
        if (GUILayout.Button("setHack"))
            myMainBar.setHackBar(setHack);
    }
}
#endif