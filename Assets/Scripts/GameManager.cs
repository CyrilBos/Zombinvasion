using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

struct ZombieInfo
{
    public ZombieMovement movement;
    public ZombieAttack attack;

    public ZombieInfo(GameObject zombie)
    {
        movement = zombie.GetComponent<ZombieMovement>();
        attack = zombie.GetComponent<ZombieAttack>();
    }
}

public class GameManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject civilianPrefab;
    public GameObject enemyPrefab;

    [SerializeField]
    private int zombieStartCount = 1;

    [SerializeField]
    private float positioningOffset = 0.3f;

    private Camera mainCamera;
    private CameraMovement cameraMovement;

    private Dictionary<GameObject, ZombieInfo> zombiesInfo = new Dictionary<GameObject, ZombieInfo>();
    private List<GameObject> zombies = new List<GameObject>();

    static private int MaxCiviliansPerSpawn = 5;
    static private int SpawnPixelOffset = 50;
    static private float EnemiesSpawnCooldownValue = 5f;
    static private float CiviliansSpawnCooldownValue = 10f;

    private float enemiesSpawnCooldown = EnemiesSpawnCooldownValue;
    private float civiliansSpawnCooldown = CiviliansSpawnCooldownValue;

    void Start()
    {
        // GetComponent<LevelGenerator>().GenerateLevel();
        for (int i = 0; i < zombieStartCount; i++)
        {
            SpawnZombie(ComputeUnitPositionAroundTarget(new Vector3(7,1,0), i));
        }

        mainCamera = Camera.main;
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
        cameraMovement.Target = zombies[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.collider.tag == "Ground")
                {
                    int i = 0;
                    foreach (GameObject zombie in zombies)
                    {
                       zombiesInfo[zombie].movement.SetNewDestination(ComputeUnitPositionAroundTarget(hit.point, i));
                        // zombiesMovements[i].SetNewDestination(ComputePositionAroundCenterZombie(hit.point, i));
                        i += 1;
                    }
                }
            }
        }

       //  SpawnCivilians();
        SpawnEnemies();
    }


    // TODO Make the zombie join the horde when spawned
    // and let zombies off the horde movement until they die or kill their target?
    public void SpawnZombie(Vector3 position)
    {
        GameObject zombie = Instantiate(zombiePrefab, position, Quaternion.identity);
        zombies.Add(zombie);
        zombiesInfo.Add(zombie, new ZombieInfo(zombie));
    }

    public GameObject GetNearestZombie(Vector3 position)
    {
        if (zombies.Count == 0)
            return null;

        float nearestDistance = Vector3.Distance(zombies[0].transform.position, position);
        GameObject nearestZombie = zombies[0];
        for(int i = 1; i < zombies.Count; i++)
        {
            float distance = Vector3.Distance(zombies[i].transform.position, position);
            if (distance < nearestDistance)
            {
               nearestDistance = distance;
                nearestZombie = zombies[i];
            }
        }

        return nearestZombie;
    }

    public void ZombieDied(GameObject zombie)
    {
        if (zombies.Count > 0)
        {
            if (zombies[0] == zombie && zombies.Count > 1)
            {
                cameraMovement.Target = zombies[1];
            }
            zombiesInfo.Remove(zombie);
            zombies.Remove(zombie);
        }
        cameraMovement.ScreenShake();
    }

    /**
   * Computes the position of the unit to form a circle with the others
   */
    private Vector3 ComputeUnitPositionAroundTarget(Vector3 center, int index)
    {
        if (index == 0)
            return center;

        int angle = index * (360 / zombies.Count);
        Vector3 direction = Quaternion.Euler(0, 0, angle) * center;
        return center + direction * positioningOffset * (index / 9);
    }

    // Computes the target position to stay in the same formation around the new destination
    // Works well if zombies are grouped, but problematic if a zombie wanders off 
    // TODO set a minimum distance between zombies to use this instead of the other 
     private Vector3 ComputePositionAroundCenterZombie(Vector3 destination, int index)
    {
        return destination - (zombies[0].transform.position - zombies[index].transform.position);
    }

    private void SpawnCivilians()
    {
        if (civiliansSpawnCooldown > 0f)
        {
            civiliansSpawnCooldown -= Time.deltaTime;
        }
        else
        {
            int civiliansSpawn = 1 + (int) Mathf.Floor(Random.value * MaxCiviliansPerSpawn);

            civiliansSpawnCooldown = CiviliansSpawnCooldownValue;
        }
    }

    private void SpawnEnemies()
    {
        if (enemiesSpawnCooldown > 0f)
        {
            enemiesSpawnCooldown -= Time.deltaTime;
        } else
        {
            int enemiesSpawnCount = 1 + zombies.Count * (int) Random.Range(0.75f, 1.25f);
            for (int i = 0; i < enemiesSpawnCount; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            enemiesSpawnCooldown = EnemiesSpawnCooldownValue;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float rng = Random.Range(0, 3);
        Vector3 position;
        float cameraHeight = Camera.main.transform.position.z;
        switch(rng)
        {
            case 0:
                position = Camera.main.ScreenToWorldPoint(
                    new Vector3(-SpawnPixelOffset, 0, cameraHeight));
                break;
            case 1:
                position = Camera.main.ScreenToWorldPoint(
                    new Vector3(0, -SpawnPixelOffset, cameraHeight));
                break;
            case 2:
                position = Camera.main.ScreenToWorldPoint(
                    new Vector3(Screen.width + SpawnPixelOffset, 0, cameraHeight));
                break;
            case 3:
                position = Camera.main.ScreenToWorldPoint(
              new Vector3(Screen.width + SpawnPixelOffset,
                  Screen.height + SpawnPixelOffset, cameraHeight));
                break;
            default:
                position = new Vector3(0,0,0);
                break;
        }

        position.y = 1;
        return position;
    }
}
