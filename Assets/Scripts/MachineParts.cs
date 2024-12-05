using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineParts : MonoBehaviour, IItem
{
    public static event Action<int> OnGearCollect;
    public int worth = 5;

    public void Collect() {
        OnGearCollect.Invoke(worth);
        Destroy(gameObject);
    }
   
}
