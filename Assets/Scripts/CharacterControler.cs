using System;
using System.Numerics;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float speed = 5f;
    public float accelerationValue = 1f;
    public bool devOut = false;
    public float size = 1f;

    private float acceleration = 0f, accelTime = 0, deccelTime = 0;
    private UnityEngine.Vector3 momentum = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 momentumVelocity = UnityEngine.Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if(size != transform.localScale.x)
        {
            transform.localScale = new UnityEngine.Vector3(size, size, size);
            transform.Translate(0, size/2, 0);
        }

        //Bewegungsrichtung berechnen
        UnityEngine.Vector3 movement = new UnityEngine.Vector3( toInt(Input.GetKey(KeyCode.D)) - toInt(Input.GetKey(KeyCode.A)), 0, 
                                                                toInt(Input.GetKey(KeyCode.W)) - toInt(Input.GetKey(KeyCode.S)));

        momentum = UnityEngine.Vector3.SmoothDamp(momentum, movement, ref momentumVelocity, Mathf.Sqrt(size/10));
        

        //bewegen
        transform.Translate(momentum * speed* Time.deltaTime);

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
