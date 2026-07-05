using UnityEngine;

public class Hook : MonoBehaviour
{
    public float hookforce = 25f; //Steuert die Geschwindigkeit des Hook-Projektils

    Grapple grapple;
    Rigidbody rigid;
    LineRenderer lineRenderer;
    Vector3 spawnPosition; //Position, von der der Hook abgeschossen wurde
    float maxRange;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Grapple grapple, Transform shootTransform, float maxRange)
    {
        this.grapple = grapple;
        this.maxRange = maxRange;
        spawnPosition = transform.position;
        rigid.AddForce(shootTransform.forward * hookforce, ForceMode.Impulse); //Hook kriegt die Werte des Mausinputs für die Richtung
    }


    // Update is called once per frame
    void Update()
    {
        if (grapple == null)
        {
            return;
        }

        //Prüfe ob der Hook die maximale Reichweite überschritten hat ohne ein Grapple-Ziel zu treffen
        if (Vector3.Distance(transform.position, spawnPosition) > maxRange)
        {
            Destroy(gameObject);
            return;
        }

        lineRenderer.SetPosition(0, transform.position); //Passt den Startpunkt des LineRenderers an, damit der Anfang der Line nicht an der Position des Schusses bleibt, wenn der Spieler sich bewegt
        lineRenderer.SetPosition(1, grapple.transform.position); //Setzt den Endpunkt des LineRenderers auf die aktuelle Position des Grapple-Ziels
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("grapple") && grapple != null)
        {   
            Destroy(gameObject);
            grapple.StartPull();
            grapple.GetComponent<CharacterController>()?.ActivateFireResistance(grapple.fireResistanceDuration);
        }else if (other.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
    }
}