using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // there are three objects that we CAN make obstacles out of,
    // and then an object pool that we assign them to.

    // the object pool exists as a way to save on garbage collection
    // by reusing the same GameObjects over and over again.

    public GameManager gameManagerScript;
    public PlayerPowerUp playerPowerUpScript;

    public GameObject[] obstaclePrefabs;
    public GameObject powerUpPrefab;
    public GameObject heartPrefab;

    private float minSpawnInterval;
    private float middleSpawnInterval;
    private float maxSpawnInterval;


    private float SpawnPosX = 60.0f;

    // we need to subscribe to three events: one to tell us when to
    // prepare the object pool, one to tell us when the player intro
    // has finished and the player is now actively running, and one to
    // tell us when the player has died

    // when the game has started or restarted, we initialize the object
    // pool, making a randomized list so that there is a slightly different
    // ratio of object variety every time.
    //
    // when the player begins running, that's when we begin spawning
    // objects.
    // 
    // when the game is over, we disable all obstacles and do nothing until
    // the game restarts


    private void Start()
    {
        SpawnPosX = 60.0f;
        minSpawnInterval = 5.0f;
        middleSpawnInterval = 8.0f;
        maxSpawnInterval = 13.0f;

        playerPowerUpScript = GameObject.Find("Player").GetComponent<PlayerPowerUp>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (gameManagerScript.isGameStopped)
        {
            GameOver();
        }

    }


    // this is fired when the player has finished the intro, and it starts
    // a coroutine that runs until the player dies

    public void StartSpawner()
    {
        StartCoroutine("CycleObstacles");
        StartCoroutine("PowerUpSpawnRoutine");
        StartCoroutine("HeartSpawnRoutine");
    }

    // this is fired when the player hits an obstacle; it destroys all
    // objects in the pool so we can create a slightly different pool for
    // the next round

    public void GameOver()
    {
        //StopCoroutine("CycleObstacles");
        //StopCoroutine("SpawnPowerUp");
        StopAllCoroutines();

        GameObject[] obstaclesOnScene = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstaclesOnScene)
        {
            Destroy(obstacle);
        }


    }


    // Routine for spawning powerups, do not spawn at the first 5 seconds of the game, after that spawn powerup at random intervals
    IEnumerator PowerUpSpawnRoutine()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnInterval, middleSpawnInterval));

        while (!gameManagerScript.isGameStopped)
        {
            if (playerPowerUpScript.hasPowerUp)
            {
                yield return new WaitForSeconds(3);
                continue;
            }
            else
            {
                SpawnPowerUp();
            }

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

    IEnumerator HeartSpawnRoutine()
    {
        while (!gameManagerScript.isGameStopped)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnHeart();
        }
    }


    // Spawn a powerup at random height
    public void SpawnPowerUp()
    {                 
        float randomSpawnHeight = Random.Range(4.0f, 7.5f);
        Instantiate(powerUpPrefab, new Vector3(SpawnPosX, randomSpawnHeight), powerUpPrefab.transform.rotation);
    }
    public void SpawnHeart()
    {
        float randomSpawnHeight = Random.Range(4.0f, 7.5f);
        Instantiate(heartPrefab, new Vector3(SpawnPosX, randomSpawnHeight), powerUpPrefab.transform.rotation);

    }


    // this coroutine spawns the obstacles. as the player's score gets
    // higher, the lower and upper limits of the spawn timer become lower
    // and tighter. the game becomes pretty much impossible fairly quickly,
    // these numbers would be more balanced in a proper game that you'd want
    // to actually sell

    private IEnumerator CycleObstacles()
    {
        yield return new WaitForSeconds(2);

        float _lowerFuzz;
        float _upperFuzz;
        do
        {

            switch (gameManagerScript.score)
            {
                case int score when score < 5:
                    _lowerFuzz = 2f;
                    _upperFuzz = 3f;
                    break;
                case int score when score < 10:
                    _lowerFuzz = 1.75f;
                    _upperFuzz = 2.75f;
                    break;
                case int score when score < 15:
                    _lowerFuzz = 1.5f;
                    _upperFuzz = 2.5f;
                    break;
                case int score when score < 20:
                    _lowerFuzz = 1.25f;
                    _upperFuzz = 2.25f;
                    break;
                case int score when score < 25:
                    _lowerFuzz = 1.0f;
                    _upperFuzz = 2.0f;
                    break;
                case int score when score < 30:
                    _lowerFuzz = 0.75f;
                    _upperFuzz = 1.75f;
                    break;
                case int score when score < 35:
                    _lowerFuzz = 0.50f;
                    _upperFuzz = 1.50f;
                    break;
                case int score when score < 40:
                    _lowerFuzz = 0.25f;
                    _upperFuzz = 1.25f;
                    break;
                default:
                    _lowerFuzz = 0.25f;
                    _upperFuzz = 1.0f;
                    break;
            }

            // once we have the possible values of our range based on
            // the current score, we get a random float between those
            // values. then we spawn in a new obstacle, and wait for
            // a period of seconds equal to the random float we got.
            // after that time period, this function will run again

            float _timer = Random.Range(_lowerFuzz, _upperFuzz);
            InitializeObstacle();
            yield return new WaitForSeconds(_timer);
        } while (true);
    }

    // create an obstacle using a random selection from the possible prefabs
    // and assign it to the object pool. the value of activeOnInit lets us
    // decide whether the object is immediately active and moving or not.
    //
    // when initializing the pool during the game intro, we make the objects
    // start out disabled, but if we're creating a new obstacle for the pool
    // at the time that the player is running, we'll let the object start out
    // already active.

    private void InitializeObstacle()
    {

        int _choice = Random.Range(0, obstaclePrefabs.Length);
        // UNCOMMENT TO RANDOMIZE ALL
        GameObject _newObstacle = Instantiate(obstaclePrefabs[_choice], new Vector3(SpawnPosX, obstaclePrefabs[_choice].transform.position.y), obstaclePrefabs[_choice].transform.rotation);

    }

}
