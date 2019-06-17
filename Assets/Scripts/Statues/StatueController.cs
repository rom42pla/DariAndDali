using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueController : MonoBehaviour
{
    [Header("Rotation settings")]
    [RangeAttribute(0, 3)] public int initialRotation, goalRotation;
    [HideInInspector] public int currentRotation;
    public float rotationSpeed = 10f;
    private bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(this.gameObject.transform.eulerAngles.x, (initialRotation * 90), this.gameObject.transform.eulerAngles.z);
        this.currentRotation = this.initialRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isRotating){
            rotate();
        }
    }

    public void rotate(){
        if(this.currentRotation == 3){
            currentRotation = 0;
        }
        else{
            currentRotation++;
        }
        StartCoroutine(rotateObjectY(90, rotationSpeed));
    }

    /* Controlla se la statua si trova nella posizione obiettivo */
    public bool isAtGoalRotation(){
        return this.currentRotation == this.goalRotation;
    }

    /* Coroutine che ruota gradualmente un oggetto sull'asse Y */
    public IEnumerator rotateObjectY(float rotation, float speed){
        this.isRotating = true;
        // I quaternioni delle rotazioni iniziale e finale
        Quaternion startRotation = this.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y + rotation, this.gameObject.transform.eulerAngles.z);
        for (float t = 0.0f; t < 1.0; t += Time.deltaTime * speed) 
        { 
            this.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t); 
            yield return null;
        }
        this.gameObject.transform.rotation = endRotation;
        this.isRotating = false;
    }
}
