using UnityEngine;

public class RespawnControler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerPrefab;
    public Vector3 respawnPoint = new Vector3(0, 1, 0);
    public Quaternion respawnRotation = new Quaternion(0, 0, 0, 1);
    public Vector3 respawnScale = new Vector3(1, 1, 1);

    private void Awake() {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
    }
    public void RespawnPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, respawnRotation);
        newPlayer.transform.localScale = respawnScale;
    }

}
