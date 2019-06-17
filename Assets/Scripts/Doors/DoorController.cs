using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Gravity settings")]
    public bool reversedGravity = false;
    [Header("Movement settings")]
    public float speed = 5f;
    public float deltaMovement = 2f;
    private bool isOpen = false;
    private Vector3 closedPos, openPos;
    private Coroutine movingCoroutine = null;

    void Start(){
        closedPos = this.gameObject.transform.position;
        if(!reversedGravity){
            openPos = this.closedPos + new Vector3(0f, deltaMovement, 0f);
        }
        else{
            openPos = this.closedPos + new Vector3(0f, -deltaMovement, 0f);
        }
        
    }

    public void changeStatus(){
        if(isOpen){
            isOpen = false;
            if(movingCoroutine != null) {
                StopCoroutine(movingCoroutine);
            }
            movingCoroutine = StartCoroutine(moveObject(this.closedPos, this.speed));
        }
        else{
            isOpen = true;
            if(movingCoroutine != null) {
                StopCoroutine(movingCoroutine);
            }
            movingCoroutine = StartCoroutine(moveObject(this.openPos, this.speed));
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
