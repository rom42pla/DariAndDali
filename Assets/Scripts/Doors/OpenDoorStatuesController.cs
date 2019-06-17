using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorStatuesController : MonoBehaviour
{
    [HideInInspector] bool isOpen = false; 
    [Header("Statues connected")]
    public List<GameObject> statues = new List<GameObject>();

    void Update()
    {
        if(!this.isOpen){
            if(allStatuesAtGoalRotation()){
                this.isOpen = true;
                this.gameObject.GetComponent<DoorController>().changeStatus();
            }
        } 
    }

    bool allStatuesAtGoalRotation(){
        foreach(GameObject statue in statues){
            if(!statue.GetComponent<StatueController>().isAtGoalRotation()){
                return false;
            }
        }
        return true;
    }
}
