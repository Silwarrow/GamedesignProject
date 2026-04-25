using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float speed = 5f;
    public bool devOut = false;
    public float sizeChange = 0f;
    public float maxSize = 10f;
    public float minSize = 0.1f;
    public bool canJump = false;
    public float jumpHeight = 750f;

    private float size;
    private Vector3 momentum = Vector3.zero;
    private Vector3 momentumVelocity = Vector3.zero;
    private GameObject PlayerManager;
    private bool isGrounded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        PlayerManager = FindFirstObjectByType<RespawnControler>().gameObject;

    }

    // Update is called once per frame
    void Update(){
        size = transform.localScale.x;

        //Bewegungsrichtung berechnen
        Vector3 movement = new Vector3( toInt(Input.GetKey(KeyCode.D)) - toInt(Input.GetKey(KeyCode.A)), 0, 
                                        toInt(Input.GetKey(KeyCode.W)) - toInt(Input.GetKey(KeyCode.S)));

        //Smooth movement
        float momentumVariable = Mathf.Sqrt((float)Mathf.Pow(size, 1.5f)/10);
        momentum = Vector3.SmoothDamp(momentum, movement, ref momentumVelocity, momentumVariable);
        
        //Größer werden
        if(((momentum.x >= 0.1f || momentum.x <= -0.1f) || (momentum.z >= 0.1f || momentum.z <= -0.1f)) && isGrounded){
            Debug.Log("MomentumZ: " + momentum.z + " MomentumX: " + momentum.x);
            transform.localScale = new Vector3(size, size, size) + new Vector3(sizeChange/100*momentum.magnitude, sizeChange/100*momentum.magnitude, sizeChange/100*momentum.magnitude);
        }

        //Sprungfähigkeit testen
        isGrounded = Physics.Raycast(transform.position, Vector3.down, size/2 + 0.1f);

        //Springen
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            momentum.y = jumpHeight;
            isGrounded = false;
        }

        //bewegen
        transform.Translate(momentum * speed* Time.deltaTime);

        //Tod nach Größe
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
