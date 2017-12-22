using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : Controller
{
    bool isHack = false;

    private void Update()
    {
        if (Input.GetButton("Fire"))
        {
            if(isHack)
            {
                isHack = false;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero, Mathf.Infinity, 256, -Mathf.Infinity);
                Debug.Log(hit);
                Debug.Log(hit.collider);
                if (hit && hit.collider) {
                    Ship target = hit.collider.gameObject.GetComponent<Ship>();
                    target.tag = PossessedPawn.tag;
                    target.UnPossess();
                    Possess(target);
                    target.transform.rotation = Quaternion.Euler(0F, 0F, 0F);
                }
                TimeManager.resetSlowMotion();
                GameManager.instance.ToogleHackEffect();
            }
            else
                PossessedPawn.Fire();
        }
        
        if (Input.GetButton("Hack") && !isHack) {
            isHack = true;
            GameManager.instance.ToogleHackEffect();
            TimeManager.doSlowMotion(3, 0.05f);
        }
    }
    
	void FixedUpdate ()
    {
        if (isPossessingPawn())
        {
            PossessedPawn.transform.position = Vector3.MoveTowards(PossessedPawn.transform.position, transform.position, ((Ship)PossessedPawn).Speed * Time.fixedDeltaTime);
        }
    }

    void OnGUI()
    {
        Camera c = Camera.main;
        Event e = Event.current;
        Vector2 mousePos = new Vector2(e.mousePosition.x, c.pixelHeight - e.mousePosition.y);
        Vector3 p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));

        transform.position = new Vector3(p.x, p.y, 0);

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + c.pixelWidth + ":" + c.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + p.ToString("F3"));
        GUILayout.EndArea();
    }

}
