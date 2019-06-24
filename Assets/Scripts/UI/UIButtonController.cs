using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public bool isPressed = false, scaled = false;
    private Vector3 initialScale, scaleIfPressed;
    [HideInInspector] public float scaleFactorIfPressed = 0.95f;

    void Start(){
        this.initialScale = this.transform.localScale;
        this.scaleIfPressed = new Vector3(initialScale.x * scaleFactorIfPressed, initialScale.y * scaleFactorIfPressed, initialScale.z * scaleFactorIfPressed);
    }
    void Update(){
        if(isPressed && !scaled){
            this.transform.localScale = scaleIfPressed;
            this.scaled = true;
        }
        if(!isPressed && scaled){
            this.transform.localScale = initialScale;
            this.scaled = false;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void restartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
