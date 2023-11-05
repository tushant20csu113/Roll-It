using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using TMPro;

public enum eGameEvent
{
    GAME_START,
    GAME_OVER,
    GAME_SCORE
}
public enum GameState
{
    STARTED,
    NOTSTARTED
}
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI logText;
    public Transform player;
    public delegate void GameEvent(eGameEvent gameEvent);
    public static event GameEvent OnGameEvent;
    [SerializeField]
    [Range(0f, 1f)]
    float lerpTime;
    [SerializeField]
    List<Color> colors;
    int colorIndex = 0;
    float t = 0f;

    public GameState currentState;
    public bool isRunning=false;
    public Vector3 direction=Vector3.zero;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentState = GameState.NOTSTARTED;
    }
    void Update()
    {

        if (currentState == GameState.NOTSTARTED)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
            {
                Touch touch1 = Input.GetTouch(0);
                if (touch1.phase == TouchPhase.Ended)
                {
                    direction = Vector3.right;
                    StartGame();
                }
            }
#elif UNITY_EDITOR
            if(Input.GetMouseButtonDown(0))
            {
                direction = Vector3.right;
                StartGame();
            }
#endif
        }
        else
        { 
            CheckAlive();
            CheckInput();
        }
    }
    public void CheckInput()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                 OnGameEvent?.Invoke(eGameEvent.GAME_SCORE);
                if (direction == Vector3.right)
                {
      
                    direction = -Vector3.forward;
                    logText.text = String.Format("Direction:{0}", direction);
                   
                }
                else if (direction == -Vector3.forward)
                {
                  
                    direction = Vector3.right;
                    logText.text = String.Format("Direction:{0}", direction);
         
                }
        
            }
          
        }
      
#elif UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            OnGameEvent?.Invoke(eGameEvent.GAME_SCORE);
            if (direction == Vector3.right)
            {
                direction = -Vector3.forward;
                logText.text = String.Format("Direction:{0}", direction);

            }
            else if (direction == -Vector3.forward)
            {

                direction = Vector3.right;
                logText.text = String.Format("Direction:{0}", direction);

            }
        }
#endif

    }
    //Changes color of platform
    public void ChangeColor(MeshRenderer cubeMeshRender)
    {

        cubeMeshRender.material.color = Color.Lerp(cubeMeshRender.material.color, colors[colorIndex], lerpTime * Time.deltaTime);
        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > 0.9f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= colors.Count) ? 0 : colorIndex;
        }
    }
    //Checks if player is alive or not
    private void CheckAlive()
    {
        if (player.transform != null)
        {
            
            if (player.transform.position.y < -5f)
            {
                OnGameEvent?.Invoke(eGameEvent.GAME_OVER);
                isRunning = false;
                currentState = GameState.NOTSTARTED;
            }
            
        }
    }
    //Invoke Game start event
    private void StartGame()
    {
        currentState = GameState.STARTED;
        isRunning = true;
        OnGameEvent?.Invoke(eGameEvent.GAME_START);
    }
    //Attached to Retry Button
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
