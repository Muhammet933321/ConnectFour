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
        if (StateBoard[column, HeightOfBoard - 1] == 0 && (FallingPiece == null || FallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
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
                    else

                    if (IsDraw())
                    {
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
                        SingleTurnText.text = "Ai's Turn";
                        //Invoke("RandomAiPlay", 1);
                        Invoke("SimpleAiPlay", 1);
                        
                        if (DidWinAi(StateBoard , PLAYER))
                        {
                            UiManagerOBJ.GetComponent<UiManager>().WinForSingle(1);
                            CanPlay = false;
                            //Debug.LogWarning("Player 1 win");
                        }
                    }
                    else

                    if (IsDraw())
                    {
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
        //  Debug.LogError("Board Was Cleared");
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

    public int evaluate(int[,] board, int player)
    {
        int opponent = (player == 1) ? 2 : 1; // rakip oyuncunun kimliði
        int score = 0;

        // Satýrlarý kontrol et
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1) - 3; col++)
            {
                int countPlayer = 0;
                int countOpponent = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (board[row, col + i] == player)
                    {
                        countPlayer++;
                    }
                    else if (board[row, col + i] == opponent)
                    {
                        countOpponent++;
                    }
                }
                score += evaluateLine(countPlayer, countOpponent);
            }
        }

        // Sütunlarý kontrol et
        for (int col = 0; col < board.GetLength(1); col++)
        {
            for (int row = 0; row < board.GetLength(0) - 3; row++)
            {
                int countPlayer = 0;
                int countOpponent = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (board[row + i, col] == player)
                    {
                        countPlayer++;
                    }
                    else if (board[row + i, col] == opponent)
                    {
                        countOpponent++;
                    }
                }
                score += evaluateLine(countPlayer, countOpponent);
            }
        }

        // Diyagonal (sol üstten sað alta) kontrol et
        for (int row = 0; row < board.GetLength(0) - 3; row++)
        {
            for (int col = 0; col < board.GetLength(1) - 3; col++)
            {
                int countPlayer = 0;
                int countOpponent = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (board[row + i, col + i] == player)
                    {
                        countPlayer++;
                    }
                    else if (board[row + i, col + i] == opponent)
                    {
                        countOpponent++;
                    }
                }
                score += evaluateLine(countPlayer, countOpponent);
            }
        }

        // Diyagonal (sað üstten sol alta) kontrol et
        for (int row = 0; row < board.GetLength(0) - 3; row++)
        {
            for (int col = 3; col < board.GetLength(1); col++)
            {
                int countPlayer = 0;
                int countOpponent = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (board[row + i, col - i] == player)
                    {
                        countPlayer++;
                    }
                    else if (board[row + i, col - i] == opponent)
                    {
                        countOpponent++;
                    }
                }
                score += evaluateLine(countPlayer, countOpponent);
            }
        }

        return score;
    }

    private int evaluateLine(int countPlayer, int countOpponent)
    {
        int score = 0;
        if (countPlayer == 4)
        {
            score += 1000;
        }
        else if (countPlayer == 3 && countOpponent == 0)
        {
            score += 5;
        }
        else if (countPlayer == 2 && countOpponent == 0)
        {
            score += 2;
        }
        else if (countPlayer == 1 && countOpponent == 0)
        {
            score += 1;
        }

        if (countOpponent == 4)
        {
            score -= 1000;
        }
        else if (countOpponent == 3 && countPlayer == 0)
        {
            score -= 5;
        }
        else if (countOpponent == 2 && countPlayer == 0)
        {
            score -= 2;
        }
        else if (countOpponent == 1 && countPlayer == 0)
        {
            score -= 1;
        }

        return score;

    }

    private int Minimax(int[,] board, int depth, bool maximizing_player)
    {
        int score = evaluate(board , AI);

        if (depth == 0 || score == 1000 || score == -1000)
        {
            return score;
        }

        if (maximizing_player)
        {
            int bestScore = -10000;

            for (int i = 0; i < LenghttOfBoard; i++)
            {
                if (IsColumnFull(board, i))
                {
                    continue;
                }

                int[,] newBoard = CopyBoard(board);
                MakeMove(newBoard, i, PLAYER);

                int currentScore = Minimax(newBoard, depth - 1, false);
                bestScore = Mathf.Max(bestScore, currentScore);
            }

            return bestScore;
        }
        else
        {
            int bestScore = 10000;

            for (int i = 0; i < LenghttOfBoard; i++)
            {
                if (IsColumnFull(board, i))
                {
                    continue;
                }

                int[,] newBoard = CopyBoard(board);
                MakeMove(newBoard, i, AI);

                int currentScore = Minimax(newBoard, depth - 1, true);
                bestScore = Mathf.Min(bestScore, currentScore);
            }

            return bestScore;
        }
    }
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
        Debug.LogWarning("RandomPlay");
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
                        Debug.Log("Called Take Ai");
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
                Debug.Log("Called Take Turn");
                TakeTurnAi(RandomMove);
                break ;
            }
        }
        
    }
    private void RandomAiPlayBut(int NaverMove)
    {
        int RandomMove;
        Debug.LogWarning("RandomPlayBut");
        for (int i = 0; i < 100; i++)
        {
            if (i == 99)
            {
                Debug.LogError("For Couldn't Find a Move");
                for (int y = 0; y < LenghttOfBoard; y++)
                {
                    if (IsColumnFull(StateBoard, y) || NaverMove == y)
                    {
                        continue;
                    }
                    else
                    {
                        Debug.Log("Called Take Ai");
                        TakeTurnAi(y);
                        break ;

                    }
                        
                    

                }
                
            }
            RandomMove = Random.Range(0, LenghttOfBoard - 1);
            if (IsColumnFull(StateBoard, RandomMove) || NaverMove == RandomMove)
            {
                continue;
            }
            else
            {
                Debug.Log("Called Take Ai");
                TakeTurnAi(RandomMove);
                break;
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
            RandomAiPlayBut(BestMove - 7);
        }
        else 
        TakeTurnAi(FindBestMove(StateBoard));

    }
    private int FindBestMove(int[,] board)
    {
        int[,] NewBoard = new int[LenghttOfBoard , HeightOfBoard] ;
        int[,] NextBoard = new int[LenghttOfBoard, HeightOfBoard];
        
        Debug.LogError("Finding Best Move");
        ///
        /// Find Ai Win Now
        ///
        for (int i = 0; i < LenghttOfBoard; i++)
        {

            for(int k = 0; k < LenghttOfBoard; k++)
            {
                for (int j  = 0; j < HeightOfBoard; j++)
                {
                    NewBoard[k,j] = board[k,j];
                }
            }
            

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
                    //Debug.LogWarning($"Ai Win On 1 Move This Move is { i }");
                    return i;

                }
            }
        }

        ///
        /// Enemy Can Win Now ?
        ///
        for (int i = 0; i < LenghttOfBoard; i++)
        {

            for (int k = 0; k < LenghttOfBoard; k++)
            {
                for (int j = 0; j < HeightOfBoard; j++)
                {
                    NewBoard[k, j] = board[k, j];
                }
            }


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
                    //Debug.LogWarning($"Enemy Win On 1 Move This Move is {i}");
                    return i;

                }
            }
        }

        ///
        ///If I Play It Can Hi Win
        ///
        /*
        Debug.LogWarning("-----------------------------------");

        for (int i = 0; i < LenghttOfBoard; i++)
        {

            for (int k = 0; k < LenghttOfBoard; k++)
            {
                for (int j = 0; j < HeightOfBoard; j++)
                {
                    NewBoard[k, j] = board[k, j];
                }
            }
            Debug.LogError($"1.deep = {i}");
            UpdateBoard(NewBoard, i, AI);
            
            for (int j = 0; j < LenghttOfBoard; j ++)
            {
                for (int l = 0; l < LenghttOfBoard; l++)
                {
                    for (int v = 0; v < HeightOfBoard; v++)
                    {
                        NextBoard[l, v] = NewBoard[l, v];
                    }
                }
                Debug.LogError($"2.deep = {j}");
                UpdateBoard(NextBoard, j, PLAYER);
                 
                

                if (DidWinAi(NextBoard, PLAYER))
                {
                   Debug.LogError($"If I Player Column {i} . He Play Column {j} And He Win");

                   return i;

                }
                
            }
               
            
        }
        Debug.LogError("Canf Find A Greet Move");
        return -1;

        */
        
        for (int i = 0; i < LenghttOfBoard; i++)
        {

            for (int k = 0; k < LenghttOfBoard; k++)
            {
                for (int j = 0; j < HeightOfBoard; j++)
                {
                    NewBoard[k, j] = board[k, j];
                }
            }



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
                    for (int l = 0; l < LenghttOfBoard; l++)
                    {
                        for (int v = 0; v < HeightOfBoard; v++)
                        {
                            NextBoard[l, v] = NewBoard[l, v];
                        }
                    }
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
                            Debug.LogError($"If I Player Column {i} . He Play Column {j} And He Win");

                            return i+7;

                        }
                        
                    }
                }
            }
        }
        Debug.LogError("Canf Find A Greet Move");
        return -1;
        
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
