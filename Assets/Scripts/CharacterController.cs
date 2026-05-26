using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{

    //public Variables
    public float speed = 30f;
    public bool devOut = false;
    public float maxWallDamagePercentage = 5f;
    
    [Header("Size Limits")]
    public float growthRate = 0.8f;
    public float maxSize = 13f;
    public float minSize = 3f;

    [Header("Jump Settings")]
    public bool canJump = false;
    public float jumpHeight = 27f;
    public float gravity = 32f;
    public float fallMultiplier = 2f;

    private float size;
    private Vector3 momentum = Vector3.zero;
    private Vector3 momentumVelocity = Vector3.zero;
    private float verticalVelocity = 0f;
    private bool isGrounded = false;
    
    //Scene Objects
    private UnityEngine.UI.Slider meltBar;
    private GameObject PlayerManager;
    //Area Tags
    private bool fastGrow = false;
    private bool isInSafeArea = false;
    private int shadowCounter = 0;
    private int waterCounter = 0;
    private bool waterSpeed = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        PlayerManager = FindFirstObjectByType<RespawnController>().gameObject;
        meltBar = FindFirstObjectByType<Canvas>().GetComponentInChildren<UnityEngine.UI.Slider>();

    }

    // Update is called once per frame
    void Update(){

        //Bewegungsrichtung berechnen
        Vector3 movement = new Vector3( toInt(Input.GetKey(KeyCode.D)) - toInt(Input.GetKey(KeyCode.A)), 0, 
                                        toInt(Input.GetKey(KeyCode.W)) - toInt(Input.GetKey(KeyCode.S)));

        //Smooth movement
        float x = Mathf.InverseLerp(3f, 13f, size);
        //inetria ist eine Funktion die zwischen den werten 0 und 1 die werte 0.2 und 3.7 ist und in dem bereich strengmonoton wachsend und konvex ist.
        float inertia = 0.2f + 3.5f * Mathf.Pow(x, 2.9f);
        momentum = Vector3.SmoothDamp(momentum, movement.normalized, ref momentumVelocity, inertia);

        //Bremsen bei Kollision
        if(IsColliderInFront(momentum)){
            momentum = Vector3.zero;
        }


        //Größer werden
        float growthMultiplier = growthRate*Mathf.Pow(momentum.magnitude, 2f) * (fastGrow ? 1.5f : 1f);
        if(isGrounded && !isInSafeArea && shadowCounter > 0)
        {
            transform.localScale += Vector3.one * growthMultiplier* Time.deltaTime;
        }
        //Kleiner werden
        if(shadowCounter <= 0 && !isInSafeArea){
            float shrinkMultiplier = 0.3f + 1.691f* (Mathf.Pow(size-3f, 2f)/(Mathf.Pow(size-3f, 2f)+13.7f))*(1-0.625f*growthMultiplier);
            transform.localScale -= Vector3.one * shrinkMultiplier * (fastGrow ? 0.5f : 1f) * Time.deltaTime;
        }




        //Canvas Slider anpassen
        meltBar.value = (size - minSize) / (maxSize - minSize) * 200 - 100;

        //Sprungfähigkeit testen: QueryTriggerInteraction.Ignore heißt, dass Trigger, also die shadow areas und safe zones, sowas halt, ignoriert werden.
        //Physics.DefaultRaycastLayers nutzt die Standard-Layer-Maske von Unity, nicht wichtig für uns, aber sonst kann ich die Methode nicht überladen um den Triggerignore zu verwenden.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, size / 2 + 0.9f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        //Springen
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            verticalVelocity = jumpHeight;
            isGrounded = false;
        }
        
        if(isGrounded && waterCounter <= 0)
        {
            waterSpeed = false;
            Debug.Log("On Ground");
        }else if(waterCounter > 0 && !waterSpeed)
        {
            waterSpeed = true;
            Debug.Log("In Water");
        }

        // Gravity: Gravitation wird stärker, wenn der Spieler fällt
        if (!isGrounded)
        {
            float appliedGravity = gravity * (verticalVelocity < 0f ? fallMultiplier : 1f);
            verticalVelocity -= appliedGravity * Time.deltaTime;
        }
        else if (verticalVelocity < 0f)
        {
            verticalVelocity = 0f;
        }



        //Bewegen
        Vector3 horizontalMovement = momentum * speed * Time.deltaTime * 
                                    (fastGrow ? 0.5f : 1f) * 
                                    (waterSpeed ? 1.7f : 1f);
                                    Debug.Log(horizontalMovement);
        Vector3 verticalMovement = Vector3.up * verticalVelocity * Time.deltaTime;
        transform.Translate(horizontalMovement + verticalMovement, Space.World);



        //Tod nach Größe
        if(size < minSize || size > maxSize){
            PlayerManager.GetComponent<RespawnController>().RespawnPlayer();
            Destroy(gameObject);
        }

        //Dev output
        if (devOut)
        {
            Debug.Log("Momentum: " + momentum
            + " | Speed: " + momentum.magnitude
            + " | Size: " + size
            + " | Vertical Velocity: " + verticalVelocity
            + " | Grounded: " + isGrounded
            + " | Shadow Counter: " + shadowCounter
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
            shadowCounter++;
        }
        if (other.CompareTag("SafeArea")){
            isInSafeArea = true;
        }
        if(other.CompareTag("ThickSnow")){
            fastGrow = true;
            gravity = 80f;
        }
        if(other.CompareTag("Water")){
            waterCounter++;
        }
    }
    void OnTriggerExit(Collider other){
        if (other.CompareTag("Shadow")){
            shadowCounter--;
        }
        if (other.CompareTag("SafeArea")){
            isInSafeArea = false;
        }
        if(other.CompareTag("ThickSnow")){
            fastGrow = false;
            gravity = 38f;
        }
        if(other.CompareTag("Water")){
            waterCounter--;
        }
    }

    bool IsColliderInFront(Vector3 direction)
    {
        size = transform.localScale.x;
        float radius = size / 2f;
        float castDistance = (speed * Time.deltaTime) + 0.05f; // kleine Sicherheitsreserve
        Vector3 origin = transform.position + direction.normalized * 0.01f; // nicht im eigenen Collider starten

        RaycastHit[] hits = Physics.SphereCastAll(
            origin,
            radius,
            direction.normalized,
            castDistance,
            ~0,
            QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
        {
            // Spieler selbst ignorieren
            if (hit.collider.gameObject == this.gameObject)
            {
                continue;
            }

            Vector3 toHit = hit.point - transform.position;
            float verticalThreshold = radius * 0.6f;
            if (toHit.y < -verticalThreshold)
            {
                continue;
            }

            
            //Wall Damage Berechnung
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            float hitSpeed = new Vector3(direction.x, 0, direction.z).magnitude;
            if(angle > 45f && hitSpeed > 0.4f && !isInSafeArea)
            {
                //je nach geschwindigkeit soo der ball zwischen 0-5% der aktuellen größe schrumpfen
                transform.localScale -= Vector3.one * hitSpeed * (maxWallDamagePercentage / 100f) * size;
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
