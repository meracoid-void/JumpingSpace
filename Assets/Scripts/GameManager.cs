using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject powerUpPrefab;
    public GameObject GoombaPrefab;
    public GameObject SmartGoombaPrefab;
    public GameObject platformPrefab;

    public bool isPlayerRespawning = false;
    public int playerLives = 3;


    private List<NPCData> SmartGoombaPositions;

    private List<TransformData> initialPlatformData;  

    private List<PowerUpData> initialPowerUpPositions;
    private List<NPCData> initialNPCPositions;

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

        initialPowerUpPositions = new List<PowerUpData>();
        foreach (PowerUp powerUp in FindObjectsOfType<PowerUp>())
        {
            PowerUpData data = new PowerUpData
            {
                position = powerUp.transform.position,
                isCheckpoint = powerUp.isCheckpoint,
                timeAdd = powerUp.addTime
            };
            initialPowerUpPositions.Add(data);
        }

        initialNPCPositions = new List<NPCData>();
        foreach(GoombaNPCBehavior goomba in FindObjectsOfType<GoombaNPCBehavior>())
        {
            if(goomba.walkingBehavior == WalkingBehavior.Left || goomba.walkingBehavior == WalkingBehavior.Right || goomba.walkingBehavior == WalkingBehavior.StandStill)
            {
                NPCData data = new NPCData
                {
                    position = goomba.transform.position,
                    behavior = goomba.walkingBehavior
                };
                initialNPCPositions.Add(data);
            }
        }
        SmartGoombaPositions = new List<NPCData>();
        foreach (GoombaNPCBehavior goomba in FindObjectsOfType<GoombaNPCBehavior>())
        {
            if(goomba.walkingBehavior == WalkingBehavior.SmartLeft || goomba.walkingBehavior == WalkingBehavior.SmartRight)
            {
                NPCData data = new NPCData
                {
                    position = goomba.transform.position,
                    behavior = goomba.walkingBehavior
                };
                SmartGoombaPositions.Add(data);
            }
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
        foreach (PowerUpData pos in initialPowerUpPositions)
        {
            GameObject newPowerup = Instantiate(powerUpPrefab, pos.position, Quaternion.identity);
            var newPowerUpData = newPowerup.GetComponent<PowerUp>();
            newPowerUpData.addTime = pos.timeAdd;
            newPowerUpData.isCheckpoint = pos.isCheckpoint;
        }

        // Reset Goomba
        foreach (GoombaNPCBehavior goomba in FindObjectsOfType<GoombaNPCBehavior>())
        {
            Destroy(goomba.gameObject);
        }

        // Instantiate new ones at the initial positions
        foreach (NPCData pos in initialNPCPositions)
        {
            GameObject npc = Instantiate(GoombaPrefab, pos.position, Quaternion.identity);
            var newNPC = npc.GetComponent<GoombaNPCBehavior>();
            newNPC.walkingBehavior = pos.behavior;
        }

        // Instantite new Smart goombas
        foreach (NPCData pos in SmartGoombaPositions)
        {
            GameObject npc = Instantiate(SmartGoombaPrefab, pos.position, Quaternion.identity);
            var newNPC = npc.GetComponent<GoombaNPCBehavior>();
            newNPC.walkingBehavior = pos.behavior;
        }
    }
}


public struct TransformData
{
    public Vector3 position;
    public Vector3 scale;
    public float shrinkSpeed;
}

public struct PowerUpData
{
    public Vector3 position;
    public bool isCheckpoint;
    public float timeAdd;
}

public struct NPCData
{
    public Vector3 position;
    public WalkingBehavior behavior;
}