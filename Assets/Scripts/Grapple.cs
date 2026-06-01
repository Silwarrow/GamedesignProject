using System.Collections;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [HideInInspector] public bool canGrapple = false;
    [HideInInspector] public float dashForce = 50f;
    public GameObject hookPrefab;
    public Transform shootTransform;
    [HideInInspector] public float maxRange = 50f;
    [HideInInspector] public float grappleCooldown = 5.0f;
    bool isCoolingDown = false;
    private CharacterController characterController;
    [HideInInspector] public float delayUntilDashBeginsStopping = 1f;
    [HideInInspector] public float dashStoppingProcessDuration = 0.35f;
    [HideInInspector] public float fireResistanceDuration = 5f;


    Hook hook;
    Rigidbody playerRigidbody;

    private float lockedY;
    private bool isYLocked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isYLocked)
        {
            Vector3 position = transform.position;
            position.y = lockedY;
            transform.position = position;

            Vector3 velocity = playerRigidbody.linearVelocity;
            playerRigidbody.linearVelocity = new Vector3(velocity.x, 0f, velocity.z);
        }

        if(hook == null && !isCoolingDown && Input.GetMouseButtonDown(0) && canGrapple) //Linksklick -> Hook schießen
        {
            Vector3 screenPos = Input.mousePosition;
            if (TryResolveAimFromMouse(screenPos, out Vector3 finalAim))
            {
                shootTransform.LookAt(finalAim);
                hook = Instantiate(hookPrefab, shootTransform.position, shootTransform.rotation).GetComponent<Hook>(); // Hook-Objekt wird erstellt
                hook.Initialize(this, shootTransform, maxRange);
                StartCoroutine(GrappleCooldownRoutine());
            }
        }
        else if (hook != null && Input.GetMouseButtonDown(1)) //Rechtsklick -> Hook zerstören bevor ankommt
        {
            DestroyHook();
        }
    }

    public void StartPull()
    {
        if (hook == null) return;
        Vector3 dashDirection = (hook.transform.position - transform.position).normalized; //Richtung vom Spieler zum Hook, normalisiert damit es nur die Richtung ist ohne Stärke
        playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse); //Impulsartiger Dash wird verwendet

        Vector3 curVel = playerRigidbody.linearVelocity;
        playerRigidbody.linearVelocity = new Vector3(curVel.x, 0f, curVel.z);

        lockedY = transform.position.y;
        isYLocked = true;
        StartCoroutine(DecayDashAfterDelay());
    }

    private void DestroyHook()
    {
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator GrappleCooldownRoutine()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(grappleCooldown);
        isCoolingDown = false;
    }
    public IEnumerator GrappleFireresistenceRoutine()
    {
        if (hook != null)
        {
            characterController?.DecrementFireCounter();
            yield return new WaitForSeconds(fireResistanceDuration);
            characterController?.IncrementFireCounter();   
        }
    }
    private IEnumerator DecayDashAfterDelay()
    {
        yield return new WaitForSeconds(delayUntilDashBeginsStopping);
        Vector3 startVelocity = playerRigidbody.linearVelocity;
        float elapsed = 0f;

        while (elapsed < dashStoppingProcessDuration)
        {   
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dashStoppingProcessDuration);
            float easedT = t * t;
            Vector3 lerped = Vector3.Lerp(startVelocity, Vector3.zero, easedT);
            playerRigidbody.linearVelocity = new Vector3(lerped.x, 0f, lerped.z);
            yield return null;
        }
        isYLocked = false;
    }

    

    private bool TryResolveAimFromMouse(Vector3 screenPos, out Vector3 aimPoint)
    {
        aimPoint = shootTransform.position;

        Ray camRay = Camera.main.ScreenPointToRay(screenPos); //Erstellt einen Ray basierend auf geklickten Punkt der genutzten Kamera
        Plane playerPlane = new(Vector3.up, shootTransform.position); //Erstellt eine horizontale Ebene auf Höhe des Schusses, damit der Spieler auch auf Wände zielen kann, die sich nicht auf der gleichen Höhe befinden, und damit der Hook nicht in die Luft geschossen wird, wenn auf den Boden gezielt wird
        
        if (playerPlane.Raycast(camRay, out float planeDist))
        {
            Vector3 dir = camRay.GetPoint(planeDist) - shootTransform.position; //Richtung vom Schuss zum Zielpunkt
            dir.Normalize(); //Normalisiert, damit es nur die Richtung ist ohne Stärke
            aimPoint = shootTransform.position + dir * maxRange; //Der endgültige Zielpunkt, auf den der Hook geschossen wird, basierend auf der Richtung und der maximalen Reichweite, damit der Hook nicht unendlich weit fliegt, wenn auf einen Punkt gezielt wird, der weiter als die maximale Reichweite entfernt ist
            return true;
        }

        return false;
    }
}
