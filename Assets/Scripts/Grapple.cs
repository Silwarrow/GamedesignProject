using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grapple : MonoBehaviour
{
    [SerializeField] float dashForce = 120f;
    [SerializeField] float hookLifetime = 2f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;
    [SerializeField] float maxRange = 30f;
    [SerializeField] float passDistanceThreshold = 1.5f;

    Hook hook;
    Rigidbody playerRigidbody;
    bool isDashing = false;
    float dashStartDistance = 0f;


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
            Vector3 screenPos = Input.mousePosition;
            if (TryResolveAimFromMouse(screenPos, out Vector3 finalAim))
            {
                StopAllCoroutines();
                shootTransform.LookAt(finalAim);
                hook = Instantiate(hookPrefab, shootTransform.position, shootTransform.rotation).GetComponent<Hook>();
                hook.Initialize(this, shootTransform, maxRange);
                StartCoroutine(DestroyHookAfterLifetime());
            }
        }
        else if (hook != null && Input.GetMouseButtonDown(1))
        {
            DestroyHook();
        }

        // Check if player has moved past the hook during dash phase
        if (isDashing && hook != null)
        {
            float currentDistance = Vector3.Distance(transform.position, hook.transform.position);
            if (currentDistance > dashStartDistance * passDistanceThreshold)
            {
                DestroyHook();
                isDashing = false;
            }
        }
    }

    public void StartPull()
    {
        if (hook == null) return;

        Vector3 dashDirection = (hook.transform.position - transform.position).normalized;
        playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        // Start distance-based despawn: don't destroy immediately
        isDashing = true;
        dashStartDistance = Vector3.Distance(transform.position, hook.transform.position);
    }

    private void DestroyHook()
    {
        if(hook == null) return;
        Destroy(hook.gameObject);
        hook = null;
        isDashing = false;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(hookLifetime);
        DestroyHook();
    }

    private bool TryResolveAimFromMouse(Vector3 screenPos, out Vector3 aimPoint)
    {
        aimPoint = shootTransform.position;
        Camera cam = Camera.main;
        if (cam == null) return false;

        // Option 1 (finalized): project camera ray to player-height plane,
        // cast player-origin ray toward that point with obstacle detection.
        Ray camRay = cam.ScreenPointToRay(screenPos);
        Plane playerPlane = new Plane(Vector3.up, shootTransform.position);
        
        if (playerPlane.Raycast(camRay, out float planeDist))
        {
            Vector3 projectedPoint = camRay.GetPoint(planeDist);
            Vector3 dir = (projectedPoint - shootTransform.position);
            if (dir.sqrMagnitude <= 0.0001f) return false;
            dir.Normalize();

            if (Physics.Raycast(shootTransform.position, dir, out RaycastHit hit, maxRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider != null && !hit.collider.CompareTag("Player"))
                {
                    aimPoint = hit.point;
                    return true;
                }
            }

            aimPoint = shootTransform.position + dir * maxRange;
            return true;
        }

        return false;
    }
}
