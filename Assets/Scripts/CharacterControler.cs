using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float speed = 5f;
    public bool devOut = false;
    public float sizeChange = 0f;
    public float lightSizeChange = 0f;
    public float maxSize = 10f;
    public float minSize = 0.1f;
    public bool canJump = false;
    public float jumpHeight = 750f;
    public float shadowRayDistance = 50f;
    public Transform sunLight;
    public float groundProbeDistance = 3f;
    public float shadowSampleRadius = 0.4f;
    public int shadowSampleCount = 5;
    public float shadowCoverageRequired = 0.5f;
    public LayerMask groundMask = ~0;
    public LayerMask shadowBlockerMask = ~0;
    public float shadowRayStartOffset = 0.05f;

    private float size;
    private Vector3 momentum = Vector3.zero;
    private Vector3 momentumVelocity = Vector3.zero;
    private GameObject PlayerManager;
    private bool isGrounded = false;
    private bool isInShadow = false;
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

        isInShadow = IsInShadow();

        bool isMovingOnPlane = (Mathf.Abs(momentum.x) >= 0.1f || Mathf.Abs(momentum.z) >= 0.1f) && isGrounded;
        float sizeDelta = 0f;

        //Außerhalb vom Schatten immer schrumpfen (auch im Stand)
        if (!isInShadow)
        {
            sizeDelta -= lightSizeChange / 100f * Time.deltaTime;
        }
        //Im Schatten nur beim Bewegen wachsen
        else if (isMovingOnPlane)
        {
            sizeDelta += sizeChange / 100f * momentum.magnitude * Time.deltaTime;
        }

        if (sizeDelta != 0f)
        {
            float nextSize = size + sizeDelta;
            transform.localScale = new Vector3(nextSize, nextSize, nextSize);
            size = nextSize;
        }

        //Sprungfähigkeit testen
        isGrounded = Physics.Raycast(transform.position, Vector3.down, size/2 + 0.1f);

        //Springen
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            momentum.y = jumpHeight/Mathf.Sqrt(size);
            isGrounded = false;
        }

        //Bewegen
        transform.Translate(momentum * speed* Time.deltaTime);

        //Tod nach Größe
        if(size < minSize || size > maxSize){
            PlayerManager.GetComponent<RespawnControler>().RespawnPlayer();
            Destroy(gameObject);
        }

        //Dev output
        if (devOut)
        {
            Debug.Log("Momentum: " + momentum + " IsInShadow: " + isInShadow + " Size: " + size);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Falls Todeszone betreten, Spieler respawnen und aktuelles Objekt zerstören
        if (other.CompareTag("DeathArea"))
        {
            PlayerManager.GetComponent<RespawnControler>().RespawnPlayer();
            Destroy(gameObject);
        }
    }

    private int toInt(bool b){
        return b ? 1 : 0;
    }

    private bool IsInShadow()
    {
        Vector3 probeStart = transform.position + Vector3.up * shadowRayStartOffset;
        RaycastHit groundHit;

        if (!Physics.Raycast(probeStart, Vector3.down, out groundHit, groundProbeDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        Vector3 lightDir = Vector3.up;
        if (sunLight != null)
        {
            lightDir = -sunLight.forward.normalized;
        }

        Vector3 sampleBase = groundHit.point + groundHit.normal * 0.03f;
        int blockedSamples = 0;
        int totalSamples = Mathf.Max(1, shadowSampleCount);

        for (int i = 0; i < totalSamples; i++)
        {
            Vector3 sampleOffset = Vector3.zero;
            if (i > 0)
            {
                float angle = (i - 1) * Mathf.PI * 2f / Mathf.Max(1, totalSamples - 1);
                sampleOffset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * shadowSampleRadius;
            }

            Vector3 sampleOrigin = sampleBase + sampleOffset;
            if (Physics.Raycast(sampleOrigin, lightDir, shadowRayDistance, shadowBlockerMask, QueryTriggerInteraction.Ignore))
            {
                blockedSamples++;
            }
        }

        float coverage = (float)blockedSamples / totalSamples;
        return coverage >= shadowCoverageRequired;
    }
}
