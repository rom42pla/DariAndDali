using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStatueController : MonoBehaviour
{
    [Header("Statues connected")]
    public List<GameObject> statues = new List<GameObject>();
    [HideInInspector] public bool haveStatuesJustMoved = false;

    void Start(){
        // Colora le statue connesse al bottone
        foreach(GameObject statue in this.statues){
            foreach(Transform partTransform in statue.transform){
                this.gameObject.GetComponent<ColorManager>().colorize(partTransform.gameObject);
            }
        }
    }

    void Update()
    {
        if(this.gameObject.GetComponent<ButtonController>().isClicked && !haveStatuesJustMoved){
            haveStatuesJustMoved = true;
            rotateStatues();
        }
        else if(!this.gameObject.GetComponent<ButtonController>().isClicked && haveStatuesJustMoved){
            haveStatuesJustMoved = false;
        }
    }

    /* Cambia lo stato delle statue collegate al bottone */
    public void rotateStatues(){
        foreach(GameObject statue in this.statues){
            statue.GetComponent<StatueController>().rotate();
        }
    }
}
