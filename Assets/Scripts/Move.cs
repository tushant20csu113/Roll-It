using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    MeshRenderer cubeMeshRenderer;
   

    private float platformSpeed = 2f;
    private Vector3 direction = Vector3.zero;
    bool isRunning = false;
    bool collided = false;
  
    private Rigidbody _rb;
   
    public static Move instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        GameManager.OnGameEvent += OnGameEvent;
    }
    private void OnDisable()
    {
        GameManager.OnGameEvent -= OnGameEvent;
    }
    private void Start()
    {
        cubeMeshRenderer = GetComponent<MeshRenderer>();
        platformSpeed = LevelGenerator.instance.platformSpeed;
        _rb = GetComponent<Rigidbody>();
        
    }
    private void OnGameEvent(eGameEvent gameEvent)
    {
        if (gameEvent == eGameEvent.GAME_OVER)
        {
            isRunning = false;
            collided = false;
        }
        if (gameEvent == eGameEvent.GAME_START)
        {
            isRunning = true;
            direction = Vector3.right;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            direction= GameManager.Instance.direction;
            GameManager.Instance.ChangeColor(cubeMeshRenderer);
            transform.Translate(direction * (platformSpeed * Time.deltaTime));
            if (collided)
                CheckAlive();
        }
    }
    

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_rb != null)
                _rb.isKinematic = false;
            else
                Debug.Log("Rigidbody gone");
            collided = true;
        }
    }
    void CheckAlive()
    {
        if (transform.position.y < -5f)
        {
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
            isRunning = false;
            collided = false;
        }
    }
}

