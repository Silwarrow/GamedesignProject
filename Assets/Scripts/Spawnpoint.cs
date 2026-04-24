using UnityEngine;

public class Spawnpoint : MonoBehaviour
{

    public UnityEngine.Vector3 spawnPoint = new UnityEngine.Vector3(0, 1, 0);

    private GameObject playerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerManager = GameObject.Find("PlayerManager");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager.GetComponent<RespawnControler>().respawnPoint = spawnPoint;
        }
    }
}
