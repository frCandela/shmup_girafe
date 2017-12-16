using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class AttackType : ScriptableObject {
    public int type;
    public float rate = 1;

    public List<Burst> bursts = new List<Burst>();
    public Texture previewShip;

    public void Attack(Transform trans, int id) {
        bursts[id].Attack(trans);
    }
}

[System.Serializable]
public class Burst {
    public int bullets = 1;
    public GameObject prefab;
    public BurstType type = BurstType.CIRCLE;

    public enum BurstType {
        CIRCLE, FRONT
    }

    public void Attack(Transform trans) {
        switch(type) {
            case BurstType.CIRCLE:
                for (int i = 0; i < bullets; i++) {
                    Object.Instantiate(prefab, trans.position, Quaternion.Euler(0, 0, trans.rotation.eulerAngles.z + i * 360 / bullets));
                }
                break;
            case BurstType.FRONT:
                for (int i = 1; i < bullets + 1; i++) {
                    Object.Instantiate(prefab, trans.position, Quaternion.Euler(0, 0, trans.rotation.eulerAngles.z + i * 90 / (bullets + 1) - 45));
                }
                break;
        }
    }
}