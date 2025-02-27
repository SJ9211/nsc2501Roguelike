using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
// Player 세팅에서 other 세팅에서 Configuration => Both 로바꾸기
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;

        // 보드에서의 player위치 지정 => 화면에서 제대로된 위치에 표시
        MoveTo(cell);

    }

    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(cell);
        transform.position = m_Board.CellToWorld(m_CellPosition);
    }

    private void Update()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            // 셀이 움직일 수 있으면 움직여라
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            if (cellData != null && cellData.Passable)
            {
                GameManager.Instance.turnManager.Tick();
                MoveTo(newCellTarget);

                if ( cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEnterd();
                }
            }
        }
    }
}
