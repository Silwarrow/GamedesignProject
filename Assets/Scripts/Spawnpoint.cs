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
            RespawnController respawnController = playerManager.GetComponent<RespawnController>();
            respawnController.respawnPoint = spawnPoint;
            respawnController.respawnRotation = spawnRotation;
            respawnController.respawnScale = spawnScale;

            CharacterController characterController = other.GetComponent<CharacterController>();
            respawnController.growthRate = characterController.growthRate;
            respawnController.minSize = characterController.minSize;
            respawnController.maxSize = characterController.maxSize;
            respawnController.canJump = characterController.canJump;
            respawnController.jumpHeight = characterController.jumpHeight;

            Grapple grapple = other.GetComponent<Grapple>();
            if (grapple != null)
            {
                respawnController.canGrapple = grapple.canGrapple;
            }
        }
    }
}
