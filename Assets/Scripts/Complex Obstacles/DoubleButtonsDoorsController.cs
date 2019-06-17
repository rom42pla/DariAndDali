using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleButtonsDoorsController : MonoBehaviour
{
    public GameObject button1, button2;
    public List<GameObject> doors = new List<GameObject>();
    [HideInInspector] public bool areDoorsOpen = false;

    void Awake(){
        // Colora i bottoni connessi
        this.gameObject.GetComponent<ColorManager>().colorize(button1);
        this.gameObject.GetComponent<ColorManager>().colorize(button2);
        // Colora le porte connesse al bottone
        foreach(GameObject door in this.doors){
            this.gameObject.GetComponent<ColorManager>().colorize(door);
        }
    }

    void Update()
    {
        if(button1.GetComponent<ButtonController>().isClicked && button2.GetComponent<ButtonController>().isClicked && !areDoorsOpen){
            areDoorsOpen = true;
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
