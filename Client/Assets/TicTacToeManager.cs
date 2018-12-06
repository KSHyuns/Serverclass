using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    enum Mark { Circle, Cross }
    PlayerType playerType;
    int[] cellStates = new int[9]; // o,x 값 정보
    TicTacToeNetwork networkManager;

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
        cells[cellIndex].GetComponent<SpriteRenderer>().sprite = markSprite;
        // 할당된 Cell을 터치가 불가능하게 비활성
        cells[cellIndex].GetComponent<BoxCollider2D>().enabled = false;


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

            // TODO: 게임의 결과 화면에 표시
        }

    }

    // 승패 확인
    Winner CheckWinner()
    {
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
}