using UnityEngine;

public class Collectable : MonoBehaviour
{

    public int level;
    public int position;
    
    private CollectableManager collectableManager;
    private Item myself;

    void Awake()
    {
        //collectableManager ist eine component im object name PlayerManager
        collectableManager = GameObject.Find("PlayerManager").GetComponent<CollectableManager>();
        myself = collectableManager.GetCollectable(level, position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (myself != null)
            {
                collectableManager.setCollected(myself);
            }
            gameObject.SetActive(false);
        }
        
    }
}
