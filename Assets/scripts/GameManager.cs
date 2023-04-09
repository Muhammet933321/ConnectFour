using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject[] SpawnLocation;
    public void SelectColumn(int column)
    {
        Debug.Log("Selected Column + " + column);
    }

    public void TakeTurn(int column)
    {
        Instantiate(Player1, SpawnLocation[column].transform.position , Quaternion.identity);
    }
    
}
