using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Utils.PawnType _pawnType;
    [SerializeField] private int _playerNumber;
    public Utils.PawnType PawnType { get { return _pawnType; } }

    private BoardCell _currentBoardCell;
    public BoardCell Cell { get { return _currentBoardCell; } }

    public int PlayerNumber
        { get { return _playerNumber; } }

    public void AssignCell(BoardCell cell)
    {
        _currentBoardCell = cell;
    }
}
