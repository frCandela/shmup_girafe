using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class AttackType : ScriptableObject
{
    public int type;
    public float rate = 1;

    public List<Burst> bursts = new List<Burst>();
    public Texture previewShip;


    public void Attack(GameObject shooter, int id)
    {
        bursts[id].Attack(shooter);
    }
}

[System.Serializable]
public class Burst
{
    public int bullets = 1;
    public GameObject prefab;
    public BurstType type = BurstType.CIRCLE;

    public enum BurstType
    {
        CIRCLE, FRONT
    }

    public void Attack(GameObject shooter)
    {
        switch(type) {
            case BurstType.CIRCLE:
                for (int i = 0; i < bullets; i++)
                {
                    GameObject bullet = Object.Instantiate(prefab, shooter.transform.position, Quaternion.Euler(0, 0, shooter.transform.rotation.eulerAngles.z + i * 360 / bullets));
                    Damage bulletDamage = bullet.GetComponent<Damage>();
                    if (bulletDamage)//Ignore damage tag
                        bulletDamage.tag = shooter.tag;
                }
                break;
            case BurstType.FRONT:
                for (int i = 1; i < bullets + 1; i++)
                {
                    GameObject bullet = Object.Instantiate(prefab, shooter.transform.position, Quaternion.Euler(0, 0, shooter.transform.rotation.eulerAngles.z + i * 90 / (bullets + 1) - 45));
                    Damage bulletDamage = bullet.GetComponent<Damage>();
                    if(bulletDamage)//Ignore damage tag
                        bulletDamage.tag = shooter.tag;
                }
                break;
        }
    }
}