using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public float speed = 5f;
    public bool devOut = false;
    public float growthRate = 0.5f;
    public float shrinkRate = 0.2f;
    public float maxWallDamagePercentage = 5f;
    public float maxSize = 10f;
    public float minSize = 1f;
    public bool canJump = false;
    public float jumpHeight = 750f;

    private float size;
    private Vector3 momentum = Vector3.zero;
    private Vector3 momentumVelocity = Vector3.zero;
    private GameObject PlayerManager;
    private bool isGrounded = false;
    private int shadowCounter = 0;
    private bool isInSafeArea = false;
    private UnityEngine.UI.Slider meltBar;
    private bool fastGrow = false;
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
        momentum = momentum * (fastGrow ? 0.9f : 1);


        //Größer werden
        float growthMultiplier = growthRate*Mathf.Pow(momentum.magnitude, 2f);
        if(isGrounded && !isInSafeArea && shadowCounter > 0)
        {
            transform.localScale += Vector3.one * growthMultiplier* Time.deltaTime;
        }
        //Kleiner werden
        if(shadowCounter <= 0 && !isInSafeArea){
            float shrinkMultiplier = 1.9f* (Mathf.Pow(size-3f, 2f)/(Mathf.Pow(size-3f, 2f)+8))*(1-0.625f*growthMultiplier);
            transform.localScale -= Vector3.one * shrinkMultiplier * Time.deltaTime;
        }




        //Canvas Slider anpassen
        meltBar.value = (size - minSize) / (maxSize - minSize) * 200 - 100;

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
            + " | Speed: " + momentum.magnitude
            + " | Size: " + size
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

            Vector3 globalHit = hit.collider.transform.TransformPoint(hit.point);
            Vector3 toHit = globalHit - transform.position;
            if(toHit.y < -(radius-0.05f))
            {
                // Boden unter uns, ignorieren
                continue;
            }

            
            //Wall Damage Berechnung
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            float hitSpeed = new Vector3(direction.x, 0, direction.z).magnitude;
            if(angle > 45f && hitSpeed > 0.4f && !isInSafeArea)
            {
                Debug.Log("Kollision mit " + hit.collider.name + " | Winkel: " + angle + " | Geschwindigkeit: " + hitSpeed);
                //je nach geschwindigkeit soo der ball zwischen 0-5% der aktuellen größe schrumpfen
                Debug.Log("Größe vor Kollision: " + (size * Vector3.one - Vector3.one * hitSpeed * (maxWallDamagePercentage / 100f) * size));
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
