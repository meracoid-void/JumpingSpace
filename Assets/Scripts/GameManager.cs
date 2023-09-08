using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject powerUpPrefab;
    public GameObject GoombaPrefab;
    public GameObject platformPrefab;

    public bool isPlayerRespawning = false;
    public int playerLives = 3;

    private List<TransformData> initialPlatformData;  

    private List<Vector3> initialPowerUpPositions;
    private List<Vector3> initialNPCPositions;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (initialPlatformData == null || initialPlatformData.Count == 0)
        {
            initialPlatformData = new List<TransformData>();
            foreach (PlatformDissolve platform in FindObjectsOfType<PlatformDissolve>())
            {
                if (platform.isDissolvable)
                {
                    TransformData data = new TransformData
                    {
                        position = platform.transform.position,
                        scale = platform.transform.localScale,
                        shrinkSpeed = platform.shrinkSpeed
                    };
                    initialPlatformData.Add(data);
                }
            }
        }

        initialPowerUpPositions = new List<Vector3>();
        foreach (PowerUp powerUp in FindObjectsOfType<PowerUp>())
        {
            initialPowerUpPositions.Add(powerUp.transform.position);
        }

        initialNPCPositions = new List<Vector3>();
        foreach(GombaNPCBehavior gomba in FindObjectsOfType<GombaNPCBehavior>())
        {
            initialNPCPositions.Add(gomba.transform.position);
        }
    }

    public void ResetGameAssets()
    {
        // Reset Platforms
        foreach (PlatformDissolve platform in FindObjectsOfType<PlatformDissolve>())
        {
            if (platform.isDissolvable)
            {
                Destroy(platform.gameObject);
            }
        }

        // Instantiate new ones at the initial positions and scales
        foreach (TransformData data in initialPlatformData)
        {
            GameObject newPlatform = Instantiate(platformPrefab, data.position, Quaternion.identity);
            newPlatform.transform.localScale = data.scale;
            var newPlatData = newPlatform.GetComponent<PlatformDissolve>();
            newPlatData.isDissolvable = true;
            newPlatData.shrinkSpeed = data.shrinkSpeed;
        }

        // Reset Power-Ups
        // Destroy existing ones first
        foreach (PowerUp powerUp in FindObjectsOfType<PowerUp>())
        {
            Destroy(powerUp.gameObject);
        }

        // Instantiate new ones at the initial positions
        foreach (Vector3 pos in initialPowerUpPositions)
        {
            Instantiate(powerUpPrefab, pos, Quaternion.identity);
        }

        // Reset Gomba
        foreach (GombaNPCBehavior gomba in FindObjectsOfType<GombaNPCBehavior>())
        {
            Destroy(gomba.gameObject);
        }

        // Instantiate new ones at the initial positions
        foreach (Vector3 pos in initialNPCPositions)
        {
            Instantiate(GoombaPrefab, pos, Quaternion.identity);
        }
    }
}


public struct TransformData
{
    public Vector3 position;
    public Vector3 scale;
    public float shrinkSpeed;
}