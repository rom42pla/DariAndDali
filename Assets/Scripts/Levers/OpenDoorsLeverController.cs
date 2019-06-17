using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorsLeverController : MonoBehaviour
{
    [Header("Doors connected")]
    public List<GameObject> doors = new List<GameObject>();
    [HideInInspector] public bool areDoorsOpen = false;

    void Start(){
        // Colora le porte connesse al bottone
        foreach(GameObject door in this.doors){
            this.gameObject.GetComponent<ColorManager>().colorize(door);
        }
    }

    void Update()
    {
        if(this.GetComponent<LeverController>().isActive && !areDoorsOpen){
            areDoorsOpen = true;
            changeDoorsStatus();
        }
        else if(!this.GetComponent<LeverController>().isActive && areDoorsOpen){
            areDoorsOpen = false;
            changeDoorsStatus();
        }
    }

    /* Cambia lo stato delle porte collegate al bottone */
    public void changeDoorsStatus(){
        foreach(GameObject door in this.doors){
            door.GetComponent<DoorController>().changeStatus();
        }
    }
}
