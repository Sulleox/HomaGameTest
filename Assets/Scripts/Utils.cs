using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public enum PawnType
    {
        Rook,
        Knight,
        Bishop
    }

    public enum GameState
    {
        Init,
        Selection,
        Dynamic,
        End
    }
}
