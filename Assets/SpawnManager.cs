using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameManager gm;
    public List<GameObject> spawnLocations;

    public GameObject enemyPrefab;
    public GameObject EnemyContainer;

    public AudioClip newEnemySpawnAudioClip;
    public AudioClip currentClip;
    public AudioSource audioSource;

    public float pitchMin = 0.8f;
    public float pitchMax = 1.2f;
    public float volumeMin = 0.25f;
    public float volumeMax = 0.75f;

    public int ticksSinceSpawn = 0;
    public int spawnEveryNumTicks = 11;

    private int activeCandleCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        GameManager.Instance.onTick += onTick;
        GameManager.Instance.onNextCandle += onNextCandle;
        // GameObject firstEnemy = spawnAtRandomPortal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onTick()
    {
        ticksSinceSpawn += 1;
        int modifiedTickTime = spawnEveryNumTicks - activeCandleCount;

        if(ticksSinceSpawn >= modifiedTickTime)
        {
            spawnAtRandomPortal();
        }
    }

    private void onNextCandle(int numCandles)
    {
        activeCandleCount = numCandles;
    }

    private GameObject spawnAtRandomPortal()
    {
        ticksSinceSpawn = 0;
        //select random spawn point
        GameObject randSpawn = spawnLocations[Random.Range(0, spawnLocations.Count)];

        GameObject obj = Instantiate(enemyPrefab, randSpawn.transform.position, randSpawn.transform.rotation, EnemyContainer.transform);
        obj.transform.position = randSpawn.transform.position;

        currentClip = newEnemySpawnAudioClip;
        audioSource.clip = currentClip;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, ((volumeMax - volumeMin) / 4) + volumeMin);
        audioSource.PlayOneShot(currentClip);

        return obj;
    }

    private void OnDisable()
    {
        GameManager.Instance.onTick -= onTick;
    }
}
