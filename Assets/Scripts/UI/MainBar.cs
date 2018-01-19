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
    public Image[] combo2;
    public Image[] combo3;
    public Image[] combo4;
    public Image[] combo5;
    public Image[] selectedCombo;

    public Image[] healthPoints;
    public Image hackBar;
    public Image healthPoint;

    [Header("Linked gamebjects :")]
    public HackSelector hackSelector;
    public MouseController mouseController;
    public Health health;
    public Text multi;

    // Use this for initialization
    void Start ()
    {
        selectedCombo = combo2;

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

    //Set the multiplierText
    public void setMulti(int value)
    {
        multi.text = "x" + value;
    }

    public void setSegments( int nbSegments)
    {
        for (int i = 0; i < selectedCombo.Length; ++i)
            selectedCombo[i].enabled = false;

        if (nbSegments == 2)
            selectedCombo = combo2;
        if (nbSegments == 3)
            selectedCombo = combo3;
        if (nbSegments == 4)
            selectedCombo = combo4;
        if (nbSegments == 5)
            selectedCombo = combo5;
    }

    //Set the combo images
    public void setCombo( int value)
    {
        for( int i = 0; i < selectedCombo.Length;  ++i )
        {
            if (i < value)
                selectedCombo[i].enabled = true;
            else
                selectedCombo[i].enabled = false;
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
    public int setSegments;
    public int setCombo;
    public int setHealth;
    public float setHack;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MainBar myMainBar = (MainBar)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Editor :", EditorStyles.boldLabel);

        //Multi
        setSegments = EditorGUILayout.IntField("setSegments", setSegments);
        if (GUILayout.Button("setSegments"))
            myMainBar.setSegments(setSegments);

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