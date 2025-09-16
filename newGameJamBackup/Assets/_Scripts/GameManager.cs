using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    private int score;
    private float timeElapsed;

    [SerializeField] private GameObject[] badObjectsToSpawn; // Array of objects to spawn
    [SerializeField] private GameObject[] collectables; // Array of objects to spawn
    [SerializeField] private float hightOfObject; // Array of objects to spawn

    private int randomSpownLineIndex;
    private int randomSecondSpownLineIndex;
    private int randomNumberOfObjectsToSpown;
    private int randomObjectListToPool;
    private int randomBadObjectToSpown;
    private int randomCollectableSpown;
    private int randomChanceTOSpownCollectableOnTopOfObject;

    private float waveSpownerTimer = 1;

    private void Start()
    {
        score = 0;
        timeElapsed = 0f;
        StartCoroutine(SpowningObjects());
    }
    private void Update()
    {
        // Update the elapsed time
        timeElapsed += Time.deltaTime;
        if (Input.GetKey(KeyCode.V)) {
            //Ispis standardnog vremena u sekundama i milisekundama u formatu "seconds:milliseconds"
            Debug.Log("Time: " + Mathf.FloorToInt(timeElapsed) + ":" + Mathf.FloorToInt((timeElapsed - Mathf.FloorToInt(timeElapsed)) * 1000).ToString("D2"));
        }
    }

    private IEnumerator SpowningObjects() {
        randomNumberOfObjectsToSpown = Random.Range(1, 4); // Random number of objects to spawn (1 to 3)
        SetLinesToSpownObjects(randomNumberOfObjectsToSpown);
        yield return new WaitForSeconds(waveSpownerTimer);
        if (timeElapsed < 50) {
            StartCoroutine(SpowningObjects());

        }
    }

    private void SetLinesToSpownObjects(int numberOfObjectsToSpown)
    {
        //ako se spowna jedan objekt
        if (numberOfObjectsToSpown == 1)
        {
            randomSpownLineIndex = Random.Range(-1, 2);
            randomChanceTOSpownCollectableOnTopOfObject = Random.Range(0, 5);

            if (randomChanceTOSpownCollectableOnTopOfObject == 0)
            {
                Debug.Log("Spowning collectable on top of object");
                randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                randomCollectableSpown = Random.Range(0, collectables.Length);
                Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), Quaternion.identity);
                Instantiate(collectables[randomCollectableSpown], new Vector3(randomSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y * 4, 5), Quaternion.identity);
            }
            else if (randomChanceTOSpownCollectableOnTopOfObject >= 1)
            {
                Debug.Log("Spowning only one object");
                randomObjectListToPool = Random.Range(0, 2);
                if (randomObjectListToPool == 0) // bad object
                {
                    randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                    Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), Quaternion.identity);

                }
                else // collectable
                {
                    randomCollectableSpown = Random.Range(0, collectables.Length);
                    Instantiate(collectables[randomCollectableSpown], new Vector3(randomSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y, 5), Quaternion.identity);
                }
            }
        }

        else if (numberOfObjectsToSpown == 2)
        {
            randomSpownLineIndex = Random.Range(-1, 2);
            randomSecondSpownLineIndex = Random.Range(-1, 2);
            while (randomSecondSpownLineIndex == randomSpownLineIndex)
            {
                randomSecondSpownLineIndex = Random.Range(-1, 2);
            }
            for (int firstSecondObject = 0; firstSecondObject < 2; firstSecondObject++)
            {
                randomChanceTOSpownCollectableOnTopOfObject = Random.Range(0, 5);
                if (randomChanceTOSpownCollectableOnTopOfObject == 0)
                {
                    Debug.Log("Spowning collectable on top of object");
                    randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                    randomCollectableSpown = Random.Range(0, collectables.Length);
                    if (firstSecondObject == 0)
                    {
                        Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), Quaternion.identity);
                        Instantiate(collectables[randomCollectableSpown], new Vector3(randomSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y * 4, 5), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSecondSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), Quaternion.identity);
                        Instantiate(collectables[randomCollectableSpown], new Vector3(randomSecondSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y * 4, 5), Quaternion.identity);
                    }
                }
                else if (randomChanceTOSpownCollectableOnTopOfObject >= 1)
                {
                    Debug.Log("Spowning only one object");
                    randomObjectListToPool = Random.Range(0, 2);
                    if (randomObjectListToPool == 0) // bad object
                    {
                        randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                        if (firstSecondObject == 0)
                        {
                            Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSecondSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), Quaternion.identity);
                        }
                    }
                    else
                    {
                        randomCollectableSpown = Random.Range(0, collectables.Length);
                        if (firstSecondObject == 0)
                        {
                            Instantiate(collectables[randomCollectableSpown], new Vector3(randomSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y, 5), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(collectables[randomCollectableSpown], new Vector3(randomSecondSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y, 5), Quaternion.identity);
                        }
                    }

                }
            }
        }

        else if (numberOfObjectsToSpown == 3)
        {
            for (int i = -1; i < 2; i++)
            {
                randomChanceTOSpownCollectableOnTopOfObject = Random.Range(0, 5);

                if (randomChanceTOSpownCollectableOnTopOfObject == 0)
                {
                    Debug.Log("Spowning collectable on top of object");
                    randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                    randomCollectableSpown = Random.Range(0, collectables.Length);
                    Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(i * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), Quaternion.identity);
                    Instantiate(collectables[randomCollectableSpown], new Vector3(i * 3, collectables[randomCollectableSpown].transform.position.y * 4, 5), Quaternion.identity);
                }
                else if (randomChanceTOSpownCollectableOnTopOfObject >= 1)
                {
                    Debug.Log("Spowning only one object");
                    randomObjectListToPool = Random.Range(0, 2);
                    if (randomObjectListToPool == 0) // bad object
                    {
                        randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                        Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(i * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), Quaternion.identity);

                    }
                    else // collectable
                    {
                        randomCollectableSpown = Random.Range(0, collectables.Length);
                        Instantiate(collectables[randomCollectableSpown], new Vector3(i * 3, collectables[randomCollectableSpown].transform.position.y, 5), Quaternion.identity);
                    }
                }
            }
        }
    }

    /// <summary>
    /// score manipulation
    /// </summary>
    /// <param name="points"></param>
    // Method to add score
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }
    // Method to get current score
    public int GetScore()
    {
        return score;
    }
}
