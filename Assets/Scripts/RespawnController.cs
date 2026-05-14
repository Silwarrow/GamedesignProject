using UnityEngine;

public class RespawnController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerPrefab;
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
    private void Awake() {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        // destroy any existing hooks in the scene to avoid orphaned hooks after respawn
        foreach (var h in Object.FindObjectsByType<Hook>(FindObjectsSortMode.None))
        {
            if (h != null && h.gameObject != null)
                Destroy(h.gameObject);
        }

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, respawnRotation);
        newPlayer.transform.localScale = respawnScale;
        CharacterController newController = newPlayer.GetComponent<CharacterController>();

        newController.speed = speed;
        newController.growthRate = growthRate;
        newController.shrinkRate = shrinkRate;
        newController.minSize = minSize;
        newController.maxSize = maxSize;
        newController.canJump = canJump;
        newController.jumpHeight = jumpHeight;
    }

}
