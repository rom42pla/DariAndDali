using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoubleButtonsSceneController : MonoBehaviour
{
    public int levelIndex = 1;
    void FixedUpdate()
    {
        if(this.gameObject.GetComponent<DoubleButtonsDoorsController>().areDoorsOpen){
            string newLevel = "Level" + levelIndex;
            SceneManager.LoadScene(newLevel, LoadSceneMode.Single);
        }
    }
}
