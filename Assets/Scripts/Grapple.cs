using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Stellt sicher dass Rigidbody am Spieler existiert
public class Grapple : MonoBehaviour
{
    public float dashForce = 50f;
    public GameObject hookPrefab;
    public Transform shootTransform;
    public float maxRange = 50f;

    Hook hook;
    Rigidbody playerRigidbody;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hook == null && Input.GetMouseButtonDown(0)) //Linksklick -> Hook schießen
        {
            Vector3 screenPos = Input.mousePosition;
            if (TryResolveAimFromMouse(screenPos, out Vector3 finalAim))
            {
                shootTransform.LookAt(finalAim);
                hook = Instantiate(hookPrefab, shootTransform.position, shootTransform.rotation).GetComponent<Hook>(); // Hook-Objekt wird erstellt
                hook.Initialize(this, shootTransform, maxRange);
            }
        }
        else if (hook != null && Input.GetMouseButtonDown(1)) //Rechtsklick -> Hook zerstören bevor ankommt
        {
            DestroyHook();
        }
    }

    public void StartPull()
    {
        if (hook == null) return; //Falls der Hook schon zerstört wurde, bevor er ankommt, soll hier nichts passieren
        Vector3 dashDirection = (hook.transform.position - transform.position).normalized; //Richtung vom Spieler zum Hook, normalisiert damit es nur die Richtung ist ohne Stärke
        playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse); //Impulsartiger Dash wird verwendet
    }

    private void DestroyHook()
    {
        Destroy(hook.gameObject);
        hook = null;
    }

    

    private bool TryResolveAimFromMouse(Vector3 screenPos, out Vector3 aimPoint)
    {
        aimPoint = shootTransform.position; //Default-Wert, falls etwas schiefgeht damit der Hook nicht in die Luft geschossen wird

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
