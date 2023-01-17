using System.Collections;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    const float SPAWN_DELAY_MIN = 4.0f;
    const float SPAWN_DELAY_MAX = 12.0f;
    public GameObject[] Cars;
    public Vector3[] CarSpawns;

    private IEnumerator coroutine;

    private IEnumerator doSpawnLoop()
    {
        while (true)
        {
            float spawnDelay = Random.Range(SPAWN_DELAY_MIN, SPAWN_DELAY_MAX);
            yield return new WaitForSeconds(spawnDelay);
            int spawnPos = Random.Range(0, CarSpawns.Length);
            Instantiate(Cars[spawnPos], CarSpawns[spawnPos], new Quaternion(0, 0, 0, 1));
        }
    }
    public void PlayerCollided()
    {
        StopCoroutine(coroutine);
    }
    public void PlayerLeft()
    {
        coroutine = doSpawnLoop();
        StartCoroutine(coroutine);
    }
    // Start is called before the first frame update
    void Start()
    {
        coroutine = doSpawnLoop();
        StartCoroutine(coroutine);
    }
}
