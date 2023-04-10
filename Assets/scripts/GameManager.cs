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
                if (DidWin(1))
                {
                    Debug.LogWarning("Player 1 win");
                }
            }
            else
            {
                FallingPiece = Instantiate(Player2, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0));
                FallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                Player1Turn = true;
                if (DidWin(2))
                {
                    Debug.LogWarning("Player 2 win");
                }
            }
            if(IsDraw())
            {
                Debug.LogWarning("Draw!");
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
    

    bool DidWin(int PlayerNum)
    {
        // Horizontal
        for(int x =0; x < LenghttOfBoard -3; x++) 
        {
            for(int y =0; y< HeightOfBoard; y++) 
            {
                if (StateBoard[x,y] == PlayerNum && StateBoard[x+1, y] == PlayerNum && StateBoard[x + 2, y] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    return true;
                }
            }
        }

        //Vertical
        for (int x = 0; x < LenghttOfBoard ; x++)
        {
            for (int y = 0; y < HeightOfBoard -3; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x , y+1] == PlayerNum && StateBoard[x , y+2] == PlayerNum && StateBoard[x , y+3] == PlayerNum)
                {
                    return true;
                }
            }
        }

        //y = x line 
        for (int x = 0; x < LenghttOfBoard -3 ; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y+3] == PlayerNum && StateBoard[x + 1, y + 2] == PlayerNum && StateBoard[x + 2, y + 1] == PlayerNum && StateBoard[x + 3, y ] == PlayerNum)
                {
                    return true;
                }
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y + 1] == PlayerNum && StateBoard[x + 2, y + 2] == PlayerNum && StateBoard[x + 3, y+3] == PlayerNum)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool IsDraw()
    {
        for(int x =0; x< LenghttOfBoard;x++)
        {
            if (StateBoard[x,HeightOfBoard -1] == 0)
            {
                return false;
            }
        }
        return true;
    }
}
