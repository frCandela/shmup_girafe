using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollection : Dictionary<Ship,Ship>
{
    Ship currentShip;

    public Ship rightShipFrom(Ship startShip)
    {
        //Init variables
        Ship selectedShip = null;
        float xMin = startShip.transform.position.x;

        //Loop over the ships in the collection
        foreach (KeyValuePair<Ship, Ship> pair in this)
        {
            Ship newShip = pair.Value;

            if (newShip != startShip && newShip.transform.position.x >= xMin)
                if (!selectedShip || newShip.transform.position.x < selectedShip.transform.position.x)
                    selectedShip = newShip;
        }
        return selectedShip ? selectedShip : startShip;
    }

    public Ship leftShipFrom(Ship startShip)
    {
        //Init variables
        Ship selectedShip = null;
        float xMax = startShip.transform.position.x;

        //Loop over the ships in the collection
        foreach (KeyValuePair<Ship, Ship> pair in this)
        {
            Ship newShip = pair.Value;

            if (newShip != startShip && newShip.transform.position.x <= xMax)
                if (!selectedShip || newShip.transform.position.x > selectedShip.transform.position.x)
                    selectedShip = newShip;
        }
        return selectedShip ? selectedShip : startShip;
    }

    public Ship upperShipFrom(Ship startShip)
    {
        //Init variables
        Ship selectedShip = null;
        float yMin = startShip.transform.position.y;

        //Loop over the ships in the collection
        foreach (KeyValuePair<Ship, Ship> pair in this)
        {
            Ship newShip = pair.Value;

            if (newShip != startShip && newShip.transform.position.y >= yMin)
                if (!selectedShip || newShip.transform.position.y < selectedShip.transform.position.y)
                    selectedShip = newShip;
        }
        return selectedShip ? selectedShip : startShip;
    }

    public Ship lowerShipFrom(Ship startShip)
    {
        //Init variables
        Ship selectedShip = null;
        float yMax = startShip.transform.position.y;

        //Loop over the ships in the collection
        foreach (KeyValuePair<Ship, Ship> pair in this)
        {
            Ship newShip = pair.Value;

            if (newShip != startShip && newShip.transform.position.y <= yMax)
                if (!selectedShip || newShip.transform.position.y > selectedShip.transform.position.y)
                    selectedShip = newShip;
        }
        return selectedShip ? selectedShip : startShip;
    }
}
