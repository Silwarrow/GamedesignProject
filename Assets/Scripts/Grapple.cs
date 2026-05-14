using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class Grapple : MonoBehaviour
{
    [SerializeField] float pullSpeed = 0.5f;
    [SerializeField] float stopDistance = 4f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;

    Hook hook;
    bool pulling;
    Rigidbody rigit;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigit = GetComponent<Rigidbody>();
        pulling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hook == null && Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            pulling = false;
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            hook.Initialize(this, shootTransform);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        else if (hook != null && Input.GetMouseButtonDown(1))
        {
            DestroyHook();
        }
        

        if (!pulling || hook == null) return;
        
        if(Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        {
            DestroyHook();
        }
        else
        {
            rigit.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
        }
    }
    public void StartPull()
    {
        pulling = true;
    }

    private void DestroyHook()
    {
        if (hook == null) return;
        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(8f);
        DestroyHook();
    }
}
