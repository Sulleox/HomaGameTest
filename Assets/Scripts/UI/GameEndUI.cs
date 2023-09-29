using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _gameEndRoot;
    [SerializeField] private GameObject _restartButton;

    void Start()
    {
        GameManager.Instance.OnGameStateChange.AddListener(CheckForGameEnd);
    }

    private void CheckForGameEnd(Utils.GameState gameState)
    {
        if (gameState == Utils.GameState.End)
        {
            _text.gameObject.SetActive(true);
            _gameEndRoot.SetActive(true);
            _restartButton.SetActive(true);
            _text.text = (GameManager.Instance.PlayerIndexTurn == 1 ? "White" : "Black") + "Wins !!";
        }
    }
}
