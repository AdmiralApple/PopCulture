using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBubble : MonoBehaviour
{
    public GameObject ArrowPrefab;
    public Bubble thisBubble;
    public float ArrowEverySeconds = 2f;
    private Coroutine arrowRoutine;

    private void Awake()
    {
        thisBubble = GetComponent<Bubble>();
    }

    private void OnEnable()
    {
        StartArrowRoutine();
    }

    private void OnDisable()
    {
        StopArrowRoutine();
    }

    void StartArrowRoutine()
    {
        if (arrowRoutine != null || ArrowPrefab == null)
        {
            return;
        }

        arrowRoutine = StartCoroutine(SpawnArrowRoutine());
    }

    void StopArrowRoutine()
    {
        if (arrowRoutine == null)
        {
            return;
        }

        StopCoroutine(arrowRoutine);
        arrowRoutine = null;
    }

    IEnumerator SpawnArrowRoutine()
    {
        while (true)
        {
            SpawnArrow();
            float waitTime = Mathf.Max(ArrowEverySeconds, 0f);
            if (waitTime <= 0f)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    void SpawnArrow()
    {
        thisBubble = GetComponent<Bubble>();
        if (ArrowPrefab == null || Camera.main == null || thisBubble == null)
        {
            print("ArrowPrefab or Main Camera is null, cannot spawn arrow.");
            return;
        }

        if(thisBubble != null && thisBubble.Popped)
        {
            return;
        }

        Vector3 spawnPosition = transform.position;
        Vector3 mouseScreen = Input.mousePosition;

        float zDistance = Mathf.Abs(Camera.main.transform.position.z - spawnPosition.z);
        mouseScreen.z = zDistance;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = spawnPosition.z;

        Vector3 direction = mouseWorld - spawnPosition;
        direction.z = 0f;
        if (direction.sqrMagnitude <= Mathf.Epsilon)
        {
            direction = Vector3.right;
        }

        GameObject arrowInstance = Instantiate(ArrowPrefab, spawnPosition, Quaternion.identity);
        arrowInstance.transform.right = direction.normalized;

        Destroy(arrowInstance, 5f);
    }
}
