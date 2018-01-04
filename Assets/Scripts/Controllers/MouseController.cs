using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : Controller
{
    [Header("GameObjects")]
    public Ship VirusShipPrefab;
    private Ship virusShip;

    [Header("Hack parameters:")]
    public float HackDuration = 3F;
    public bool infiniteDuration = false;
    [Range(0F, 1F)] public float TimeScaleFactor = 0.1F;
    [Range(0F, 20F)] public float hackRefillSpeed = 1F;

    //Private hack parameters
    private float hackPower;
    private float maxHackPower = 100F;
    private float minHackPower = 100F;
    private bool isHacking = false;


    private void Start()
    {
        //ui initialisation
        GameManager.MainBar.health = GetComponent<Health>();

        //Hack parameters
        hackPower = 0;
        isHacking = false;
        if (infiniteDuration)
            HackDuration = float.MaxValue;

        //Virus ship
        virusShip = Instantiate(VirusShipPrefab, transform.position, transform.rotation);
        virusShip.enabled = false;
    }

    private void Update()
    {
        //Refill the hack bar
        hackPower += hackRefillSpeed * Time.deltaTime;
        if (hackPower > maxHackPower)
            hackPower = maxHackPower;

        //If the ship is destroyed, control the virus ship
        if (!isPossessingPawn())
        {
            GameManager.MainBar.health = virusShip.GetComponent<Health>();
            virusShip.enabled = true;
            Possess(virusShip);
        }

        if (Input.GetButton("Fire"))
        {
            if (isHacking)
            {
                isHacking = false;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -GameManager.MainCameraController.transform.position.z)), Vector2.zero, Mathf.Infinity, 256, -Mathf.Infinity);
                if (hit && hit.collider)
                {
                    Ship target = hit.collider.gameObject.GetComponent<Ship>();

                    if(target.hackCost <= hackPower)
                    {
                        //misc
                        hackPower -= target.hackCost;
                        if (hackPower < 0F)
                            hackPower = 0F;
                        GameManager.MainBar.health = target.GetComponent<Health>();

                        //Destroy the old pawn
                        Health oldHealth = this.PossessedPawn.GetComponent<Health>();
                        if (!oldHealth || !oldHealth.immortal)//Don't destroy immortal objects 
                            Destroy(this.PossessedPawn.gameObject);

                        //Possess the new ship
                        target.gameObject.tag = this.gameObject.tag;
                        this.Possess(target);
                        target.transform.rotation = Quaternion.Euler(0F, 0F, 0F);
                    }
                }

                TimeManager.resetSlowMotion();
                GameManager.instance.setHackEffect(false);
            }
            else
                PossessedPawn.Fire();
        }

        if (Input.GetButton("Hack") && !isHacking && hackPower >= minHackPower)
        {
            isHacking = true;
            GameManager.instance.ToogleHackEffect();
            TimeManager.doSlowMotion(3, 0.05f);
        }
    }

    public float getHackPowerRatio() { return hackPower / maxHackPower; }

    void FixedUpdate()
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
        Vector3 p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, - GameManager.MainCameraController.transform.position.z));

        transform.position = new Vector3(p.x, p.y, 0);

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + c.pixelWidth + ":" + c.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + p.ToString("F3"));
        GUILayout.EndArea();
    }

}
