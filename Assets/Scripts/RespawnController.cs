using UnityEngine;

public class RespawnController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerPrefab;
    public Vector3 respawnPoint = new(0, 1, 0);
    public Quaternion respawnRotation = new(0, 0, 0, 1);
    public Vector3 respawnScale = new(1, 1, 1);
    public float speed = 15f;

    [Header("Size Limits")]
    public float growthRate = 0.2f;
    public float shrinkRate = 0.2f;
    public float minSize = 0.1f;
    public float maxSize = 10f;

    [Header("Jump Settings")]
    public bool canJump = false;
    public float jumpHeight = 5f;
    public float gravity = 25f;
    public float fallMultiplier = 2f;

    [Header("Grapple Settings")]
    public bool canGrapple = false;
    public float dashForce = 50f;
    public float maxRange = 50f;
    public float grappleCooldown = 5.0f;
    public float delayUntilDashBeginsStopping = 1f;
    public float dashStoppingProcessDuration = 0.35f;
    public float fireResistanceDuration = 5f;


    private void Start() {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Debug.Log($"RespawnController: RespawnPlayer aufgerufen. CameraController.Instance = {(CameraController.Instance != null ? "GEFUNDEN" : "NULL")}");
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        //sucht nach dem Hook, wenn gefunden -> nimmt das gesamte Objekt wo Hook dran ist und löscht es (also auch Linerenderer)
        Hook hook = Object.FindFirstObjectByType<Hook>();
        if (hook != null)
        {
            Destroy(hook.gameObject);
        }

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, respawnRotation);
        newPlayer.transform.localScale = respawnScale;
        CharacterController newController = newPlayer.GetComponent<CharacterController>();
        Grapple newGrapple = newPlayer.GetComponent<Grapple>();

        newController.speed = speed;
        newController.growthRate = growthRate;
        newController.minSize = minSize;
        newController.maxSize = maxSize;
        newController.canJump = canJump;
        newController.jumpHeight = jumpHeight;
        newController.gravity = gravity;
        newController.fallMultiplier = fallMultiplier;

        if (newGrapple != null)
        {
            newGrapple.dashForce = dashForce;
            newGrapple.canGrapple = canGrapple;
            newGrapple.maxRange = maxRange;
            newGrapple.grappleCooldown = grappleCooldown;
            newGrapple.delayUntilDashBeginsStopping = delayUntilDashBeginsStopping;
            newGrapple.dashStoppingProcessDuration = dashStoppingProcessDuration;
            newGrapple.fireResistanceDuration = fireResistanceDuration;
        }


        CameraController.Instance?.PlayerRespawned(newPlayer);
    }

}
