using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : Controller {

	void FixedUpdate () {

        if (isPossessingPawn()) {
            PossessedPawn.transform.position = Vector3.MoveTowards(PossessedPawn.transform.position, transform.position, ((Ship)PossessedPawn).Speed * Time.fixedDeltaTime);
            //Vector3 dir = transform.position - PossessedPawn.transform.position;
            //dir.z = 0;
            //if (dir.magnitude > 0.2f)
            //    dir.Normalize();

            //Debug.Log(dir);

            //PossessedPawn.MoveHorizontal(dir.x);
            //PossessedPawn.MoveVertical(dir.y);

            if (Input.GetButton("Fire"))
                PossessedPawn.Fire();
        }

    }

    void OnGUI() {
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
