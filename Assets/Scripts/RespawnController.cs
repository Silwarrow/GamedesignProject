using UnityEngine;

public class RespawnController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerPrefab;
    public GameObject camera;
    public Vector3 respawnPoint = new Vector3(0, 1, 0);
    public Quaternion respawnRotation = new Quaternion(0, 0, 0, 1);
    public Vector3 respawnScale = new Vector3(1, 1, 1);
    public float speed = 15f;
    public float growthRate = 0.2f;
    public float shrinkRate = 0.2f;
    public float minSize = 0.1f;
    public float maxSize = 10f;
    public bool canJump = false;
    public float jumpHeight = 5f;
    public float gravity = 25f;
    public float fallMultiplier = 2f;


    private void Awake() {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        camera = GameObject.Find("Main Camera");
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, respawnRotation);
        newPlayer.transform.localScale = respawnScale;
        CharacterController newController = newPlayer.GetComponent<CharacterController>();

        newController.speed = speed;
        newController.growthRate = growthRate;
        newController.minSize = minSize;
        newController.maxSize = maxSize;
        newController.canJump = canJump;
        newController.jumpHeight = jumpHeight;
        newController.gravity = gravity;
        newController.fallMultiplier = fallMultiplier;

        CameraController.Instance?.PlayerRespawned(newPlayer);
    }

}
