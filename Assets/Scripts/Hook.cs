using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    [SerializeField] float hookforce = 25f;
    [SerializeField] string grappleTag = "grapple";

    Grapple grapple;
    Rigidbody rigid;
    LineRenderer lineRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Grapple grapple, Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        this.grapple = grapple;
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.AddForce(transform.forward * hookforce, ForceMode.Impulse);   
    }


    // Update is called once per frame
    void Update()
    {
        if (grapple == null || lineRenderer == null)
        {
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
        if (other.CompareTag(grappleTag))
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;

            if (grapple == null) return;
            grapple.StartPull();
        }
    }
}