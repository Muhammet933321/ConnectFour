using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player1Ghost;
    public GameObject Player2Ghost;
    GameObject FallingPiece;

    public GameObject[] SpawnLocation;
    bool Player1Turn;
    int HeightOfBoard = 6;
    int LenghttOfBoard = 7;
    

    int[,] StateBoard;
    private void Start()
    {
        Player1Turn = true;
        StateBoard = new int[LenghttOfBoard, HeightOfBoard];
        Player1Ghost.SetActive(false);
        Player2Ghost.SetActive(false);
    }
    public void SelectColumn(int column)
    {
        //Debug.Log("Selected Column + " + column);
    }

    public void HoverCloumn(int column) 
    {
        if (StateBoard[column , HeightOfBoard -1 ] == 0 && (FallingPiece == null || FallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            if (Player1Turn)
            {
                Player1Ghost.SetActive(true);
                Player1Ghost.transform.position = SpawnLocation[column].transform.position;
            }
            else
            {
                Player2Ghost.SetActive(true);
                Player2Ghost.transform.position = SpawnLocation[column].transform.position;
            }

        }
        
    }
    public void TakeTurn(int column)
    {
        if(UpdateBoardState(column))
        {
            Player1Ghost.SetActive(false);
            Player2Ghost.SetActive(false);
            if (Player1Turn == true)
            {
                FallingPiece = Instantiate(Player1, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0));
                FallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0,0.1f,0);
                Player1Turn = false;
            }
            else
            {
                FallingPiece = Instantiate(Player2, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0));
                FallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                Player1Turn = true;
            }
            
        }
        

    }
    bool UpdateBoardState(int column)
    {
        for (int Raw = 0; Raw < HeightOfBoard; Raw++)
        {
            if (StateBoard[column, Raw] == 0)
            {
                if (Player1Turn)
                {
                    StateBoard[column, Raw] = 1;
                }
                else
                {
                    StateBoard[column, Raw] = 2;
                }
                Debug.Log("Column ,Raw = " + column + " , " + Raw);
                return true;
            }
        }
        return false;
    }
    
    
}
