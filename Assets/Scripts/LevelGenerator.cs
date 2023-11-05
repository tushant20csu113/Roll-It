using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform startPos;
    public GameObject platform_Prefab;
    [SerializeField]
    int PlatformsToBePrespawned=15;
    public float platformSpeed=2f;
    Vector3 spawnPos;

    List<GameObject> platformPool;
    List<Transform> tileList;
    GameObject lastSpawnedTile;
    private bool isRunning=false;
    Vector3 direction=Vector3.zero;
    //Singleton
    public static LevelGenerator instance { get; private set;}
    private void OnEnable()
    {
        GameManager.OnGameEvent += OnGameEvent;
    }
    private void OnGameEvent(eGameEvent gameEvent)
    {
        if (gameEvent == eGameEvent.GAME_OVER)
        {
            tileList.Clear();
            isRunning = false;
        }
        
        if (gameEvent == eGameEvent.GAME_START)
        {
            direction = Vector3.right;
            isRunning = true;
        }
        
   }
    private void Awake()
    {
        instance = this;
        platformPool = new List<GameObject>();
        tileList = new List<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = startPos.position;
        PreloadPlatforms();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            direction=GameManager.Instance.direction;
            GameObject tile;

            if (platformPool.Count > 0)
            {
                tile = GetFromPool();
                if (tile != null)
                {
                    tileList.Add(tile.transform);
                }
            }
            if(tileList.Count>0)
            {
                MovePlatforms();
            }
        }
    }


    public GameObject GetFromPool()
    {
         foreach(GameObject platform in platformPool)
        {
            if(!platform.activeInHierarchy)
            {
                platform.SetActive(true);      
                platform.transform.position = GetSpawnPosition();

                return platform;
            }
            
        }
        return null;
    }
    void MovePlatforms()
    {
        foreach(Transform tile in tileList)
        {
            tile.Translate(direction * platformSpeed * Time.deltaTime);
        }
    }
    Vector3 GetSpawnPosition()
    {
        int ranNum = (int)Random.Range(0, 2);
        if (ranNum == 0)
            spawnPos = tileList[tileList.Count - 1].transform.forward + tileList[tileList.Count - 1].transform.position;
        else
            spawnPos = -tileList[tileList.Count - 1].transform.right + tileList[tileList.Count - 1].transform.position;
        return spawnPos;
    }
    public void ReturnToPool(GameObject platform)
    {
        // Debug.Log("Returned");
         Rigidbody _rb=platform.transform.GetComponent<Rigidbody>();
         _rb.isKinematic = true;
        platform.SetActive(false);
        tileList.Remove(platform.transform);
    }
   
    void PreloadPlatforms()
    {
        if (platform_Prefab != null)
        {
            for (int i = 0; i <= PlatformsToBePrespawned; i++)
            {
                lastSpawnedTile = Instantiate(platform_Prefab, spawnPos, platform_Prefab.transform.rotation);
                lastSpawnedTile.transform.SetParent(gameObject.transform);
                platformPool.Add(lastSpawnedTile);
                tileList.Add(lastSpawnedTile.transform);
                int ranNum = (int)Random.Range(0, 2);
                if (ranNum == 0)
                    spawnPos = lastSpawnedTile.transform.forward + lastSpawnedTile.transform.position;
                else
                    spawnPos = -lastSpawnedTile.transform.right + lastSpawnedTile.transform.position;
            }
        }
        else
            Debug.Log("Prefab missing");
    }
 
}
