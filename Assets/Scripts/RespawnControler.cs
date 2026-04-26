using UnityEngine;

public class RespawnControler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerPrefab;
    public Vector3 respawnPoint = new Vector3(0, 1, 0);
    public Quaternion respawnRotation = new Quaternion(0, 0, 0, 1);
    public Vector3 respawnScale = new Vector3(1, 1, 1);
    public float sizeChange = 0.2f;
    public float lightSizeChange = 0.2f;
    public float minSize = 0.1f;
    public float maxSize = 10f;
    public bool canJump = false;
    public float jumpHeight = 5f;
    public float shadowRayDistance = 50f;
    public Transform sunLight;
    public float groundProbeDistance = 3f;
    public float shadowSampleRadius = 0.4f;
    public int shadowSampleCount = 5;
    public float shadowCoverageRequired = 0.5f;
    public LayerMask groundMask = ~0;
    public LayerMask shadowBlockerMask = ~0;
    public float shadowRayStartOffset = 0.05f;
    private void Awake() {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, respawnRotation);
        newPlayer.transform.localScale = respawnScale;
        CharacterControler newController = newPlayer.GetComponent<CharacterControler>();

        newController.sizeChange = sizeChange;
        newController.lightSizeChange = lightSizeChange;
        newController.minSize = minSize;
        newController.maxSize = maxSize;
        newController.canJump = canJump;
        newController.jumpHeight = jumpHeight;
        newController.shadowRayDistance = shadowRayDistance;
        newController.sunLight = sunLight;
        newController.groundProbeDistance = groundProbeDistance;
        newController.shadowSampleRadius = shadowSampleRadius;
        newController.shadowSampleCount = shadowSampleCount;
        newController.shadowCoverageRequired = shadowCoverageRequired;
        newController.groundMask = groundMask;
        newController.shadowBlockerMask = shadowBlockerMask;
        newController.shadowRayStartOffset = shadowRayStartOffset;
    }

}
