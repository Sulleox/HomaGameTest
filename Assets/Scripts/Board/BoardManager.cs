using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Button _deselectButton;
    [SerializeField] private Pawn[] _playerOnePawn;
    [SerializeField] private Pawn[] _playerTwoPawn;

    private int _playerOnePawnPlacementCount = 0;
    private int _playerTwoPawnPlacementCount = 0;

    private BoardCell _selectedCell;
    private BoardCell[] _allBoardCells;

    private UnityEvent _allPawnPlaced = new UnityEvent();
    public UnityEvent AllPawnPlaced { get { return _allPawnPlaced; } }

    private UnityEvent _playerMoveDone = new UnityEvent();
    public UnityEvent PlayerMoveDone { get { return _playerMoveDone; } }

    private UnityEvent _ticTacToe = new UnityEvent();
    public UnityEvent TicTacToe { get { return _ticTacToe; } }

    public void Initialize()
    {
        _deselectButton.onClick.AddListener(OnDeselectClick);
        _allBoardCells = GetComponentsInChildren<BoardCell>();
        foreach (BoardCell cell in _allBoardCells)
            cell.Init();
        GameManager.Instance.OnGameStateChange.AddListener(OnGameStateChange);
    }

    public void OnGameStateChange(Utils.GameState gameState)
    {
        switch (gameState)
        {
            case Utils.GameState.Selection:
                InitSelectionPhase();
                break;
            case Utils.GameState.Dynamic:
                InitDynamicPhase();
                break;
            case Utils.GameState.End:
                InitEndPhase();
                break;
        }
    }

    private void InitSelectionPhase()
    {
        foreach (BoardCell cell in _allBoardCells)
            cell.Button.interactable = true;
        Debug.Log("BoardManager Selection Phase init");
    }

    private void InitDynamicPhase()
    {
        foreach (BoardCell cell in _allBoardCells)
        {
            if (cell.Pawn != null)
                cell.Button.interactable = true;
            else
                cell.Button.interactable = false;
        }
    }

    private void InitEndPhase()
    {
        foreach (BoardCell cell in _allBoardCells)
        {
            cell.Button.interactable = false;
        }
        _deselectButton.interactable = false;
        _deselectButton.gameObject.SetActive(false);
    }

    public void OnCellClick(BoardCell targetCell)
    {
        if (GameManager.Instance.CurrentGameState == Utils.GameState.Selection)
            OnCellClickSelectionMode(targetCell);
        else if (GameManager.Instance.CurrentGameState == Utils.GameState.Dynamic)
            OnCellClickDynamicMode(targetCell);
    }

    private void OnCellClickDynamicMode(BoardCell targetCell)
    {
        // case the player hasn't selected the pawn to move
        if (_selectedCell == null && targetCell.Pawn != null && targetCell.Pawn.PlayerNumber == GameManager.Instance.PlayerIndexTurn)
        {
            _selectedCell = targetCell;
            _selectedCell.DisplayPossibleMove(_selectedCell.Pawn.PawnType, true);
        }

        if (_selectedCell != null)
        {
            // the player select a cell to move the pawn
            if (targetCell.Pawn == null)
            {
                Pawn pawnToMove = _selectedCell.Pawn;
                _selectedCell.DisplayPossibleMove(_selectedCell.Pawn.PawnType, false);
                _selectedCell.UnassignPawn();
                _selectedCell.Button.interactable = false;
                _selectedCell = null;

                targetCell.AssignPawn(pawnToMove);
                targetCell.Button.interactable = true;
                CheckEndTurn();
            }
            // the player select another pawn to move
            else if (targetCell.Pawn.PlayerNumber == GameManager.Instance.PlayerIndexTurn)
            {
                _selectedCell?.DisplayPossibleMove(_selectedCell.Pawn.PawnType, false);
                _selectedCell = targetCell;
                _selectedCell.DisplayPossibleMove(_selectedCell.Pawn.PawnType, true);
            }

        }
    }

    private void OnCellClickSelectionMode(BoardCell targetCell)
    {
        if (targetCell.Pawn != null)
            return;

        if (GameManager.Instance.PlayerIndexTurn == 1)
        {
            targetCell.AssignPawn(_playerOnePawn[_playerOnePawnPlacementCount]);
            _playerOnePawnPlacementCount++;
            Debug.Log("PlayerOne placed is " + _playerOnePawnPlacementCount + " piece");
        }
        else if (GameManager.Instance.PlayerIndexTurn == 2)
        {
            targetCell.AssignPawn(_playerTwoPawn[_playerTwoPawnPlacementCount]);
            _playerTwoPawnPlacementCount++;
            Debug.Log("PlayerTwo placed is " + _playerTwoPawnPlacementCount + " piece");
        }
        targetCell.Button.interactable = false;

        if (CheckForTickTacToe())
            _ticTacToe.Invoke();
        else
        {
            if (_playerOnePawnPlacementCount == 3 && _playerTwoPawnPlacementCount == 3)
                _allPawnPlaced.Invoke();
            else
                _playerMoveDone.Invoke();
        }
    }

    private void OnDeselectClick()
    {
        _selectedCell?.DisplayPossibleMove(_selectedCell.Pawn.PawnType, false);
        _selectedCell = null;
    }

    private bool CheckForTickTacToe()
    {
        if (GameManager.Instance.PlayerIndexTurn == 1)
        {
            return IsDiagonallyAlign(_playerOnePawn[0]) || IsVerticallyAlign(_playerOnePawn[0]) || IsHorizontallyAlign(_playerOnePawn[0]);
        }
        else if (GameManager.Instance.PlayerIndexTurn == 2)
        {
            return IsDiagonallyAlign(_playerTwoPawn[0]) || IsVerticallyAlign(_playerTwoPawn[0]) || IsHorizontallyAlign(_playerTwoPawn[0]);
        }
        return false;
    }

    private bool IsDiagonallyAlign(Pawn pawn)
    {
        int pawnPlayerNumber = pawn.PlayerNumber;
        if (pawn.Cell.TopLeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.BottomRightCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        else if (pawn.Cell.TopRightCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.BottomLeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        else if (pawn.Cell.TopRightCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.TopRightCell?.TopRightCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        else if (pawn.Cell.TopLeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.TopLeftCell?.TopLeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        else if (pawn.Cell.BottomRightCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.BottomRightCell?.BottomRightCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        else if (pawn.Cell.BottomLeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.BottomLeftCell?.BottomLeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        return false;
    }

    private bool IsVerticallyAlign(Pawn pawn)
    {
        int pawnPlayerNumber = pawn.PlayerNumber;
        if (pawn.Cell.TopCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.TopCell?.TopCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        if (pawn.Cell.BottomCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.BottomCell?.BottomCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        if (pawn.Cell.TopCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.BottomCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
     
        return false;
    }

    private bool IsHorizontallyAlign(Pawn pawn)
    {
        int pawnPlayerNumber = pawn.PlayerNumber;
        if (pawn.Cell.RightCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.RightCell?.RightCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        if (pawn.Cell.LeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.LeftCell?.LeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;
        if (pawn.Cell.RightCell?.Pawn?.PlayerNumber == pawnPlayerNumber && pawn.Cell.LeftCell?.Pawn?.PlayerNumber == pawnPlayerNumber)
            return true;

        return false;
    }

    private void CheckEndTurn()
    {
        if (CheckForTickTacToe())
        {
            _ticTacToe.Invoke();
        }
        else
        {
            if (OtherPlayerCanMove(GameManager.Instance.NextPlayerIndexTurn))
            {
                _playerMoveDone.Invoke();
            }
        }
    }

    private bool OtherPlayerCanMove(int nextPlayerIndex)
    {
        if (nextPlayerIndex == 1)
        {
            foreach (Pawn pawn in _playerOnePawn)
            {
                if (pawn.Cell.PawnCanMove(pawn.PawnType))
                    return true;
            }
            return false;
        }
        else if (nextPlayerIndex == 2)
        {
            foreach (Pawn pawn in _playerTwoPawn)
            {
                if (pawn.Cell.PawnCanMove(pawn.PawnType))
                    return true;
            }
            return false;
        }
        return false;
    }
}
