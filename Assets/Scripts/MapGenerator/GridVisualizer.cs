using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GameObject groundPrefab;

    public void VisualizeGrid(int width, int lenght)
    {
        Vector3 position = new Vector3(width / 2f, lenght / 2f, 0);
        //Quaternion rotation = Quaternion.Euler(90, 0, 0);

        GameObject board = Instantiate(groundPrefab, position, Quaternion.identity);
        board.GetComponent<SpriteRenderer>().size = new Vector2(width, lenght);
        //board.transform.position = position;
    }
}
