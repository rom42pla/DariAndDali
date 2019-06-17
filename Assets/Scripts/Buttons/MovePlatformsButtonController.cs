using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformsButtonController : MonoBehaviour
{
    [Header("Platforms connected")]
    public List<GameObject> platforms = new List<GameObject>();
    [HideInInspector] public bool arePlatformsMoving = false;

    void Start(){
        // Colora le piattaforme connesse al bottone
        foreach(GameObject platform in this.platforms){
            this.gameObject.GetComponent<ColorManager>().colorize(platform);
        }
    }

    void Update()
    {
        if(this.gameObject.GetComponent<ButtonController>().isClicked && !arePlatformsMoving){
            arePlatformsMoving = true;
            changePlatformsStatus();
        }
        else if(!this.gameObject.GetComponent<ButtonController>().isClicked && arePlatformsMoving){
            arePlatformsMoving = false;
            changePlatformsStatus();
        }
    }

    /* Cambia lo stato delle piattaforme collegate al bottone */
    public void changePlatformsStatus(){
        foreach(GameObject platform in this.platforms){
            platform.GetComponent<PlatformController>().changeStatus();
        }
    }
}
