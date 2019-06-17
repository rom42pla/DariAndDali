using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour
{
    [Header("Position settings")]
    // X ed Y in [-500, 500], Z in [300, 800]
    public Vector3 position = new Vector3(0, 0, 500);
    [Header("Rotation settings")]
    public Vector3 rotation = new Vector3(0, 359, 0);
    public float rotationSpeed = 5f;
    private bool isRotating = false;
    [Header("Size settings")]
    // X, Y e Z in [1, 5]
    public Vector3 size = new Vector3(1, 1, 1);

    void Start(){
        this.transform.position = this.position;
        this.transform.localScale = new Vector3(0.1f * this.size.x, 0.1f * this.size.y, 0.1f * this.size.z);
    }

    void Update()
    {
        if(!this.isRotating){
            StartCoroutine(rotateObject(this.rotation.x, this.rotation.y, this.rotation.z, rotationSpeed));
        }
    }

    /* Coroutine che ruota gradualmente un oggetto */
    public IEnumerator rotateObject(float x, float y, float z, float speed){
        this.isRotating = true;
        // I quaternioni delle rotazioni iniziale e finale
        Quaternion startRotation = this.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(this.gameObject.transform.eulerAngles.x + x, this.gameObject.transform.eulerAngles.y + y, this.gameObject.transform.eulerAngles.z + z);
        for (float t = 0.0f; t < 1.0; t += Time.deltaTime * speed) 
        { 
            this.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t); 
            yield return null;
        }
        this.gameObject.transform.rotation = endRotation;
        this.isRotating = false;
    }
}
