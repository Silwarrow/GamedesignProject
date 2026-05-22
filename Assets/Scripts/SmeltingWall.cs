using UnityEngine;

public class SmeltingWall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject parent;
    private bool activated = false;
    private float meltspeed = 2.0f; // Set this value as needed

    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated){
            parent.transform.Translate(Vector3.down * meltspeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player"))
        {
            activated = true;
        }
    }
}
