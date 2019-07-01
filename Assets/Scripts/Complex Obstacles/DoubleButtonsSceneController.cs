using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoubleButtonsSceneController : MonoBehaviour
{
    public bool levelEnd = false;
    void FixedUpdate()
    {

        if (this.gameObject.GetComponent<DoubleButtonsDoorsController>().areDoorsOpen){
            if (levelEnd) {
                print("livello finito");
                Animator animator= transform.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Image>().GetComponent<Animator>();
                animator.SetTrigger("FadeOut");
                
            }
            
        }
    }

}
