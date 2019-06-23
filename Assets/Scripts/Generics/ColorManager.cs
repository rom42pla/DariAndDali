using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public Color color;
    private Material material;

    void Start()
    {
        // Colora il gameobject associato
        colorize(this.gameObject);
    }

    public void colorize(GameObject obj){
        if(obj.GetComponent<Renderer>() != null){
            Renderer rend = obj.GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Custom/MobileColor"));
            rend.material.color = color;
        }
    }
}
