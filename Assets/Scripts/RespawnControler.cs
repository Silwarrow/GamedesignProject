using System.Numerics;
using Unity.Android.Gradle.Manifest;
using UnityEditor.VersionControl;
using UnityEngine;

public class RespawnControler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerPrefab;
    public UnityEngine.Vector3 respawnPoint = new UnityEngine.Vector3(0, 1, 0);
    public UnityEngine.Quaternion respawnRotation = new UnityEngine.Quaternion(0, 0, 0, 1);
    public UnityEngine.Vector3 respawnScale = new UnityEngine.Vector3(1, 1, 1);

    private void Awake() {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
    }
    public void RespawnPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint, respawnRotation);
        newPlayer.transform.localScale = respawnScale;
    }

}
