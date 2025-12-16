using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public Transform SpawnTop;
    public Transform SpawnBottom;
    public GameObject BubblePrefab;
    public float spawnTime = 5f;

    //coroutine that spawns a bubble between top and bottom every spawnTime seconds

    IEnumerator Start()
    {
        while (true)
        {
            float randomY = Random.Range(SpawnBottom.position.y, SpawnTop.position.y);
            Vector3 spawnPosition = new Vector3(SpawnTop.position.x, randomY, SpawnTop.position.z);

            //randomize rotation
            Quaternion spawnRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(BubblePrefab, spawnPosition, spawnRotation);

            yield return new WaitForSeconds(spawnTime);
        }
    }




}