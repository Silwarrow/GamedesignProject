using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    public float hookforce = 25f; // Steuert die Geschwindigkeit des Hook-Projektils, also wie schnell das wandert

    Grapple grapple;
    Rigidbody rigid;
    LineRenderer lineRenderer;
    Vector3 spawnPosition;
    float maxRange; // Reichweite des Grapplinghooks, wenn erreicht -> Hook zerstört

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Grapple grapple, Transform shootTransform, float maxRange)
    {
        this.grapple = grapple;
        this.maxRange = maxRange;
        this.spawnPosition = transform.position;
        rigid.AddForce(shootTransform.forward * hookforce, ForceMode.Impulse); // Der Hook wird Impuls-artig in die Richtung geschossen, in die der Spieler schaut
    }


    // Update is called once per frame
    void Update()
    {
        if (grapple == null)
        {
            return;
        }

        // Prüfe ob der Hook die maximale Reichweite überschritten hat ohne ein Grapple-Ziel zu treffen
        if (Vector3.Distance(transform.position, spawnPosition) > maxRange)
        {
            Destroy(gameObject);
            return;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, grapple.transform.position);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("grapple"))
        {
            if (grapple == null) return;
            grapple.StartPull();
        }
    }
}