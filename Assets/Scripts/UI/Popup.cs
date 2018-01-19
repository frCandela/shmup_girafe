using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Rigidbody2D))]
public class Popup : MonoBehaviour
{
    public float lifeTime = 2f;
    public bool alphaFadeOut = true;
    public bool scaleFadeOut = true;
    public Vector2 PopupSpeed = new Vector2(10, 100);
    

    //Components
    private Rigidbody2D rb;
    private Text text;
    private float initialScale;

    //Private attributes
    private float fadeOutTimer;

    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, lifeTime);
        
        //Set components
        rb = GetComponent<Rigidbody2D>();
        text = GetComponent<Text>();

        //Set speed
        rb.velocity = new Vector2(Random.Range(-PopupSpeed.x, PopupSpeed.x), Random.Range(0, PopupSpeed.y));

        fadeOutTimer = 0f;
        initialScale = transform.localScale.x;

        print(initialScale);
    }
	
	// Update is called once per frame
	void Update ()
    {
        fadeOutTimer += Time.deltaTime;

        //alphaFadeOut
        if (alphaFadeOut)
        {
            Color color = text.color;
            color.a = 1 - fadeOutTimer / lifeTime;
            text.color = color;
        }

        // scaleFadeOut
        if (scaleFadeOut)
        {
            float scaleValue = initialScale*(1f - fadeOutTimer / lifeTime);
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }

    }
}
