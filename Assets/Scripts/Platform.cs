using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Platform : MonoBehaviour
{
    public GameObject diamond_Prefab;
    [Range(0f, 100f)] public float spawnProb;
    MeshRenderer cubeMeshRenderer;
    private Rigidbody _rb;
    GameObject _diamond;
    private void Awake()
    {  
            DiamondSpawner();
    }
    private void OnEnable()
    {
        int ranNum = UnityEngine.Random.Range(0, 100);
        if (ranNum < spawnProb)
            transform.GetChild(0).gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        cubeMeshRenderer = GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = true;
        _rb.isKinematic = true;

    }
    private void Update()
    {
        if (GameManager.Instance.isRunning == true)
        { 
            GameManager.Instance.ChangeColor(cubeMeshRenderer);
         
        }
    }

    void DiamondSpawner()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);
        _diamond=Instantiate(diamond_Prefab, spawnPos, Quaternion.identity);
        _diamond.transform.SetParent(transform);
        _diamond.SetActive(false);
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
          
            if (_rb != null)
                _rb.isKinematic = false;
            Invoke("Repool", 0.7f);
            /* if (Vector3.Distance(transform.position, BallController.instance.transform.position) > 1f)
             {
                 if (_rb != null)
                     _rb.isKinematic = false;
                 else
                     Debug.Log("Rigidbody gone");

                     Invoke("Repool", 0.5f);


             }*/
        }
    }
   
    void Repool()
    { 
        LevelGenerator.instance.ReturnToPool(this.gameObject);
    }
}
