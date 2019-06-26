using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    public GameObject soundOn;
    public GameObject soundOff;
    [SerializeField]
    private float fadeTime = 1;


    public void Exit()
    {
        Application.Quit();
        print("uscito");
    }


    public void ToggleSound()
    {

        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            PlayerPrefs.SetInt("Muted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Muted", 0);

        }
        SetSoundState();
    }

    private void SetSoundState()
    {

        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            AudioListener.volume = 1;
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            print("volume: " + AudioListener.volume);
            print(PlayerPrefs.GetInt("Muted", 0));
        }
        else
        {
            AudioListener.volume = 0;
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            print("volume: " + AudioListener.volume);
            print(PlayerPrefs.GetInt("Muted", 0));
        }

    }

    public void Pause(GameObject PauseUI)
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;   
    }

    public void Resume() {
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SelectMenu(GameObject nextMenu) {
        StartCoroutine(DoFadeOff(nextMenu));
        
    }

    IEnumerator DoFadeOff(GameObject next)
    {
        CanvasGroup myCanvas = transform.parent.gameObject.GetComponent<CanvasGroup>();

        while (myCanvas.alpha > 0f)
        {
            myCanvas.alpha -= Time.deltaTime / fadeTime;
            yield return  null;
        }
        
        StartCoroutine(DoFadeOn(next));
        //myCanvas.interactable = false;
        
        yield return null;        
    }


    IEnumerator DoFadeOn(GameObject next)
    {
        
        CanvasGroup newCanvas = next.GetComponent<CanvasGroup>();
        newCanvas.alpha = 0;

        next.SetActive(true);

        while (newCanvas.alpha < 1f)
        {
            
            newCanvas.alpha += Time.deltaTime / (fadeTime * 0.9f);

            yield return null;
        }
        transform.parent.gameObject.SetActive(false);
        //newCanvas.interactable = false;

        yield return null;

    }

    IEnumerator Waiter() {
        print(Time.time);
        yield return new WaitForSeconds(5);
        print(Time.time);
    }

}
