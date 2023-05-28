using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    float G = 9.81f;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player1Ghost;
    public GameObject Player2Ghost;
    public GameObject BoardInputs;
    [SerializeField] private AudioSource bumpPiecies;
    [SerializeField] private GameObject partycalEffect;

    [SerializeField] private GameObject BoardInput0;
    [SerializeField] private GameObject BoardInput1;
    [SerializeField] private GameObject BoardInput2;
    [SerializeField] private GameObject BoardInput3;
    [SerializeField] private GameObject BoardInput4;
    [SerializeField] private GameObject BoardInput5;
    [SerializeField] private GameObject BoardInput6;
    [SerializeField] private GameObject UiManagerOBJ;
    [SerializeField] private GameObject WinPointerVertical;
    [SerializeField] private GameObject WinPointerHorizontal;
    [SerializeField] private GameObject WinPointerXY;
    GameObject FallingPiece;
    public TextMeshProUGUI SingleTurnText;




    public GameObject[] SpawnLocation;
    bool Player1Turn;
    public bool CanPlay;

    int HeightOfBoard = 6;
    int LenghttOfBoard = 7;
    private int AI = 2;
    private int PLAYER = 1;

    int[,] StateBoard;









    int[] takeWinPointerPatition(int PlayerNum)
    {
        int[] position = { -1, -1 };

        // Horizontal
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y] == PlayerNum && StateBoard[x + 2, y] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    position[0] = x;
                    position[1] = y;
                    return position;
                }
            }
        }
        //Vertical
        for (int x = 0; x < LenghttOfBoard; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x, y + 1] == PlayerNum && StateBoard[x, y + 2] == PlayerNum && StateBoard[x, y + 3] == PlayerNum)
                {
                    position[0] = x;
                    position[1] = y;
                    return position;
                }
            }
        }
        //y = x line 
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y + 3] == PlayerNum && StateBoard[x + 1, y + 2] == PlayerNum && StateBoard[x + 2, y + 1] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    position[0] = x;
                    position[1] = y;
                    return position;
                }
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y + 1] == PlayerNum && StateBoard[x + 2, y + 2] == PlayerNum && StateBoard[x + 3, y + 3] == PlayerNum)
                {
                    position[0] = x;
                    position[1] = y;
                    return position;
                }
            }
        }
        return position;
    }


    private void Awake()
    {

    }
    public void OnWtoPlayerModeSellected()
    {
        Player1Turn = true;
        SingleTurnText.text = "Player 1 Turn";
        StateBoard = new int[LenghttOfBoard, HeightOfBoard];
        Player1Ghost.SetActive(false);
        Player2Ghost.SetActive(false);
        BoardInputs.SetActive(true);
        BoardInput0.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput1.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput2.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput3.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput4.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput5.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput6.GetComponent<MultiInputFileds>().GameMode = 1;
    }
    private void Start()
    {
        Player1Turn = true;
        StateBoard = new int[LenghttOfBoard, HeightOfBoard];
        Player1Ghost.SetActive(false);
        Player2Ghost.SetActive(false);
        BoardInputs.SetActive(true);
        BoardInput0.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput1.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput2.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput3.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput4.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput5.GetComponent<MultiInputFileds>().GameMode = 1;
        BoardInput6.GetComponent<MultiInputFileds>().GameMode = 1;
        SingleTurnText.text = "Player 1 Turn";



    }


    public void SelectColumn(int column)
    {
        //Debug.Log("Selected Column + " + column);
    }
    public void HoverCloumn(int column)
    {
        if (StateBoard[column, HeightOfBoard - 1] == 0 && (FallingPiece == null || FallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            if (CanPlay)
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
    }
    public void TakeTurn(int column)
    {
        if (StateBoard[column, HeightOfBoard - 1] == 0 && (FallingPiece == null || FallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            if (CanPlay)
            {

                if (UpdateBoardState(column))
                {
                    //Debug.LogWarning("Piecies was moved");
                    Player1Ghost.SetActive(false);
                    Player2Ghost.SetActive(false);
                    if (Player1Turn == true)
                    {
                        FallingPiece = Instantiate(Player1, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0));
                        FallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                        Player1Turn = false;
                        SingleTurnText.text = "Player 2 Turn";
                        Invoke("BumpPiecie", float.Parse(HowTimeDrop(column).ToString()));

                        if (DidWin(PLAYER))
                        {
                            
                            UiManagerOBJ.GetComponent<UiManager>().WinForSingle(1);
                            CanPlay = false;
                            int[] WinPosition;
                            WinPosition = WinHowPositionAndHowType(PLAYER);
                            if (WinPosition[0] != 5)
                            {
                                PointWinner(WinPosition);
                            }
                            //    Debug.LogWarning("Player 1 win");
                        }


                    }
                    else
                    {
                        FallingPiece = Instantiate(Player2, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0));
                        FallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                        Player1Turn = true;
                        SingleTurnText.text = "Player 1 Turn";
                        Invoke("BumpPiecie", float.Parse(HowTimeDrop(column).ToString()));
                        if (DidWin(AI))
                        {
                            
                            UiManagerOBJ.GetComponent<UiManager>().WinForSingle(2);
                            CanPlay = false;
                            int[] WinPosition;
                            WinPosition = WinHowPositionAndHowType(AI);
                            if (WinPosition[0] != 5)
                            {
                                PointWinner(WinPosition);
                            }
                            //    Debug.LogWarning("Player 2 win");
                        }
                    }
                    if (IsDraw())
                    {
                        UiManagerOBJ.GetComponent<UiManager>().WinForSingle(4);
                        CanPlay = false;
                        //Debug.LogWarning("Draw!");
                    }
                }
            }
            else
            {
                //   Debug.LogWarning("Cant Play Now");
            }

        }
    }

    Vector3 PartycalPosition;
    void SpawnPartycalEffect()
    {
        Instantiate(partycalEffect, PartycalPosition, new Quaternion(0, 0, 0, 0));

    }    
    void PointWinner(int[] PositionAndType)
    {
        float Column1sX = 1.393533f;
        float Line1sY = 0.620878f;
        float everyYUp = 0.365f;
        float everyXRight = -0.45f;
        float DefultZ = 0.051f;
        
        if (PositionAndType[0] == 0)
        {
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 0) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 0))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 1) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 0))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 2) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 0))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 3) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 0))), DefultZ);
            SpawnPartycalEffect();
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 0) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2]+ 0))) );
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 1) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2]+ 0))) );
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 2) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2]+ 0))) );
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 3) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2]+ 0))) );
        }
        if (PositionAndType[0] == 1)
        {
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 0) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 0))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 0) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 1))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 0) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 2))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 0) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 3))), DefultZ);
            SpawnPartycalEffect();
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 0) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 0))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 0) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 1))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 0) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 2))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 0) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 3))));
        }
        if (PositionAndType[0] == 2)
        {
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 0) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 3))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 1) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 2))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 2) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 1))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 3) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 0))), DefultZ);
            SpawnPartycalEffect();
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 0) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 3))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 1) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 2))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 2) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 1))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 3) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 0))));
        }
        if (PositionAndType[0] == 3)
        {
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 0) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 0))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 1) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 1))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 2) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 2))), DefultZ);
            SpawnPartycalEffect();
            PartycalPosition = new Vector3((Column1sX + ((PositionAndType[1] + 3) * everyXRight)), (Line1sY + (everyYUp * (PositionAndType[2] + 3))), DefultZ);
            SpawnPartycalEffect();
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 0) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 0))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 1) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 1))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 2) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 2))));
            Debug.Log("WinPointer1 = x =" + (Column1sX + ((PositionAndType[1] + 3) * everyXRight)) + "Y = " + (Line1sY + (everyYUp * (PositionAndType[2] + 3))));
        }
    }
    int[] WinHowPositionAndHowType(int PlayerNum)
    {
        // Horizontal
        int[] Returner = { 0, 0, 0 };
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y] == PlayerNum && StateBoard[x + 2, y] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    Returner[0] = 0;
                    Returner[1] = x;
                    Returner[2] = y;
                    return Returner;
                }
            }
        }
        //Vertical
        for (int x = 0; x < LenghttOfBoard; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x, y + 1] == PlayerNum && StateBoard[x, y + 2] == PlayerNum && StateBoard[x, y + 3] == PlayerNum)
                {
                    Returner[0] = 1;
                    Returner[1] = x;
                    Returner[2] = y;
                    return Returner;
                }
            }
        }
        //y = x line 
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y + 3] == PlayerNum && StateBoard[x + 1, y + 2] == PlayerNum && StateBoard[x + 2, y + 1] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    Returner[0] = 2;
                    Returner[1] = x;
                    Returner[2] = y;
                    return Returner;
                }
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y + 1] == PlayerNum && StateBoard[x + 2, y + 2] == PlayerNum && StateBoard[x + 3, y + 3] == PlayerNum)
                {
                    Returner[0] = 3;
                    Returner[1] = x;
                    Returner[2] = y;
                    return Returner;
                }
            }
        }
        Returner[0] = 5;
        return Returner;
    }
    bool DidWin(int PlayerNum)
    {
        // Horizontal
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y] == PlayerNum && StateBoard[x + 2, y] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    return true;
                }
            }
        }
        //Vertical
        for (int x = 0; x < LenghttOfBoard; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x, y + 1] == PlayerNum && StateBoard[x, y + 2] == PlayerNum && StateBoard[x, y + 3] == PlayerNum)
                {
                    return true;
                }
            }
        }
        //y = x line 
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y + 3] == PlayerNum && StateBoard[x + 1, y + 2] == PlayerNum && StateBoard[x + 2, y + 1] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    return true;
                }
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y + 1] == PlayerNum && StateBoard[x + 2, y + 2] == PlayerNum && StateBoard[x + 3, y + 3] == PlayerNum)
                {
                    return true;
                }
            }
        }
        return false;
    }


    void BumpPiecie()
    {
        bumpPiecies.Play();
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
                //Debug.Log("Column ,Raw = " + column + " , " + Raw);
                return true;
            }
        }
        return false;
    }

    
     
    bool IsDraw()
    {
        for (int x = 0; x < LenghttOfBoard; x++)
        {
            if (StateBoard[x, HeightOfBoard - 1] == 0)
            {
                return false;
            }
        }
        return true;
    }

    public void ClearSingleBoard()
    {
        for (int x = 0; x < HeightOfBoard; x++)
        {
            for (int y = 0; y < LenghttOfBoard; y++)
            {
                StateBoard[y, x] = 0;
            }
        }
      //  Debug.LogError("Board Was Cleared");
    }

    double HowTimeDrop(int column)
    {
        float H = 0;
        for (int x = 0; x < HeightOfBoard; x++)
        {
            if (StateBoard[column, x] == 0)
            {
                H = SpawnLocation[column].transform.position.y - (0.365f *x + 0.62f);
            }
        }
        
        return  Math.Sqrt(2 * H / G)  ;
    }
    

    public void ClearSinglePecies()
    {
        //for(int i = 0 ; i< 42 ; i++)
        //{
            /*if(GameObject.FindGameObjectsWithTag("Piecies")[i] != null)
            {
                Destroy(GameObject.FindGameObjectsWithTag("Piecies")[i]);
                Debug.LogError($"Piecies {i} Cleared");
            }
            else
            {
                Debug.LogError("Piecies Was Cleared");
                break;
            }*/

            GameObject[] destroyObject;
            destroyObject = GameObject.FindGameObjectsWithTag("Piecies");
            foreach (GameObject oneObject in destroyObject)
                Destroy(oneObject);

        //Debug.LogError("Piecies Was Cleared");
       // }
        
        
    }




}
    