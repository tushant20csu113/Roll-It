using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    Vector3 _distance;
    bool isRunning = false;
    private void OnEnable()
    {
        GameManager.OnGameEvent += OnGameEvent;
    }

    private void OnGameEvent(eGameEvent gameEvent)
    {
        switch (gameEvent)
        {
            case eGameEvent.GAME_START:
                isRunning = true;
                break;
            case eGameEvent.GAME_OVER:
                isRunning = false;
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _distance = player.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        transform.position = new Vector3(player.position.x - _distance.x, transform.position.y, player.position.z - _distance.z);
    }
}
