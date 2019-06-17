using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingThingController : MonoBehaviour
{
    public Vector2 endPositionDelta = new Vector2(4, 0);
    public float speed = 1f;
    private Vector3 startPos, endPos;

    void Start()
    {
        this.startPos = this.transform.position;
        this.endPos = this.startPos + new Vector3(this.endPositionDelta.x, this.endPositionDelta.y, 0);
    }

    void Update()
    {
        if(this.transform.position == startPos) {
            StartCoroutine(moveObject(this.endPos, this.speed));
        }
        
        if(this.transform.position == endPos) {
            StartCoroutine(moveObject(this.startPos, this.speed));
        }
    }

    /* Eseguita nel momento in cui un giocatore entra nel trigger */
    void OnTriggerEnter(Collider coll)
    {
        // Se viene toccato da un giocatore...
        if(coll.gameObject.tag == "Player"){
            coll.gameObject.GetComponent<PlayerController>().respawn();
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
