using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public float maxStorage = 10000;
    public float currentStorage;


    void Start()
    {
        currentStorage = 0;
    }

    void Update()
    {
        if (currentStorage > maxStorage)
        {
            currentStorage = maxStorage;
        }
    }
    public bool Give()
    {
        if (currentStorage >= maxStorage)
        {
            return false;
            // ai finishes
        }
        return true;
    }
}
