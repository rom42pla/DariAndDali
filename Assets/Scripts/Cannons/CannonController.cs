using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right };

    [Header("Cannon settings")]
    public Direction direction = Direction.Left;
    public float missilesPerSecond = 0.5f;
    public bool reversedGravity = false;
    [Header("Missile settings")]
    public GameObject cannonBall;
    public float missileSpeed = 1f;
    private GameObject missile;

    // Start is called before the first frame update
    void Start()
    {
        this.rotateCannon();
        missile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        missile.transform.position = this.gameObject.transform.position;
        StartCoroutine(spawnMissile());
    }

    /* Coroutine che muove gradualmente un oggetto verso una coordinata */
    public IEnumerator moveMissile(GameObject missile){
        Vector3 missileDirection;
        if(this.direction == Direction.Left) missileDirection = new Vector3(-1, 0, 0);
        else if(this.direction == Direction.Up) missileDirection = new Vector3(0, 1, 0);
        else if(this.direction == Direction.Right) missileDirection = new Vector3(1, 0, 0);
        else missileDirection = new Vector3(0, -1, 0);
        while (missile != null){
            missile.transform.position = Vector3.MoveTowards(missile.transform.position, missile.transform.position + missileDirection, missileSpeed * Time.deltaTime);
            yield return 0;
        }
    }

    /* Coroutine che spawna dei missili ad intermittenza */
    public IEnumerator spawnMissile(){
        while(true){
            GameObject missile = Instantiate(this.cannonBall, this.transform);
            StartCoroutine(moveMissile(missile));
            yield return new WaitForSeconds(1f/missilesPerSecond);
        }  
    }

    /* Coroutine che ruota il cannoncino in base alla direzione indicata */
    public void rotateCannon(){
        /* La rotazione iniziale, rivolto verso sinistra */
        this.transform.rotation = Quaternion.identity;
        if(!this.reversedGravity){
            if(this.direction == Direction.Up) this.transform.Rotate(0, 0, 270, Space.Self);
            else if(this.direction == Direction.Right) this.transform.Rotate(180, 180, 0, Space.Self);
            else if(this.direction == Direction.Down) this.transform.Rotate(0, 0, 90, Space.Self);
            else this.transform.Rotate(180, 0, 0, Space.Self);
        }
        else{
            if(this.direction == Direction.Up) this.transform.Rotate(180, 0, 270, Space.Self);
            else if(this.direction == Direction.Right) this.transform.Rotate(0, 180, 0, Space.Self);
            else if(this.direction == Direction.Down) this.transform.Rotate(180, 0, 90, Space.Self);
            else this.transform.Rotate(0, 0, 0, Space.Self);
        }
        
    }
}
