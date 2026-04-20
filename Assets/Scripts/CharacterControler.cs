using System;
using System.Numerics;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float speed = 5f;
    public float accelerationValue = 1f;
    public bool devOut = false;
    private int time = 0;
    private int movementDuration = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        //Bewegungsrichtung berechnen
        UnityEngine.Vector3 movement = new UnityEngine.Vector3( toInt(Input.GetKey(KeyCode.D)) - toInt(Input.GetKey(KeyCode.A)), 0, 
                                                                toInt(Input.GetKey(KeyCode.W)) - toInt(Input.GetKey(KeyCode.S)));

        //Nach bewegungsdauer
        if(movement == UnityEngine.Vector3.zero){
            movementDuration = 0;
            time = 0;
        }else{
            movementDuration++;
        }
        float size = (transform.localScale.x+transform.localScale.y+transform.localScale.z)/3f;
        float acceleration = 1 - Mathf.Exp(-movementDuration * accelerationValue/(size*3.14159f));
        if(devOut){
            Debug.Log(acceleration);
            
            if(movementDuration > 0 && acceleration == 1)
            {
                if(time == 0){
                    time = movementDuration;
                }
                Debug.Log("Maximalgeschwindigkeit erreicht bei time = " + time);
            }
        }

        //bewegen
        transform.Translate(movement * speed * acceleration * Time.deltaTime);
    }

    private int toInt(bool b){
        return b ? 1 : 0;
    }
}
