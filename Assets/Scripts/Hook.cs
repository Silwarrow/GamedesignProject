using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    [SerializeField] float hookforce = 25f;
    [SerializeField] string grappleTag = "grapple";

    Grapple grapple;
    Rigidbody rigid;
    LineRenderer lineRenderer;
    Vector3 spawnPosition;
    float maxRange;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Grapple grapple, Transform shootTransform, float maxRange)
    {
        transform.forward = shootTransform.forward;
        this.grapple = grapple;
        this.maxRange = maxRange;
        this.spawnPosition = transform.position;
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.AddForce(transform.forward * hookforce, ForceMode.Impulse);   
    }


    // Update is called once per frame
    void Update()
    {
        if(grapple == null || lineRenderer == null)
        {
            return;
        }

        // Check if hook exceeded max range without hitting a grapple target
        if (Vector3.Distance(transform.position, spawnPosition) > maxRange)
        {
            Destroy(gameObject);
            return;
        }

        Vector3[] positions = new Vector3[]
        {
            transform.position,
            grapple.transform.position
        };
        lineRenderer.SetPositions(positions);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(grappleTag))
        {
            if (grapple == null) return;
            grapple.StartPull();
        }
    }
}