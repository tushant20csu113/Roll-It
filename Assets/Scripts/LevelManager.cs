using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform startPoint;
    public GameObject tilePrefab;
    [SerializeField]
    private int tilesToBePrespawned = 5;
    private Vector3 spawnPos;
    private int j = 0;

    public List<GameObject> tilePool;
    public List<Transform> tileList;
    // Start is called before the first frame update
    void Start()
    {
        tilePool = new List<GameObject>();
        tileList = new List<Transform>();
        spawnPos = startPoint.position;
        PreSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown((KeyCode.A)))
        {
            if (j == tileList.Count)
                j = 0;
            Debug.Log("j="+ j);
            ReturnToPool(tileList[0].transform.gameObject);
        }
        if (Input.GetKeyDown((KeyCode.S)))
        {

            GameObject ob=GetFromPool();
            if (ob!=null)
            {
                ob.transform.position = GetSpawnPosition();
                tileList.Add(ob.transform);
            }
        }
        
    }
    public GameObject GetFromPool()
    {
     
        for( int i=0;i<tilePool.Count;i++)
        {
   
            if(!tilePool[i].activeInHierarchy)
            {
                tilePool[i].SetActive(true);      
           
                return tilePool[i];
            }
        }
        return null;
    }
    public void ReturnToPool(GameObject platform)
    {

        Rigidbody _rb=platform.transform.GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        platform.SetActive(false);
        tileList.Remove(platform.transform);
    }
    void PreSpawn()
    {
        for(int i=0;i<=tilesToBePrespawned;i++)
        {
            GameObject spawnedTile = Instantiate(tilePrefab, spawnPos, tilePrefab.transform.rotation);
            spawnedTile.transform.SetParent(transform);
            tilePool.Add(spawnedTile);
            tileList.Add(spawnedTile.transform);
            spawnPos = GetSpawnPosition();
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition;
        int rannum =(int) UnityEngine.Random.Range(0, 2);
        if (rannum == 0)
            spawnPosition = tileList[tileList.Count - 1].forward + tileList[tileList.Count - 1].transform.position;
        else
            spawnPosition = -tileList[tileList.Count - 1].right + tileList[tileList.Count - 1].transform.position;
        return spawnPosition;
    }
    private Vector3 GetSpawnPosition1()
    {
        Vector3 spawnPosition;
        int rannum =(int) UnityEngine.Random.Range(0, 2);
        if (rannum == 0)
            spawnPosition = tilePool[tilePool.Count - 1].transform.forward + tilePool[tilePool.Count - 1].transform.position;
        else
            spawnPosition = -tilePool[tilePool.Count - 1].transform.right + tilePool[tilePool.Count - 1].transform.position;
        return spawnPosition;
    }
}
