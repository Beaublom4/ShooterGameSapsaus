using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    public int currentWave;
    public float doorOpeningSpeed;
    public Transform enemieSpawnPoint, enemyHolder, doorObjWantedLoc;
    public GameObject doorObj;
    bool openDoor, canPress = true;
    [System.Serializable]
    public class Wave
    {
        public EnemyType[] enemies;
        public float timeBetweenWave;
    }
    [System.Serializable]
    public class EnemyType
    {
        public GameObject[] randomEnemyPrefabs;
        public int enemieNumber;
    }
    public Wave[] waves;
    private void Update()
    {
        if(openDoor == true)
        {
            doorObj.transform.position = Vector3.Lerp(doorObj.transform.position, doorObjWantedLoc.position, doorOpeningSpeed * Time.deltaTime);
            if (Vector3.Distance(doorObj.transform.position, doorObjWantedLoc.position) < .5f)
            {
                openDoor = false;
                print("Door opened");
            }
        }
    }
    public void OpenGarageDoor()
    {
        if (canPress == true)
        {
            canPress = false;
            print("Open door");
            openDoor = true;
            StartWave();
        }
    }
    public void StartWave()
    {
        if(currentWave < waves.Length)
        {
            for (int o = 0; o < waves[currentWave].enemies.Length; o++)
            {
                for (int i = 0; i < waves[currentWave].enemies[o].enemieNumber; i++)
                {
                    int randomNumber = Random.Range(0, waves[currentWave].enemies[o].randomEnemyPrefabs.Length);
                    Instantiate(waves[currentWave].enemies[o].randomEnemyPrefabs[randomNumber], enemieSpawnPoint.transform.position, enemieSpawnPoint.rotation, enemieSpawnPoint);
                    GameObject g = enemieSpawnPoint.GetChild(0).gameObject;
                    g.GetComponent<Enemy>().Trigger(GameObject.FindWithTag("Player"));
                    g.transform.SetParent(enemyHolder);
                }
            }
            currentWave++;
            Invoke("StartWave", waves[-1 + currentWave].timeBetweenWave);
        }
        else
        {
            print("Out of waves");
        }
    }
}
