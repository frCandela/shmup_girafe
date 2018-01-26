using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class AttackType : ScriptableObject
{
    public float rate = 1;
    public List<Burst> bursts = new List<Burst>();
}

[System.Serializable]
public class Burst
{
    public int amount = 1;
    public float spread = 90;
    public float angle = 0;
    public GameObject prefab;

    public bool Attack(GameObject shooter, Transform origin)
    {
        bool attacked = false;

        if (amount == 1) {
            GameObject bullet = Object.Instantiate(prefab, origin.position, Quaternion.Euler(0, 0, origin.rotation.eulerAngles.z - angle));
            Damage bulletDamage = bullet.GetComponent<Damage>();
            if (bulletDamage) //Ignore damage tag
                bulletDamage.tag = shooter.tag;
            return true;
        }

        if (spread == 360)
            amount++;
        
        for (int i = 0; i < amount; i++)
        {
            GameObject bullet = Object.Instantiate(prefab, origin.position, Quaternion.Euler(0, 0, origin.rotation.eulerAngles.z - angle - spread / 2f + i * spread / Mathf.Max(1, amount - 1)));
            Damage bulletDamage = bullet.GetComponent<Damage>();

            //Objects with shooter.tag dont get damaged
            if (bulletDamage) 
                bulletDamage.tag = shooter.tag;

            attacked = true;
        }

        if (spread == 360)
            amount--;

        return attacked;
    }
}