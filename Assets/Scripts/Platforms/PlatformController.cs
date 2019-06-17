using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Vector2 endPositionDelta = new Vector2(4, 0);
    public float speed = 1f;
    private Vector3 startPos, endPos;
    public bool autoMovement = true;
    [HideInInspector] public bool isGoingToEndPos = true;
    private bool isMoving = false;
    private Coroutine movingCoroutine = null;

    void Start()
    {
        this.startPos = this.transform.position;
        this.endPos = this.startPos + new Vector3(this.endPositionDelta.x, this.endPositionDelta.y, 0);
    }

    void Update()
    {
        if(!autoMovement && movingCoroutine != null){
            StopCoroutine(movingCoroutine);
            this.isMoving = false;
        }
        if(this.isGoingToEndPos && autoMovement && !isMoving) {
            movingCoroutine = StartCoroutine(moveObject(this.endPos, this.speed));
        }
        
        if(!this.isGoingToEndPos && autoMovement && !isMoving) {
            movingCoroutine = StartCoroutine(moveObject(this.startPos, this.speed));
        }
    }
    
    /* Coroutine che muove gradualmente un oggetto verso una coordinata */
    public IEnumerator moveObject(Vector3 position, float speed){
        this.isMoving = true;
        while (Vector3.Distance(this.gameObject.transform.position, position) > speed * Time.deltaTime){
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, position, speed * Time.deltaTime);
            yield return 0;
        }
        this.gameObject.transform.position = position;
        this.isGoingToEndPos = !this.isGoingToEndPos;
        this.isMoving = false;
    }

    public void changeStatus(){
        this.autoMovement = !this.autoMovement;
    }
}
