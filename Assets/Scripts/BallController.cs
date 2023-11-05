using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isAlive=false;
    public Vector3 originalBallPosition;
    public static BallController instance { get; private set; }
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        originalBallPosition = transform.position;
    }
    private void OnEnable()
    {
        GameManager.OnGameEvent += OnGameEvent;
    }
    private void OnDisable()
    {
        GameManager.OnGameEvent += OnGameEvent;
    }
    private void OnGameEvent(eGameEvent gameEvent)
    {
        if (gameEvent == eGameEvent.GAME_START)
        {

            isAlive = true;
        }
        if(gameEvent==eGameEvent.GAME_OVER)
        {
            isAlive = false;
        }
       
    }
}
