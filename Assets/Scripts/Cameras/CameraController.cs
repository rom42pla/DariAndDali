using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player1, player2;
    public bool lockedOnZ = false, lockedOnY = true;
    public float distanceFromPlayers = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!lockedOnZ && !lockedOnY){
            this.transform.position = new Vector3((player1.transform.position.x + player2.transform.position.x)/2, (player1.transform.position.y + player2.transform.position.y)/2, -Vector3.Distance(player1.transform.position, player2.transform.position)/2 - distanceFromPlayers);
        }
        else if (lockedOnZ && !lockedOnY){
            this.transform.position = new Vector3((player1.transform.position.x + player2.transform.position.x)/2, (player1.transform.position.y + player2.transform.position.y)/2, -distanceFromPlayers * 2);
        }
        else if (!lockedOnZ && lockedOnY){
            this.transform.position = new Vector3((player1.transform.position.x + player2.transform.position.x)/2, 0, -Vector3.Distance(player1.transform.position, player2.transform.position)/2 - distanceFromPlayers);
        }
        else if (lockedOnZ && lockedOnY){
            this.transform.position = new Vector3((player1.transform.position.x + player2.transform.position.x)/2, 0, -distanceFromPlayers * 2);
        }
    }
}
