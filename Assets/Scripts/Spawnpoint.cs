using UnityEngine;

public class Spawnpoint : MonoBehaviour
{

    public UnityEngine.Vector3 spawnPoint = new UnityEngine.Vector3(0, 1, 0);
    public UnityEngine.Quaternion spawnRotation = new UnityEngine.Quaternion(0, 0, 0, 1);
    public UnityEngine.Vector3 spawnScale = new UnityEngine.Vector3(1, 1, 1);

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
            RespawnControler respawnControler = playerManager.GetComponent<RespawnControler>();
            respawnControler.respawnPoint = spawnPoint;
            respawnControler.respawnRotation = spawnRotation;
            respawnControler.respawnScale = spawnScale;

            CharacterControler characterControler = other.GetComponent<CharacterControler>();
            respawnControler.sizeChange = characterControler.sizeChange;
            respawnControler.minSize = characterControler.minSize;
            respawnControler.maxSize = characterControler.maxSize;
            respawnControler.canJump = characterControler.canJump;
            respawnControler.jumpHeight = characterControler.jumpHeight;
        }
    }
}
