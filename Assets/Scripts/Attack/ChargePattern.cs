using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class ChargePattern : ScriptableObject {
    public float rate = 1;
    public List<Charge> charges = new List<Charge>();
}


[System.Serializable]
public class Charge {
    public float distance = 2;
    public float angle = 0;
}