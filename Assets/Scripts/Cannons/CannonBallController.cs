using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    private enum Direction { Up, Down, Left, Right };
    private float xLimit, yLimit;

    void Start()
    {
        GameManager gameManagerScript = GameObject.Find("EventSystem").GetComponent<GameManager>();
        this.xLimit = gameManagerScript.xLimit;
        this.yLimit = gameManagerScript.yLimit;
        
    }

    void Update()
    {
        if((this.gameObject.transform.position.x > this.xLimit) || (this.gameObject.transform.position.x < -this.xLimit) || (this.gameObject.transform.position.y > this.yLimit) || (this.gameObject.transform.position.y < -this.yLimit)){
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player"){
            coll.gameObject.GetComponent<PlayerController>().respawn();
        }
        else if(coll.gameObject.tag != "Cannon"){
            Destroy(this.gameObject);
        }
    }
}
