using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Attached to Spawn Manager game object in Luksong Baka Scene
    // Manage the spawn of obstacles, powerups, and hearts 

    public GameManager gameManagerScript;
    public PlayerPowerUp playerPowerUpScript;

    public GameObject[] obstaclePrefabs;
    public GameObject powerUpPrefab;
    public GameObject heartPrefab;

    // Interval between spawns
    private float minSpawnInterval = 5.0f;
    private float middleSpawnInterval = 8.0f;
    private float maxSpawnInterval = 13.0f;

    private float SpawnPosX = 60.0f;

    // Spawn height for powerups and hearts
    private float minSpawnHeight = 4.0f;
    private float maxSpawnHeight = 7.5f;


    void Start()
    {
        playerPowerUpScript = GameObject.Find("Player").GetComponent<PlayerPowerUp>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Called in player controller script when idle animation is finished
    // Start the routine for spawning obstacle, powerup, and heart
    public void StartSpawner()
    {
        StartCoroutine("ObstaclesSpawnRoutine");
        StartCoroutine("PowerUpSpawnRoutine");
        StartCoroutine("HeartSpawnRoutine");
    }

    // Stop all routines for spawning, and destroy all obstacles
    // Called in player controller when player has 0 lives/game over
    public void GameOver()
    {
        StopAllCoroutines();
        GameObject[] obstaclesOnScene = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstaclesOnScene)
        {
            Destroy(obstacle);
        }
    }

    // Routine for spawning powerups
    IEnumerator PowerUpSpawnRoutine()
    {
        // A random delay at the start of the routine
        yield return new WaitForSeconds(Random.Range(minSpawnInterval, middleSpawnInterval));

        // Continue spawning an obstacle as long as the game is not over
        while (true)
        {
            // Continue spawning until routine is stopped
            if (playerPowerUpScript.hasPowerUp)
            {
                yield return new WaitForSeconds(3);
                continue;
            }
            // if player has no powerup, then spawn one
            else
            {
                SpawnPowerUp();
            }

            // Acts as a delay (necessary before checking if player has powerup)
            yield return new WaitForSeconds(1.5f);

            // If the player picked up the most recent powerup, then spawn the next one a little longer
            if (playerPowerUpScript.hasPowerUp)
            {
                yield return new WaitForSeconds(Random.Range(middleSpawnInterval, maxSpawnInterval));
            }
            // If player did not picked up the most recent powerup, then shorten the delay for the next powerup
            else
            {
                yield return new WaitForSeconds(Random.Range(minSpawnInterval, middleSpawnInterval));
            }
        }
    }

    // Routine for spawning hearts
    IEnumerator HeartSpawnRoutine()
    {
        // Continue spawning until routine is stopped
        while (true)
        {
            // Random interval between spawns
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnHeart();
        }
    }

    // Routine for spawning obstacles
    private IEnumerator ObstaclesSpawnRoutine()
    {
        // Delay at the start of the game
        yield return new WaitForSeconds(2);

        // minInterval and maxInterval values changes depending on score
        // The higher the score, the shorter the interval
        float minInterval;
        float maxInterval;
        do
        {
            switch (gameManagerScript.score)
            {
                case int score when score < 5:
                    minInterval = 2f;
                    maxInterval = 3f;
                    break;
                case int score when score < 10:
                    minInterval = 1.75f;
                    maxInterval = 2.75f;
                    break;
                case int score when score < 15:
                    minInterval = 1.5f;
                    maxInterval = 2.5f;
                    break;
                case int score when score < 20:
                    minInterval = 1.25f;
                    maxInterval = 2.25f;
                    break;
                case int score when score < 25:
                    minInterval = 1.0f;
                    maxInterval = 2.0f;
                    break;
                case int score when score < 30:
                    minInterval = 0.75f;
                    maxInterval = 1.75f;
                    break;
                case int score when score < 35:
                    minInterval = 0.50f;
                    maxInterval = 1.50f;
                    break;
                case int score when score < 40:
                    minInterval = 0.25f;
                    maxInterval = 1.25f;
                    break;
                default:
                    minInterval = 0.25f;
                    maxInterval = 1.0f;
                    break;
            }

            // Spawns the obstacle 
            SpawnObstacle();
            // Wait for a random interval before spawning again
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        } while (true);
    }


    // Spawn a powerup at random height
    public void SpawnPowerUp()
    {
        float randomSpawnHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
        Instantiate(powerUpPrefab, new Vector3(SpawnPosX, randomSpawnHeight), powerUpPrefab.transform.rotation);
    }

    // Spawn a heart at a random height
    private void SpawnHeart()
    {
        float randomSpawnHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
        Instantiate(heartPrefab, new Vector3(SpawnPosX, randomSpawnHeight), powerUpPrefab.transform.rotation);
    }

    // Spawn a random obstacle
    private void SpawnObstacle()
    {
        int index = Random.Range(0, obstaclePrefabs.Length);
        Instantiate(obstaclePrefabs[index], new Vector3(SpawnPosX, obstaclePrefabs[index].transform.position.y), obstaclePrefabs[index].transform.rotation);
    }
}
