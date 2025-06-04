using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstanceTest : MonoBehaviour {
    public GameObject go;
    public Color color;
    public Material material;

    void Start()
    {
        go = this.gameObject;
        material = go.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        material.color = color;
    }
}
