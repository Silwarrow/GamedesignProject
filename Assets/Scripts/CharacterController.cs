using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    public bool devOut = false;
    public float growthRate = 0.5f;
    public float shrinkRate = 0.2f;
    public float maxSize = 10f;
    public float minSize = 1f;
    public bool canJump = false;
    public float jumpHeight = 750f;

    private float size;
    private Vector3 momentum = Vector3.zero;
    private Vector3 momentumVelocity = Vector3.zero;
    private GameObject PlayerManager;
    private bool isGrounded = false;
    private bool isSmelting = true;
    private bool isInSafeArea = false;
    private UnityEngine.UIElements.Slider meltBar;
    private bool fastGrow = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        PlayerManager = FindFirstObjectByType<RespawnController>().gameObject;
        meltBar = FindFirstObjectByType<Canvas>().GetComponentInChildren<UnityEngine.UIElements.Slider>();

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

        //Bremsen bei Kollision
        if(IsColliderInFront(momentum.normalized)){
            momentum = Vector3.zero;
        }
        momentum = momentum * (fastGrow ? 0.9f : 1);
        
        
        //Größer werden
        if(isGrounded && !isInSafeArea){
            float growthMultiplier = fastGrow ? growthRate*2*Mathf.Pow(momentum.magnitude, 2f) : growthRate*Mathf.Pow(momentum.magnitude, 2f);
            transform.localScale = Vector3.one * size + Vector3.one * growthMultiplier * Time.deltaTime;
        }
        //Kleiner werden
        if(isSmelting && !isInSafeArea){
            transform.localScale = Vector3.one * size - Vector3.one * shrinkRate * Time.deltaTime;
        }

        //Canvas Slider anpassen
        meltBar.value = ((size - minSize) / (maxSize - minSize)) * 200 - 100;

        //Sprungfähigkeit testen
        isGrounded = Physics.Raycast(transform.position, Vector3.down, size/2 + 0.1f);

        //Springen
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            momentum.y = jumpHeight/Mathf.Sqrt(size);
            isGrounded = false;
        }

        //Bewegen
        transform.Translate(momentum * speed * Time.deltaTime);



        //Tod nach Größe
        if(size < minSize || size > maxSize){
            PlayerManager.GetComponent<RespawnController>().RespawnPlayer();
            Destroy(gameObject);
        }

        //Dev output
        if (devOut)
        {
            Debug.Log("Momentum: " + momentum
            + " | Size: " + size
            + " | Grounded: " + isGrounded
            + " | Smelting: " + isSmelting
            + " | In Safe Area: " + isInSafeArea
            + " | Fast Grow: " + fastGrow);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Falls Todeszone betreten, Spieler respawnen und aktuelles Objekt zerstören
        if (other.CompareTag("DeathArea"))
        {
            PlayerManager.GetComponent<RespawnController>().RespawnPlayer();
            Destroy(gameObject);
        }
        if (other.CompareTag("Shadow")){
            isSmelting = false;
        }
        if (other.CompareTag("SafeArea")){
            isInSafeArea = true;
        }
        if(other.CompareTag("ThickSnow")){
            fastGrow = true;
        }
    }
    void OnTriggerExit(Collider other){
        if (other.CompareTag("Shadow")){
            isSmelting = true;
        }
        if (other.CompareTag("SafeArea")){
            isInSafeArea = false;
        }
        if(other.CompareTag("ThickSnow")){
            fastGrow = false;
        }
    }

    bool IsColliderInFront(Vector3 direction)
    {
        float radius = size / 2f;
        float castDistance = (speed * Time.deltaTime) + 0.05f; // kleine Sicherheitsreserve
        Vector3 origin = transform.position + direction * 0.01f; // nicht im eigenen Collider starten

        if (Physics.SphereCast(
            origin,
            radius,
            direction,
            out RaycastHit hit,
            castDistance,
            ~0,
            QueryTriggerInteraction.Ignore))
        {
            // Spieler selbst ignorieren
            if (hit.collider.gameObject == this.gameObject)
            {
                return false;
            }

            // Jeder andere Collider blockiert
            return true;
        }

        return false;
    }



    private int toInt(bool b){
        return b ? 1 : 0;
    }
}
