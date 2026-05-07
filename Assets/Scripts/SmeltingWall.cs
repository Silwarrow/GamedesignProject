using Unity.VisualScripting;
using UnityEngine;

public class SmeltingWall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject parent;
    private bool activated = false;
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated){
            parent.transform.Translate(Vector3.down * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("Object:"+other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            activated = true;
        }
    }
}
