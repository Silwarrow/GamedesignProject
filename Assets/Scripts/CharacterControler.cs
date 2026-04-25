using System;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float speed = 5f;
    public bool devOut = false;
    public float sizeChange = 0f;
    public float maxSize = 10f;
    public float minSize = 0.1f;

    private float size;
    private Vector3 momentum = Vector3.zero;
    private Vector3 momentumVelocity = Vector3.zero;
    private GameObject PlayerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        PlayerManager = GameObject.Find("PlayerManager");
    }

    // Update is called once per frame
    void Update(){
        size = transform.localScale.x;

        //Bewegungsrichtung berechnen
        Vector3 movement = new Vector3( toInt(Input.GetKey(KeyCode.D)) - toInt(Input.GetKey(KeyCode.A)), 0, 
                                                                toInt(Input.GetKey(KeyCode.W)) - toInt(Input.GetKey(KeyCode.S)));

        //Smooth movement
        momentum = Vector3.SmoothDamp(momentum, movement, ref momentumVelocity, Mathf.Sqrt((float)Math.Pow(size, 1.5)/10));
        
        if(momentum != Vector3.zero){
            transform.localScale = new Vector3(size, size, size) + new Vector3(sizeChange/100*momentum.magnitude, sizeChange/100*momentum.magnitude, sizeChange/100*momentum.magnitude);
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
