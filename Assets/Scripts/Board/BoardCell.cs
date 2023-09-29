using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Utils;

public class BoardCell : MonoBehaviour
{
    [SerializeField] private BoardCell _topCell;
    public BoardCell TopCell
    { get { return _topCell; } }

    [SerializeField] private BoardCell _bottomCell;
    public BoardCell BottomCell
    { get { return _bottomCell; } }

    [SerializeField] private BoardCell _leftCell;
    public BoardCell LeftCell 
    { get { return _leftCell; } }

    [SerializeField] private BoardCell _rightCell;
    public BoardCell RightCell
    { get { return _rightCell; } }

    public BoardCell TopLeftCell
    {
        get { return _topCell?.LeftCell; }
    }

    public BoardCell TopRightCell
    {
        get { return _topCell?.RightCell; }
    }

    public BoardCell BottomLeftCell
    {
        get { return _bottomCell?.LeftCell; }
    }

    public BoardCell BottomRightCell
    {
        get { return _bottomCell?.RightCell; }
    }


    [SerializeField] private RawImage _highlight;

    private BoardManager _boardManager;
    private UnityEngine.UI.Button _button;
    public UnityEngine.UI.Button Button
    { get { return _button; } }

    private bool _isPawnDestination = false;

    private Pawn _pawn = null;
    public Pawn Pawn 
    { get { return _pawn; } }

    private List<BoardCell> _possibleRookMove = new List<BoardCell>();
    private List<BoardCell> _possibleKnightMove = new List<BoardCell>();
    private List<BoardCell> _possibleBishopMove = new List<BoardCell>();


    public void Init()
    {
        _button = GetComponent<UnityEngine.UI.Button>();
        _button.interactable = false;
        _button.onClick.AddListener(OnClick);
        _boardManager = GetComponentInParent<BoardManager>();
        ComputeRookDestination();
        ComputeBishopDestination();
        ComputeKnightDestination();
    }

    private void ComputeRookDestination()
    {
        if (_topCell)
        {
            _possibleRookMove.Add(_topCell);
            if (_topCell.TopCell)
                _possibleRookMove.Add(_topCell.TopCell);
        }

        if (_bottomCell)
        {
            _possibleRookMove.Add(_bottomCell);
            if (_bottomCell.BottomCell)
                _possibleRookMove.Add(_bottomCell.BottomCell);
        }

        if (_leftCell)
        {
            _possibleRookMove.Add(_leftCell);
            if (_leftCell.LeftCell)
                _possibleRookMove.Add(_leftCell.LeftCell);
        }

        if (_rightCell)
        {
            _possibleRookMove.Add(_rightCell);
            if (_rightCell.RightCell)
                _possibleRookMove.Add(_rightCell.RightCell);
        }
    }

    private void ComputeBishopDestination()
    {

        if (_topCell && _topCell.LeftCell)
        {
            _possibleBishopMove.Add(_topCell.LeftCell);
            if (_topCell.LeftCell.TopCell && _topCell.LeftCell.TopCell.LeftCell)
                _possibleBishopMove.Add(_topCell.LeftCell.TopCell.LeftCell);
        }

        if (_topCell && _topCell.RightCell)
        {
            _possibleBishopMove.Add(_topCell.RightCell);
            if (_topCell.RightCell.TopCell && _topCell.RightCell.TopCell.RightCell)
                _possibleBishopMove.Add(_topCell.RightCell.TopCell.RightCell);
        }


        if (_bottomCell && _bottomCell.LeftCell)
        {
            _possibleBishopMove.Add(_bottomCell.LeftCell);
            if (_bottomCell.LeftCell._bottomCell && _bottomCell.LeftCell._bottomCell.LeftCell)
                _possibleBishopMove.Add(_bottomCell.LeftCell.BottomCell.LeftCell);
        }

        if (_bottomCell && _bottomCell.RightCell)
        {
            _possibleBishopMove.Add(_bottomCell.RightCell);
            if (_bottomCell.RightCell.BottomCell && _bottomCell.RightCell.BottomCell.RightCell)
                _possibleBishopMove.Add(_bottomCell.RightCell.BottomCell.RightCell);
        }
    }

    private void ComputeKnightDestination()
    {
        if (_topCell && _topCell.TopCell)
        {
            if (_topCell.TopCell.LeftCell)
                _possibleKnightMove.Add(_topCell.TopCell.LeftCell);
            if (_topCell.TopCell.RightCell)
                _possibleKnightMove.Add(_topCell.TopCell.RightCell);
        }

        if (_leftCell && _leftCell.LeftCell)
        {
            if (_leftCell.LeftCell.TopCell)
                _possibleKnightMove.Add(_leftCell.LeftCell.TopCell);
            if (_leftCell.LeftCell.BottomCell)
                _possibleKnightMove.Add(_leftCell.LeftCell.BottomCell);
        }

        if (_bottomCell && _bottomCell.BottomCell)
        {
            if (_bottomCell.BottomCell.LeftCell)
                _possibleKnightMove.Add(_bottomCell.BottomCell.LeftCell);
            if (_bottomCell.BottomCell.RightCell)
                _possibleKnightMove.Add(_bottomCell.BottomCell.RightCell);
        }

        if (_rightCell && _rightCell.RightCell)
        {
            if (_rightCell.RightCell.TopCell)
                _possibleKnightMove.Add(_rightCell.RightCell.TopCell);
            if (_rightCell.RightCell.BottomCell)
                _possibleKnightMove.Add(_rightCell.RightCell.BottomCell);
        }
    }

    public void DisplayPossibleMove(PawnType pawnType, bool state)
    {
        switch (pawnType)
        {
            case PawnType.Rook:
                DisplayRookMove(state);
                break;
            case PawnType.Knight:
                DisplayKnightMove(state);
                break;
            case PawnType.Bishop:
                DisplayBishopMove(state);
                break; 
        }
    }

    public bool PawnCanMove(PawnType pawnType)
    {
        switch (pawnType)
        {
            case PawnType.Rook:
                return CanRookMove();
            case PawnType.Knight:
                return CanKnightMove();
            case PawnType.Bishop:
                return CanBishopMove();
        }
        return false;
    }

    private bool CanRookMove()
    {
        foreach (BoardCell cell in _possibleRookMove)
        {
            if (cell.Pawn == null)
                return true;
        }
        return false;
    }

    private bool CanBishopMove()
    {
        foreach (BoardCell cell in _possibleBishopMove)
        {
            if (cell.Pawn == null)
                return true;
        }
        return false;
    }

    private bool CanKnightMove()
    {
        foreach (BoardCell cell in _possibleKnightMove)
        {
            if (cell.Pawn == null)
                return true;
        }
        return false;
    }

    private void DisplayRookMove(bool state)
    {
        foreach (BoardCell cell in _possibleRookMove)
        {
            if (cell.Pawn == null)
                cell.SetHighlightState(state);
        }
    }

    private void DisplayBishopMove(bool state)
    {
        foreach (BoardCell cell in _possibleBishopMove)
        {
            if (cell.Pawn == null)
                cell.SetHighlightState(state);
        }
    }

    private void DisplayKnightMove(bool state)
    {
        foreach (BoardCell cell in _possibleKnightMove)
        {
            if (cell.Pawn == null)
                cell.SetHighlightState(state);
        }
    }

    public void SetHighlightState(bool state)
    {
        //to be a pawn destination it requires the cell to be empty
        _highlight.gameObject.SetActive(state);
        _button.interactable = state;
        _isPawnDestination = state;
    }

    public void OnClick()
    {
        _boardManager.OnCellClick(this);
    }

    public void AssignPawn(Pawn pawn)
    {
        _pawn = pawn;
        _pawn.transform.parent = transform;
        _pawn.transform.localPosition = Vector2.zero;
        _pawn.transform.SetAsLastSibling();
        _pawn.AssignCell(this);
    }

    public void UnassignPawn()
    {
        _pawn = null;
    }
}
