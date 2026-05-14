using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grapple : MonoBehaviour
{
    [SerializeField] float dashForce = 120f;
    [SerializeField] float hookLifetime = 2f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;

    Hook hook;
    Rigidbody playerRigidbody;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        if(shootTransform == null)
        {
            shootTransform = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hook == null && Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            hook = Instantiate(hookPrefab, shootTransform.position, shootTransform.rotation).GetComponent<Hook>();
            hook.Initialize(this, shootTransform);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        else if (hook != null && Input.GetMouseButtonDown(1))
        {
            DestroyHook();
        }
    }

    public void StartPull()
    {
        if (hook == null) return;

        Vector3 dashDirection = (hook.transform.position - transform.position).normalized;
        playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        DestroyHook();
    }

    private void DestroyHook()
    {
        if(hook == null) return;
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(hookLifetime);
        DestroyHook();
    }
}
