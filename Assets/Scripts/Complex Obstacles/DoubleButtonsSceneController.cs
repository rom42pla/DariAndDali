using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoubleButtonsSceneController : MonoBehaviour
{
    public int levelIndex = 1;
    public int delay = 5;

    void FixedUpdate()
    {
        if(this.gameObject.GetComponent<DoubleButtonsDoorsController>().areDoorsOpen){
            StartCoroutine(changeSceneAfterDelay());
        }
    }

    public IEnumerator changeSceneAfterDelay(){
        yield return new WaitForSeconds(this.delay);
        string newLevel = "Level" + levelIndex;
        SceneManager.LoadScene(newLevel, LoadSceneMode.Single);
    }
}
