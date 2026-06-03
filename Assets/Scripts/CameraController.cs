using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

/// Kamerasystem mit Spline-Rail und offenen Bereichen.
///
/// Scene-Aufbau:
///   - Alle Spline-Objekte bekommen den Tag "CamRail" + SplineContainer Component
///   - Alle offenen Bereiche bekommen den Tag "CamZone" + Collider (Is Trigger = true)
///   - Dieses Script liegt auf der Hauptkamera
///   - Der Spieler hat den Tag "Player"
public class CameraController : MonoBehaviour
{
    // Enums
    public enum CameraMode
    {
        Rail,
        ToOpen,
        ToRail,
        Open
    }

    // Felder – Referenzen

    private SplineContainer[] allRails;
    private Transform player;

    // Felder – Zustand
    private CameraMode mode = CameraMode.Rail;
    private SplineContainer activeRail;

    // Felder – Blend
    [Header("Blend")]
    [Tooltip("Dauer eines Übergangs in Sekunden")]
    public float blendDuration = 1.2f;

    [Tooltip("Sperrzeit nach einem Übergang, verhindert schnelles Flippen")]
    public float blendCooldownDuration = 0.5f;

    private float blendT = 0f;
    private float blendCooldown = 0f;

    private Vector3 cachedPositionA;
    private Quaternion cachedRotationA;

    // Felder – Kameraverhalten
    [Header("Kamera")]
    [Tooltip("Abstand der Kamera zum Spieler")]
    public float camDistance = 80f;

    [Tooltip("Vertikaler Winkel in Grad (60 = von oben)")]
    public float camAngleDegrees = 60f;

    [Header("Rail")]
    [Tooltip("Geschwindigkeit mit der die Kamera dem Rail-Punkt folgt")]
    public float railFollowSpeed = 40f;

    [Header("Rail Suche")]
    [Tooltip("Wie weit hinter dem Spieler der Suchpunkt liegt")]
    public float railSearchOffsetBack = 40f;

    [Tooltip("Wie weit über dem Spieler der Suchpunkt liegt")]
    public float railSearchOffsetUp = 100f;

    // Awake – Initialisierung

    public static CameraController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        // Alle Rails suchen
        GameObject[] railObjects = GameObject.FindGameObjectsWithTag("CamRail");
        var railList = new List<SplineContainer>();
        foreach (var obj in railObjects)
        {
            SplineContainer sc = obj.GetComponent<SplineContainer>();
            if (sc != null)
                railList.Add(sc);
            else
                Debug.LogWarning($"CameraController: '{obj.name}' hat Tag 'CamRail' aber keinen SplineContainer.");
        }
        allRails = railList.ToArray();
        Debug.Log($"CameraController: {allRails.Length} Rail(s) gefunden.");

        if (allRails.Length == 0)
            Debug.LogWarning("CameraController: Keine Rails mit Tag 'CamRail' gefunden.");


        // Startposition setzen
        if (activeRail != null)
        {
            transform.position = GetNearestPointOnRail(activeRail);
            transform.rotation = ComputeRotation(transform.position);
        }
    }

    // LateUpdate – Hauptlogik

    private void LateUpdate()
    {
        if (player == null) return;

        // Übergangs-Cooldown runterzählen
        if (blendCooldown > 0f)
            blendCooldown -= Time.deltaTime;

        // Aktive Rail immer aktualisieren (nächste Spline zum Spieler)
        UpdateActiveRail();

        switch (mode)
        {
            case CameraMode.Rail:
                HandleRail();
                break;
            case CameraMode.ToOpen:
                HandleToOpen();
                break;
            case CameraMode.ToRail:
                HandleToRail();
                break;
            case CameraMode.Open:
                HandleOpen();
                break;
        }
    }

    // Modus-Handler

    private void HandleRail()
    {
        if (activeRail == null) return;
        
        Vector3 targetPos = GetNearestPointOnRail(activeRail);
    
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            railFollowSpeed * Time.deltaTime
        );
    
        transform.rotation = ComputeRotation(transform.position);
    }

    private void HandleToOpen()
    {
        blendT += Time.deltaTime / blendDuration;
        blendT = Mathf.Clamp01(blendT);

        float smooth = Mathf.SmoothStep(0f, 1f, blendT);

        Vector3 openPos = ComputeOpenPosition();
        Quaternion openRot = ComputeRotation(openPos);

        transform.position = Vector3.Lerp(cachedPositionA, openPos, smooth);
        transform.rotation = Quaternion.Slerp(cachedRotationA, openRot, smooth);

        if (blendT >= 1f)
            mode = CameraMode.Open;
    }

    private void HandleToRail()
    {
        blendT += Time.deltaTime / blendDuration;
        blendT = Mathf.Clamp01(blendT);

        float smooth = Mathf.SmoothStep(0f, 1f, blendT);

        Vector3 railPos = activeRail != null ? GetNearestPointOnRail(activeRail) : transform.position;
        Quaternion railRot = ComputeRotation(railPos);

        transform.position = Vector3.Lerp(cachedPositionA, railPos, smooth);
        transform.rotation = Quaternion.Slerp(cachedRotationA, railRot, smooth);

        if (blendT >= 1f)
            mode = CameraMode.Rail;
    }

    private void HandleOpen()
    {
        Vector3 openPos = ComputeOpenPosition();
        transform.position = openPos;
        transform.rotation = ComputeRotation(openPos);
    }

    // Hilfsmethoden – Kameraberechnung

    /// Berechnet die Kameraposition für den offenen Modus:
    /// Direkt hinter dem Spieler (keine seitliche Rotation),
    /// camAngleDegrees über dem Spieler, camDistance entfernt.
    private Vector3 ComputeOpenPosition()
    {
        float angleRad = camAngleDegrees * Mathf.Deg2Rad;
        // Kamera ist direkt "hinter" dem Spieler in Weltkoordinaten (keine Yaw-Rotation)
        Vector3 offset = new Vector3(
            0f,
            Mathf.Sin(angleRad) * camDistance,
            -Mathf.Cos(angleRad) * camDistance
        );
        return player.position + offset;
    }


    /// Berechnet die Rotation so dass die Kamera von gegebener Position
    /// auf den Spieler schaut, mit Euler X = camAngleDegrees (nach unten geneigt),
    /// Y = 0 (keine seitliche Drehung), Z = 0.
    private Quaternion ComputeRotation(Vector3 fromPosition)
    {
        Vector3 direction = player.position - fromPosition;
        if (direction == Vector3.zero)
            return Quaternion.Euler(camAngleDegrees, 0f, 0f);

        Quaternion lookRot = Quaternion.LookRotation(direction);
        // Y und Z auf 0 lassen, nur X-Neigung verwenden
        Vector3 euler = lookRot.eulerAngles;
        return Quaternion.Euler(euler.x, 0f, 0f);
    }

    // Hilfsmethoden – Spline

    /// Findet die nächste Spline zum Spieler und setzt activeRail.
    private void UpdateActiveRail()
    {
    if (allRails == null || allRails.Length == 0) return;
    if (player == null) return;

    // Suchpunkt hinter und über dem Spieler
    Vector3 searchPoint = player.position
        + Vector3.up * railSearchOffsetUp
        + Vector3.back * railSearchOffsetBack;

    float bestDist = float.MaxValue;
    SplineContainer best = null;

    //Allen Rails durchgehen und die nächste zum Suchpunkt finden
    foreach (var rail in allRails)
    {
        if (rail == null) continue;
        Vector3 nearest = GetNearestPointOnRailFromPoint(rail, searchPoint);
        float dist = Vector3.SqrMagnitude(nearest - searchPoint);
        if (dist < bestDist)
        {
            bestDist = dist;
            best = rail;
        }
    }

    activeRail = best;
}

    /// Gibt den nächsten Punkt auf einer Spline zum Spieler zurück (Weltkoordinaten).
    private Vector3 GetNearestPointOnRail(SplineContainer splineContainer)
    {
        return GetNearestPointOnRailFromPoint(splineContainer, player.position + new Vector3(0f, railSearchOffsetUp, -railSearchOffsetBack));
    }
    
    private Vector3 GetNearestPointOnRailFromPoint(SplineContainer splineContainer, Vector3 worldPoint)
    {
        Vector3 localPoint = splineContainer.transform.InverseTransformPoint(worldPoint);
    
        SplineUtility.GetNearestPoint(
            splineContainer.Spline,
            (float3)localPoint,
            out float3 nearestLocal,
            out float t
        );
    
        return splineContainer.transform.TransformPoint((Vector3)nearestLocal);
    }

    // Trigger – Zonenwechsel

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("CamZone")) return;
        if (blendCooldown > 0f) return;

        if (mode == CameraMode.Rail || mode == CameraMode.ToRail)
        {
            cachedPositionA = transform.position;
            cachedRotationA = transform.rotation;
            blendT = 0f;
            blendCooldown = blendCooldownDuration;
            mode = CameraMode.ToOpen;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("CamZone")) return;
        if (blendCooldown > 0f) return;

        if (mode == CameraMode.Open || mode == CameraMode.ToOpen)
        {
            cachedPositionA = transform.position;
            cachedRotationA = transform.rotation;
            blendT = 0f;
            blendCooldown = blendCooldownDuration;
            mode = CameraMode.ToRail;
        }
    }

    public void PlayerRespawned(GameObject newPlayer)
    {
        player = newPlayer.transform;


        // Nächste Rail suchen und Kamera sofort hinbringen
        UpdateActiveRail();
        if (activeRail != null)
        {
            transform.position = GetNearestPointOnRail(activeRail);
            transform.rotation = ComputeRotation(transform.position);
        }
    

        // Zustand zurücksetzen
        mode = CameraMode.Rail;
        blendT = 0f;
        blendCooldown = 0f;
    }

    // Gizmos – Scene-View Debugging
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (player == null) return;

        // Aktuelle Kameraposition
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);

        // Linie zum Spieler
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.position);

        // Offene Position vorschau
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(ComputeOpenPosition(), 0.3f);
        }
    }
#endif
}