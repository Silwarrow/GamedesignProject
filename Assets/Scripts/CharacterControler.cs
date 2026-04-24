using System;
using System.Numerics;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float speed = 5f;
    public bool devOut = false;
    public float sizeChange = 0f;
    public float maxSize = 10f;
    public float minSize = 0.1f;

    private float size;
    private UnityEngine.Vector3 momentum = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 momentumVelocity = UnityEngine.Vector3.zero;
    private GameObject PlayerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        PlayerManager = GameObject.Find("PlayerManager");
    }

    // Update is called once per frame
    void Update(){
        size = transform.localScale.x;

        //Bewegungsrichtung berechnen
        UnityEngine.Vector3 movement = new UnityEngine.Vector3( toInt(Input.GetKey(KeyCode.D)) - toInt(Input.GetKey(KeyCode.A)), 0, 
                                                                toInt(Input.GetKey(KeyCode.W)) - toInt(Input.GetKey(KeyCode.S)));

        //Smooth movement
        momentum = UnityEngine.Vector3.SmoothDamp(momentum, movement, ref momentumVelocity, Mathf.Sqrt((float)Math.Pow(size, 1.5)/10));
        
        if(momentum != UnityEngine.Vector3.zero){
            transform.localScale = new UnityEngine.Vector3(size, size, size) + new UnityEngine.Vector3(sizeChange/100*momentum.magnitude, sizeChange/100*momentum.magnitude, sizeChange/100*momentum.magnitude);
        }

        //bewegen
        transform.Translate(momentum * speed* Time.deltaTime);

        if(size < minSize || size > maxSize){
            PlayerManager.GetComponent<RespawnControler>().RespawnPlayer();
            Destroy(gameObject);
        }

        //Dev output
        if (devOut)
        {
            Debug.Log("Momentum: " + momentum);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Fals Todesszone betreten, Spieler respawnen und aktuelles Objekt zerstören
        if (other.CompareTag("DeathArea"))
        {
            PlayerManager.GetComponent<RespawnControler>().RespawnPlayer();
            Destroy(gameObject);
        }
    }

    private int toInt(bool b){
        return b ? 1 : 0;
    }
}
