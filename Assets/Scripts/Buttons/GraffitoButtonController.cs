using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitoButtonController : MonoBehaviour
{
    [Header("Graffiti connected")]
    public List<GameObject> graffiti = new List<GameObject>();
    [HideInInspector] public bool areGraffitiShowed = false;

    void Start(){

    }

    void Update()
    {
        if(this.gameObject.GetComponent<ButtonController>().isClicked && !areGraffitiShowed){
            areGraffitiShowed = true;
            changeGraffitiStatus();
        }
        else if(!this.gameObject.GetComponent<ButtonController>().isClicked && areGraffitiShowed){
            areGraffitiShowed = false;
            changeGraffitiStatus();
        }
    }

    /* Cambia lo stato delle piattaforme collegate al bottone */
    public void changeGraffitiStatus(){
        foreach(GameObject graffito in this.graffiti){
            graffito.GetComponent<GraffitoController>().changeStatus();
        }
    }
}
