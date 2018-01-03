using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class AttackType : ScriptableObject
{
    public int type;
    public float rate = 1;

    public List<Burst> bursts = new List<Burst>();

    public void Attack(GameObject shooter, int id)
    {
        if (id < bursts.Count)
            bursts[id].Attack(shooter);
    }
}

[System.Serializable]
public class Burst
{
    public int amount = 1;
    public float spread = 90;
    public float angle = 0;
    public GameObject prefab;

    public void Attack(GameObject shooter) {
        if(amount == 1) {
            GameObject bullet = Object.Instantiate(prefab, shooter.transform.position, Quaternion.Euler(0, 0, shooter.transform.rotation.eulerAngles.z - angle));
            Damage bulletDamage = bullet.GetComponent<Damage>();
            if (bulletDamage) //Ignore damage tag
                bulletDamage.tag = shooter.tag;
            return;
        }

        if (spread == 360)
            amount++;
        
        for (int i = 0; i < amount; i++)
        {
            GameObject bullet = Object.Instantiate(prefab, shooter.transform.position, Quaternion.Euler(0, 0, shooter.transform.rotation.eulerAngles.z - angle - spread / 2f + i * spread / Mathf.Max(1, amount - 1)));
            Damage bulletDamage = bullet.GetComponent<Damage>();

            //Objects with shooter.tag dont get damaged
            if (bulletDamage) 
                bulletDamage.tag = shooter.tag;
        }

        if (spread == 360)
            amount--;
    }
}