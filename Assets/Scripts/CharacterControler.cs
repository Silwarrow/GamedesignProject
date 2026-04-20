using System;
using System.Numerics;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float speed = 5f;
    public bool devOut = false;
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
        }else{
            movementDuration++;
        }
        float acceleration = 1 - Mathf.Exp(-movementDuration / 200f);
        if(devOut){
            Debug.Log(acceleration);
        }

        //bewegen
        transform.Translate(movement * speed * acceleration * Time.deltaTime);
    }

    private int toInt(bool b){
        return b ? 1 : 0;
    }
}
