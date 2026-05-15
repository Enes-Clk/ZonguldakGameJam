using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int fishCount = 0;
    float fishSpawnCounter = 0;
    
    public GameObject[] fishPrefabs;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        FishSpawn();
    }

    void FishSpawn()
    {
        if(fishSpawnCounter <= 0 && fishCount < 20)
        {
            Instantiate(fishPrefabs[Random.Range(0,3)], new Vector3(Random.Range(4f, 30f), Random.Range(-8f, -12f), 0), Quaternion.identity, null);
            fishSpawnCounter = 1f;
            fishCount++;
        }
        else
        {
            fishSpawnCounter -= Time.deltaTime;
        }
    }

    public void FishCaught()
    {
        fishCount--;
    }
}
