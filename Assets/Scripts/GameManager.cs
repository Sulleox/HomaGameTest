using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private int _playerTurn = 0;
    public int PlayerIndexTurn { get { return _playerTurn + 1; } }
    public int NextPlayerIndexTurn { get { return ((_playerTurn + 1) % 2) + 1; } }

    private Utils.GameState _gameState = Utils.GameState.Init;
    public Utils.GameState CurrentGameState { get { return _gameState; } }

    private UnityEvent<Utils.GameState> _onGameStateChange = new UnityEvent<Utils.GameState>();
    public UnityEvent<Utils.GameState> OnGameStateChange { get { return _onGameStateChange; } }

    private BoardManager _boardManager;

    [SerializeField] private int _sceneIndex;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        _onGameStateChange.RemoveAllListeners();
    }

    private void Start()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        _boardManager.Initialize();
        _boardManager.AllPawnPlaced.AddListener(StartDynamicMode);
        _boardManager.PlayerMoveDone.AddListener(EndTurn);
        _boardManager.TicTacToe.AddListener(EndGame);
        StartSelectionPhase();
    }

    private void StartSelectionPhase()
    {
        Debug.Log("Start Selection Phase");
        _playerTurn = Random.Range(0, 2);
        Debug.Log("Player " + _playerTurn + " is starting");
        _gameState = Utils.GameState.Selection;
        _onGameStateChange?.Invoke(_gameState);
    }

    private void StartDynamicMode()
    {
        Debug.Log("Start Dynamic Mode");
        _gameState = Utils.GameState.Dynamic;
        _onGameStateChange?.Invoke(_gameState);
    }

    private void EndTurn()
    {
        _playerTurn = (_playerTurn + 1) % 2;
        Debug.Log("Player turn start " + _playerTurn);
    }

    private void EndGame()
    {
        Debug.Log("Game Ended");
        _gameState = Utils.GameState.End;
        _onGameStateChange?.Invoke(_gameState);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(_sceneIndex);
    }
}
