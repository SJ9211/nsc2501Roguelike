using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
// Player 세팅에서 other 세팅에서 Configuration => Both 로바꾸기
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;

    // Property 방법
    public Vector2Int Cell
    {
        get
        {
            return m_CellPosition;
        }

        // 아무것도 안써도 {} 는 넣어줘야 Error가 안난다
        private set { } // 코드에 문제없어서 막아두기위해 private 쓴다
    }

    #region private
    private readonly int hashMoving = Animator.StringToHash("Moving");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private BoardManager m_Board;
    private Vector2Int m_CellPosition; // property 로 빼주면 TurnHappened 사용가능
    private bool m_IsGameOver;
    private bool m_IsMoving;
    private Vector3 m_MoveTarget;
    private Animator m_Animator;
    private Vector2Int newCellTarget;
    private bool hasMoved = false;
    #endregion

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;

        // 보드에서의 player위치 지정 => 화면에서 제대로된 위치에 표시
        MoveTo(cell, true);
    }

    public void Init()
    {
        m_IsMoving = false;
        m_IsGameOver = false;
    }


    // references 함수가 쓰인곳 편하게 찾는 기능
    public void MoveTo(Vector2Int cell, bool immediate = false)
    {
        m_CellPosition = cell;

        if (immediate)
        {
            m_IsMoving = false;
            transform.position = m_Board.CellToWorld(m_CellPosition);
        }
        else
        {
            m_IsMoving = true;
            //transform.position = m_Board.CellToWorld(cell);
            m_MoveTarget = m_Board.CellToWorld(m_CellPosition);
        }
        // todo : StringToHash 변환
        m_Animator.SetBool(hashMoving, m_IsMoving);
    }

    public void GameOver()
    {
        m_IsGameOver = true;
    }

    private void Update()
    {
        if (m_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }
        if (m_IsMoving)
        {
            transform.position = Vector3.MoveTowards
                       (transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);
            if (transform.position == m_MoveTarget)
            {
                m_IsMoving = false;
                m_Animator.SetBool(hashMoving, false);
                var cellData = m_Board.GetCellData(m_CellPosition);
                if (cellData.ContainedObject != null)
                    cellData.ContainedObject.PlayerEnterd();
            }
            return;
        }

        newCellTarget = m_CellPosition;
        hasMoved = false;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            MoveSkip();
        }
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            MoveUp();
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            MoveDown();
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            MoveRight();
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            MoveLeft();
        }

        if (hasMoved)
        {
            // 셀이 움직일 수 있으면 움직여라
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            if (cellData != null && cellData.Passable)
            {
                GameManager.Instance.turnManager.Tick();

                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);
                }
                else
                {
                    if (cellData.ContainedObject.PlayerWantsToEnter())
                    {
                        MoveTo(newCellTarget);
                        // player 먼저 셀로 이동 후 호출 (과거형)
                        //cellData.ContainedObject.PlayerEnterd();
                    }
                    else
                    {
                        m_Animator.SetTrigger(hashAttack);
                    }
                }
            }
        }
    }

    public void MoveSkip()
    {
        if (m_IsGameOver)
        {
            GameManager.Instance.StartNewGame();
            return;
        }

        if (m_IsMoving) return;
        hasMoved = true;
    }

    public void MoveUp()
    {
        if (m_IsMoving) return;
        newCellTarget.y++;
        hasMoved = true;
    }

    public void MoveDown()
    {
        if (m_IsMoving) return;
        newCellTarget.y--;
        hasMoved = true;
    }

    public void MoveLeft()
    {
        if (m_IsMoving) return;
        newCellTarget.x--;
        hasMoved = true;
    }

    public void MoveRight()
    {
        if (m_IsMoving) return;
        newCellTarget.x++;
        hasMoved = true;
    }
}
