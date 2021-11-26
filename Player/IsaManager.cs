using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    RPG,
    RTS,
    Menu,
    Pause
}

public class IsaManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static IsaManager instance;
    // Start is called before the first frame update


    public GameState currentGameState = GameState.RPG;


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartGame();
    }
    void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.RPG)
        {

        }
        else if (newGameState == GameState.RTS)
        {


        }
        else if (newGameState == GameState.Pause)
        {

        }

        currentGameState = newGameState;

    }
    public void StartGame()
    {
        SetGameState(GameState.RPG);
    }
    // Update is called once per frame
    void Update()
    {



    }

    public void RPG()
    {


    }
    public void RTS()
    {


    }
    public void Menu()
    {

    }
}
