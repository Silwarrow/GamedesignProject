using System;
using System.Numerics;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float speed = 5f;
    public float accelerationValue = 1f;
    public bool devOut = false;

    private float acceleration = 0f, accelTime = 0, deccelTime = 0;
    private UnityEngine.Vector3 momentum = UnityEngine.Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        float size = (transform.localScale.x+transform.localScale.y+transform.localScale.z)/3f;
        
        //Bewegungsrichtung berechnen
        UnityEngine.Vector3 movement = new UnityEngine.Vector3( toInt(Input.GetKey(KeyCode.D)) - toInt(Input.GetKey(KeyCode.A)), 0, 
                                                                toInt(Input.GetKey(KeyCode.W)) - toInt(Input.GetKey(KeyCode.S)));

        //Nach bewegungsdauer
        if(movement == UnityEngine.Vector3.zero)
        {
            deccelTime+=Time.deltaTime;
            accelTime=0;
            acceleration = Mathf.Exp(-deccelTime* (accelerationValue/size));
        }
        else
        {
            momentum = movement;
            accelTime+=Time.deltaTime;
            deccelTime=0;
            acceleration = 1 -Mathf.Exp(-accelTime*(accelerationValue/size));
        }
        

        //bewegen
        transform.Translate(momentum * speed * acceleration * Time.deltaTime);

        //Dev output
        if (devOut)
        {
            Debug.Log("Momentum: " + momentum + " Acceleration: " + acceleration);
        }
    }

    private int toInt(bool b){
        return b ? 1 : 0;
    }
}
