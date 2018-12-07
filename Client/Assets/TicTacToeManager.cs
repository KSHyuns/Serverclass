using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlayerType { PlayerOne, PlayerTwo }
public enum GameState
{
    None,
    PlayerTurn,
    OpponentTurn,
    GameOver,
    OpponentDisconnect,
}
public enum Player { Player, Opponent }

public class TicTacToeManager : MonoBehaviour
{
    public Sprite circleSprite;     // O 스프라이트
    public Sprite crossSprite;      // X 스프라이트
    public GameObject[] cells;      // Cell 정보를 담을 배열
    public RectTransform gameoverPanel;


    enum Mark { Circle, Cross }
    PlayerType playerType;
    int[] cellStates = new int[9]; // o,x 값 정보
    TicTacToeNetwork networkManager;

    int rowNum = 3;


    // 게임 승패
    enum Winner { None, Player, Opponent, Tie }

    // 게임 상태
    GameState gameState;





    private void Awake()
    {
        gameState = GameState.None;
    }

    private void Start()
    {
        networkManager = GetComponent<TicTacToeNetwork>();

        // cell 정보 배열 초기화
        for (int i = 0; i < cellStates.Length; i++)
        {
            cellStates[i] = -1;
        }
    }

    private void Update()
    {
        if (gameState == GameState.PlayerTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos =
                    new Vector2(Input.mousePosition.x,
                    Input.mousePosition.y);
                RaycastHit2D hitInfo =
                    Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos),
                    Vector2.zero);

                if (hitInfo)
                {
                    GameObject cell = hitInfo.transform.gameObject;
                    int cellIndex = int.Parse(cell.name);


                    if (cellStates[cellIndex] != -1) return;

                    // 서버에 Cell Index 전달
                    networkManager.DoPlayer(cellIndex);

                    // TODO: 셀에 O 혹은 X 표시하기
                    DrawMark(cellIndex, Player.Player);

                }
            }
        }
    }

    public void DrawMark(int cellIndex, Player player)
    {
        

        Sprite markSprite;
        
        if (player == Player.Player)
        {
            markSprite =
                playerType == PlayerType.PlayerOne ? circleSprite : crossSprite;

            cellStates[cellIndex] =
                playerType == PlayerType.PlayerOne ? 0 : 1;
        }
        else
        {
            markSprite =
                playerType == PlayerType.PlayerOne ? crossSprite : circleSprite;

            cellStates[cellIndex] =
                playerType == PlayerType.PlayerOne ? 1 : 0;
        }

        // 해당 Cell에 Sprite 할당하기
        //cells[cellIndex].GetComponent<SpriteRenderer>().sprite = markSprite;
        // 할당된 Cell을 터치가 불가능하게 비활성
        //cells[cellIndex].GetComponent<BoxCollider2D>().enabled = false;

        GameObject go = cells[cellIndex];
        go.GetComponent<SpriteRenderer>().sprite = markSprite;
        go.GetComponent<BoxCollider2D>().enabled = false;

        //해당 애니메이션 효과가 끝났을때      실행하는 람다식 
        go.transform.DOScale(2, 0.5f).OnComplete(() =>
        {   
            go.transform.DOScale(1, 1).SetEase(Ease.OutBounce);
            go.GetComponent<SpriteRenderer>().DOFade(1, 1);


            //카메라 흔들기 
            Camera.main.DOShakePosition(0.5f, 1, 15);       //시간  범위와 길이   
        });
        

        // 턴 변경
        // 1. 게임이 계속 진행 중이어야 한다.
        Winner result = CheckWinner();

        if (result == Winner.None)
        {
            if (gameState == GameState.PlayerTurn)
            {
                gameState = GameState.OpponentTurn;
            }
            else if (gameState == GameState.OpponentTurn)
            {
                gameState = GameState.PlayerTurn;
            }
        }
        else
        {
            gameState = GameState.GameOver;

            //게임오버패널
            ShowGameover();


            // TODO: 게임의 결과 화면에 표시
            if (result == Winner.Player)
            {
                Debug.Log("Player Win!");
            }
            else if (result == Winner.Opponent)
            {
                Debug.Log("Opponent Win!");
            }
            else if (result == Winner.Tie)
            {
                Debug.Log("Tie");
            }

        }

    }

    // 승패 확인
    Winner CheckWinner()
    {
        int playerTypeValue = (int)playerType;

        //가로 체크

        for (int y = 0; y < rowNum; y++)
        {
            int mark = cellStates[y * rowNum];              //0 3 5 자리의 인덱스를 mark에 넣는다.  0 아니면 1 아니면 -1
            int num = 0;
            for (int x = 0; x < rowNum; x++)                //y 가 0일때  y가 1일때   y가 2일때
            {
                int index = y * rowNum + x;                 //0 1 2      3 4 5       6 7 8
                if (mark == cellStates[index])              
                {
                    ++num;
                }
            }
            if (mark != -1 && num == rowNum)                //가로 한줄에 0이 3개라면
            {
                int markValue = (int)mark;                  
                return (playerTypeValue == markValue) ? Winner.Player : Winner.Opponent;
            }
        }

        //세로 체크
        for (int x = 0; x < rowNum; x++)
        {
            int mark = cellStates[x];                       //0 1 2 자리의 인덱스 번호를 mark에 넣는다. 0 아니면 1 아니면 -1
            int num = 0;
            for (int y = 0; y < rowNum; y++)                //x 가 0일때  x가 1일때   x가 2일때
            {
                int index = y * rowNum + x;                 //0 3 6      1 4 7       2 5 8
                if (mark == cellStates[index])
                {
                    ++num;
                }
            }
            if (mark != -1 && num == rowNum)                //세로 한줄이 0이 3개라면 
            {
                int markValue = (int)mark;
                return (playerTypeValue == markValue) ? Winner.Player : Winner.Opponent;
            }
        }


        //대각선 좌에서 우
        {
            int mark = cellStates[0];
            int num = 0;
            for (int xy = 0; xy < rowNum; xy++)
            {
                int index = xy * rowNum + xy;
                if (mark == cellStates[index])
                {
                    ++num;
                }
            }
            if (mark != -1 && num == rowNum)
            {
                int markValue = (int)mark;
                return (playerTypeValue == markValue) ? Winner.Player : Winner.Opponent;
            }
        }
        //대각선 우에서 좌
        {
            int mark = cellStates[rowNum - 1];          //2번째 있는 배열에 들어있는 값 0아니면 1  아니면 -1
            int num = 0;
            for (int xy = 0; xy < rowNum; xy++)            //xy 0일때  xy가 1일때  xy 2일때
            {
                int index = xy * rowNum + rowNum - xy - 1; //2        4          6
                if (mark == cellStates[index])             //선택을 햇다면 0이나 1인데 한줄을 검사를 해서 
                {
                    ++num;
                }
            }
            if (mark != -1 && num == rowNum)
            {
                int markValue = (int)mark;
                return (playerTypeValue == markValue) ? Winner.Player : Winner.Opponent;
            }
        }
        //무승부
        {
            //플레이어들이 선택을 하면 cellStates배열안에 0이나 1이 들어가는데 위 코드에서 함수를 빠져나가지 않고 아래가 실행됫는데
            //-1이 하나도 없게 된다면 셀을 전부 선택한게된다.
            //전부 선택해도 승패를 알수없는데  선택을 안한 -1이 없다면 셀들을전부 선택한게 되므로 무승부처리를 한다.
            int num = 0;
            foreach (var cellState in cellStates)
            {
                if (cellState == -1)
                {
                    ++num;
                }
            }
            if (num == 0)
            {
                return Winner.Tie;
            }
        }

        //승패 무승부도 아니면 None
        return Winner.None;  
    }

    public void StartGame(PlayerType type)
    {
        playerType = type;

        if (type == PlayerType.PlayerOne)
        {
            gameState = GameState.PlayerTurn;
        }
        else
        {
            gameState = GameState.OpponentTurn;
        }
    }



    public void ShowGameover()
    {
        //게임오버 패널이 화면자리로 온다.
       // gameoverPanel.anchoredPosition = Vector2.zero;
        gameoverPanel.DOLocalMoveY(0, 1).SetEase(Ease.InOutBack);
        gameoverPanel.GetComponent<CanvasGroup>().DOFade(1, 3); //1초동안 투명도를 1로 만든다.
    }


}