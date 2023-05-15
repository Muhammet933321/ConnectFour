using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AiGameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player1Ghost;
    public GameObject Player2Ghost;
    public GameObject BoardInputs;
    [SerializeField] private GameObject BoardInput0;
    [SerializeField] private GameObject BoardInput1;
    [SerializeField] private GameObject BoardInput2;
    [SerializeField] private GameObject BoardInput3;
    [SerializeField] private GameObject BoardInput4;
    [SerializeField] private GameObject BoardInput5;
    [SerializeField] private GameObject BoardInput6;
    [SerializeField] private GameObject UiManagerOBJ;
    GameObject FallingPiece;
    public TextMeshProUGUI SingleTurnText;


    public GameObject[] SpawnLocation;
    public bool Player1Turn;
    public bool CanPlay;

    int HeightOfBoard = 6;
    int LenghttOfBoard = 7;
    private int AI = 2;
    private int PLAYER = 1;


    int[,] StateBoard;
    private void Start()
    {
        Player1Turn = true;
        StateBoard = new int[LenghttOfBoard, HeightOfBoard];
        Player1Ghost.SetActive(false);
        Player2Ghost.SetActive(false);
        BoardInputs.SetActive(true);
        BoardInput0.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput1.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput2.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput3.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput4.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput5.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput6.GetComponent<MultiInputFileds>().GameMode = 2;
        SingleTurnText.text = "Player 1 Turn";



    }
    public void PlayAiGameOnSellected()
    {
        Player1Turn = true;
        SingleTurnText.text = "Player 1 Turn";
        Player1Ghost.SetActive(false);
        Player2Ghost.SetActive(false);
        BoardInputs.SetActive(true);
        BoardInput0.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput1.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput2.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput3.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput4.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput5.GetComponent<MultiInputFileds>().GameMode = 2;
        BoardInput6.GetComponent<MultiInputFileds>().GameMode = 2;
    }
    public void SelectColumn(int column)
    {
        //Debug.Log("Selected Column + " + column);
    }

    
    public void HoverCloumn(int column)
    {
        if (StateBoard[column, HeightOfBoard - 1] == 0 && (FallingPiece == null || FallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            //Debug.LogError("Mouse On Column");
            if (CanPlay)
            {
                //Debug.LogError("CanPlay ");
                if (Player1Turn)
                {
                    Player1Ghost.SetActive(true);
                    Player1Ghost.transform.position = SpawnLocation[column].transform.position;
                }

            }
        }
    }
    private void TakeTurnAi(int column)
    {
        
            if (CanPlay)
            {

                if (UpdateBoardState(column))
                {
                    //Debug.LogWarning("Ai Moved A Piecies");
                    Player1Ghost.SetActive(false);
                    Player2Ghost.SetActive(false);
                    if (Player1Turn == false)
                    {
                        FallingPiece = Instantiate(Player2, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0));
                        FallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                        Player1Turn = true;
                        SingleTurnText.text = "Your Turn";
                        if (DidWinAi(StateBoard, AI))
                        {
                            UiManagerOBJ.GetComponent<UiManager>().WinForSingle(3);
                            CanPlay = false;
                            //Debug.LogWarning("Ai Win");
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
    public void TakeTurn(int column)
    {
        if (StateBoard[column, HeightOfBoard - 1] == 0 && (FallingPiece == null || FallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            if (CanPlay && Player1Turn)
            {

                if (UpdateBoardState(column))
                {
                    //Debug.LogWarning("Piecies was moved");
                    Player1Ghost.SetActive(false);
                    Player2Ghost.SetActive(false);
                    FallingPiece = Instantiate(Player1, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0));
                    FallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                    Player1Turn = false;
                    SingleTurnText.text = "Ai's Turn";
                    //Invoke("RandomAiPlay", 1);
                    Invoke("SimpleAiPlay", 1);
                        
                    if (DidWinAi(StateBoard , PLAYER))
                    {
                        UiManagerOBJ.GetComponent<UiManager>().WinForSingle(1);
                        CanPlay = false;
                        //Debug.LogWarning("Player 1 win");
                    }

                    if (IsDraw())
                    {
                        UiManagerOBJ.GetComponent<UiManager>().WinForSingle(4);
                        CanPlay = false;
                        // Debug.LogWarning("Draw!");
                    }
                }
            }
            else
            {
                //   Debug.LogWarning("Cant Play Now");
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
                //Debug.Log("Column ,Raw = " + column + " , " + Raw);
                return true;
            }
        }
        return false;
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
    private bool DidWinAi(int[,] board, int PlayerNum)
    {
        // Horizontal
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard; y++)
            {
                if (board[x, y] == PlayerNum && board[x + 1, y] == PlayerNum && board[x + 2, y] == PlayerNum && board[x + 3, y] == PlayerNum)
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
                if (board[x, y] == PlayerNum && board[x, y + 1] == PlayerNum && board[x, y + 2] == PlayerNum && board[x, y + 3] == PlayerNum)
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
                if (board[x, y + 3] == PlayerNum && board[x + 1, y + 2] == PlayerNum && board[x + 2, y + 1] == PlayerNum && board[x + 3, y] == PlayerNum)
                {
                    return true;
                }
                if (board[x, y] == PlayerNum && board[x + 1, y + 1] == PlayerNum && board[x + 2, y + 2] == PlayerNum && board[x + 3, y + 3] == PlayerNum)
                {
                    return true;
                }
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
         //Debug.LogError("Board Was Cleared");
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


    // FROM CHAT GPT

    private bool IsColumnFull(int[,] board , int column)
    {
        if (board[column, 5] == 0)
        {
            //Debug.LogError($"Column {column} Is Not Full");
            return false;
        }
            
        else
        {
            //Debug.LogError($"Column {column} Is Full");
            return true;
        }
            
    }

    private void MakeMove(int[,] board, int column, int player)
    {
        for (int i = HeightOfBoard - 1; i >= 0; i--)
        {
            if (board[i, column] == 0)
            {
                board[i, column] = player;
                break;
            }
        }
    }


    private int[,] CopyBoard(int[,] board)
    {
        int[,] newBoard = new int[LenghttOfBoard, HeightOfBoard];

        for (int i = 0; i < LenghttOfBoard; i++)
        {
            for (int j = 0; j < HeightOfBoard; j++)
            {
                newBoard[i, j] = board[i, j];
            }
        }

        return newBoard;
    }

    private void RandomAiPlay()
    {
        int RandomMove;
       // Debug.LogWarning("RandomPlay");
        for(int  i= 0; i< 100; i ++)
        {
            if( i == 99)
            {
                //Debug.LogError("For Couldn't Find a Move");
                for(int y = 0; y < LenghttOfBoard; y++)
                {
                    if (IsColumnFull(StateBoard, y))
                    {
                        
                    }
                    else
                    {
                       // Debug.Log("Called Take Ai");
                        TakeTurnAi(y);
                        break;
                    }
                        

                }
                //Debug.LogError("It is Most DRAW");
            }
            RandomMove = Random.Range(0, LenghttOfBoard -1);
            if(IsColumnFull(StateBoard , RandomMove))
            {
                continue;
            }
            else
            {
                //Debug.Log("Called Take Turn");
                TakeTurnAi(RandomMove);
                break ;
            }
        }
        
    }

    private void SimpleAiPlay()
    {
        int BestMove =  FindBestMove(StateBoard);
        if (BestMove == -1)
        {
            RandomAiPlay();
        }
        else if(BestMove >= 7  && BestMove < 14)
        {
            //RandomAiPlayBut(BestMove - 7);
        }
        else 
        TakeTurnAi(BestMove);

    }
    private int FindBestMove(int[,] board)
    {
        int[,] NewBoard = new int[LenghttOfBoard , HeightOfBoard] ;
        int[,] NextBoard = new int[LenghttOfBoard, HeightOfBoard];
        int[] NaverMoves = new int[7];
        
        //Debug.LogError("Finding Best Move");
        ///
        /// Find Ai Win Now
        ///
        for (int i = 0; i < LenghttOfBoard; i++)
        {
            NewBoard = CopyBoard(board);
             
            

            if (IsColumnFull(NewBoard, i))
            {
                //Debug.LogWarning($"Column {i} Is Full");
                continue;
            }
            else
            {
                UpdateBoard(NewBoard, i, AI);
                if (DidWinAi(NewBoard, AI))
                {
                    //Debug.Log($"Ai Win On 1 Move This Move is { i }");
                    return i;

                }
            }
        }

        ///
        /// Enemy Can Win Now ?
        ///
        for (int i = 0; i < LenghttOfBoard; i++)
        {
            NewBoard = CopyBoard(board);
            


            if (IsColumnFull(NewBoard, i))
            {
                //Debug.LogWarning($"Column {i} Is Full");
                continue;
            }
            else
            {
                UpdateBoard(NewBoard, i, PLAYER);
                if (DidWinAi(NewBoard, PLAYER))
                {
                    //Debug.Log($"Enemy Win On 1 Move This Move is {i}");
                    return i;

                }
            }
        }

        ///
        /// Is He Can Win 3 Play
        ///


        NewBoard = CopyBoard(StateBoard);

        for (int y = 0; y < LenghttOfBoard - 3; y++)
        {
            for (int z = 0; z < HeightOfBoard -1; z++)
            {
                if (NewBoard[y, z] == 0 && NewBoard[y + 1, z] == PLAYER && NewBoard[y + 2, z] == PLAYER && NewBoard[y + 3, z] == 0)
                {
                    if(z == 0 )
                    {
                        //Debug.Log("He Have 2 Horizontal Piecies On floor Do it ");
                        return y;
                    }
                    else if((NewBoard[y, z -1] == PLAYER || NewBoard[y, z - 1] == AI) && ( NewBoard[y + 3, z-1] == PLAYER || NewBoard[y + 3, z - 1] == AI))
                    {
                        //Debug.Log("He Have 2 Horizontal Piecies On Up Do it ");
                        return y;
                    }
                    

                }
            }
        }


        ///
        ///If I Play It Can Hi Win
        ///
       
        
        for (int i = 0; i < LenghttOfBoard; i++)
        {
            NewBoard = CopyBoard(board);
            



            if (IsColumnFull(NewBoard, i))
            {
                //Debug.LogWarning($"Column {i} Is Full");
                continue;
            }

            else
            {
                UpdateBoard(NewBoard, i, AI);
                //Debug.Log($"if I Move Column {i}");

                for (int j = 0; j < LenghttOfBoard; j++)
                {
                    NextBoard = CopyBoard(NewBoard);
                    
                    if (IsColumnFull(NextBoard, j))
                    {
                        //Debug.LogWarning($"Column {j} Is Full");
                        continue;
                    }
                    else
                    {
                        UpdateBoard(NextBoard, j, PLAYER);

                        //Debug.Log($"NextBoard = {NextBoard[i, 0]}");

                        if (DidWinAi(NextBoard, PLAYER))
                        {
                            //Debug.Log($"If I Player Column {i} . He Play Column {j} And He Win");
                            NaverMoves[i] = 1;
                            

                        }
                        
                    }
                }
            }
        }
        //Debug.LogError("Cant Find A Greet Move");
        for(int i = 0 ; i < LenghttOfBoard ; i++)
        {
            if (NaverMoves[i] == 1)
            {
                RandomAiPlayButnew(NaverMoves);
                return 9;
            }
        }
        
        return -1 ;
        
    }

    private void RandomAiPlayButnew(int[] NaverMove)
    {
        int RandomMove;
       //Debug.LogWarning("RandomPlayBut");
        for (int i = 0; i < 100; i++)
        {
            if (i == 99)
            {
                //Debug.LogError("Couldn't Find a Move");
                for (int y = 0; y < LenghttOfBoard; y++)
                {
                    if (IsColumnFull(StateBoard, y))
                    {
                        continue;
                    }
                    else
                    {
                        
                        if (NaverMove[y] == 1)
                        {
                            //Debug.Log("I will lose but Must Play here");
                            TakeTurnAi(y);
                            break;
                        }
                        else
                        {
                            TakeTurnAi(y);
                            break;
                        }

                    }



                }

            }
            RandomMove = Random.Range(0, LenghttOfBoard - 1);
            if (IsColumnFull(StateBoard, RandomMove) || NaverMove[RandomMove] == 1)
            {
                continue;
            }
            else
            {
                //Debug.Log($"If i Play {RandomMove} He Cant Win");
                TakeTurnAi(RandomMove);
                break;
            }
        }

    }

    private void UpdateBoard(int[,] board ,int column , int Playernum)
    {


        for (int Raw = 0; Raw < HeightOfBoard; Raw++)
        {
            if (board[column, Raw] == 0)
            {
                board[column, Raw] = Playernum;
                //Debug.Log("Column ,Raw = " + column + " , " + Raw);
                break;
            }
        }
    }
}
