using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private int maxCap = 2000;
    [SerializeField] private float generated;
    public int taken;
    public float howFull;

    private Renderer genRenderer;
    private Color genColor;

    void Awake()
    {
        genRenderer = this.GetComponent<Renderer>();
    }

    void OnEnable()
    {
        generated = 0;
    }

    void FixedUpdate()
    {
        // Cap max resource generated
        if (generated <= maxCap)
        {
            generated++;
        }

        // Change color of generator depending on how full it is
        genColor = new Color(1f, 1f - (generated / maxCap), 1f, 1f);
        genRenderer.material.SetColor("_Color", genColor);

        // Calculate how full
        howFull = generated / maxCap;
    }
    public bool CanTake()
    {
        if (generated <= 0)
        {
            return false;
        }
        taken = (int)generated;
        generated = 0;
        return true;
    }

    //[ContextMenu("Snap")]
    //private void Snap()
    //{
    //    if (NavMesh.SamplePosition(transform.position, out var hit, 5f, NavMesh.AllAreas))
    //    {
    //        transform.position = hit.position;
    //    }
    //}
}