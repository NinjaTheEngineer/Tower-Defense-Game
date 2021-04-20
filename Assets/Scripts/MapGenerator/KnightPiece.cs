using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPiece
{
    public static List<Vector3> listOfPossibleMoves = new List<Vector3>
    {
        new Vector3(-1.5f, 2.5f, 0),
        new Vector3(1.5f, 2, 0),
        new Vector3(-1, -2.5f, 0),
        new Vector3(1, -2, 0),
        new Vector3(-2.5f, -1.5f, 0),
        new Vector3(-2, 1.5f, 0),
        new Vector3(2.5f, -1, 0),
        new Vector3(2, 1, 0)
    };

    private Vector3 position;

    public Vector3 Position { get => position; set => position = value; }

    public KnightPiece(Vector3 position)
    {
        this.Position = position;
    }
}
