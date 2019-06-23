using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // La posizione iniziale del bottone
    private Vector3 unclickedPos, clickedPos;
    private GameObject playerTouching = null;
    // Variabili di stato
    [HideInInspector] public bool isClicked = false;
    [Header("Gravity settings")]
    public bool reversedGravity = false;
    private Coroutine movingCoroutine = null;
    [Header("Audio settings")]
    private AudioSource audioSource;
    public AudioClip switchSFX;

    void Start(){
        audioSource = GetComponent<AudioSource>();
        this.unclickedPos = this.transform.position;
            // Setta la posizione del bottone da premuto
            if(!reversedGravity){
                this.clickedPos = unclickedPos + new Vector3(0f, -0.10f, 0f);
            }
            else{
                this.clickedPos = unclickedPos + new Vector3(0f, +0.10f, 0f);
            }
    }
    void Update(){
        if((this.isClicked) && (this.playerTouching.GetComponent<PlayerController>().isDying)){
            this.isClicked = false;
            changeStatus();
        } 
    }

    /* Eseguita nel momento in cui un giocatore entra nel trigger */
    void OnTriggerEnter(Collider coll)
    {
        // Se il bottone viene toccato da un giocatore...
        if(coll.gameObject.tag == "Player" && !this.isClicked){
            if((!this.reversedGravity && coll.gameObject.GetComponent<PlayerController>().playerNo == 1) ||
            (this.reversedGravity && coll.gameObject.GetComponent<PlayerController>().playerNo == 2)){
                this.playerTouching = coll.gameObject;
                // Il bottone entra nello stato "isClicked"
                isClicked = true;
                changeStatus();
            }
        }
    }
    /* Eseguita nel momento in cui un giocatore esce dal trigger */
    void OnTriggerExit(Collider coll)
    {
        // Se il bottone viene toccato da un giocatore...
        if(coll.gameObject.tag == "Player"){
            // Il bottone entra nello stato "not isClicked"
            isClicked = false;
            changeStatus();
        }  
    }

    /* Cambia lo stato del bottone da premuto a non premuto e viceversa */
    void changeStatus(){
        if(isClicked){
            if(movingCoroutine != null) {
                StopCoroutine(movingCoroutine);
            }
            audioSource.PlayOneShot(switchSFX);
            movingCoroutine = StartCoroutine(moveObject(this.clickedPos, 1f));
        }
        if(!isClicked){
            if(movingCoroutine != null) {
                StopCoroutine(movingCoroutine);
            }
            movingCoroutine = StartCoroutine(moveObject(this.unclickedPos, 1f));
        }
    }
    
    /* Coroutine che muove gradualmente un oggetto verso una coordinata */
    public IEnumerator moveObject(Vector3 position, float speed){
        while (Vector3.Distance(this.gameObject.transform.position, position) > speed * Time.deltaTime){
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, position, speed * Time.deltaTime);
            yield return 0;
        }
        this.gameObject.transform.position = position;
    }

}
