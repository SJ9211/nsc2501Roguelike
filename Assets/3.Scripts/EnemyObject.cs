using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyObject : CellObject
{
    public int Health = 2;
    public AudioClip Clip;
    private int m_CurrentHealth;
    private void Awake()
    {
        GameManager.Instance.turnManager.OnTick += TurnHappened;
    }
    private void OnDestroy()
    {
        GameManager.Instance.turnManager.OnTick -= TurnHappened;
    }
    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        m_CurrentHealth = Health;
    }

    public override bool PlayerWantsToEnter()
    {
        m_CurrentHealth -= 1;
        if (m_CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
        return false;
    }

    private bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if ( targetCell == null
              || !targetCell.Passable
              || targetCell.ContainedObject != null)
        {
            return false;
        }

        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }
    private void TurnHappened()
    {
        // property (밖으로 뺀다)
        var Playercell = GameManager.Instance.PlayerController.Cell;

        int xDist = Playercell.x - m_Cell.x;
        int yDist = Playercell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1)
            || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.ChangeFood(-3);
            GameManager.Instance.PlaySound(Clip);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
        }
    }

    private bool TryMoveInX(int xDist)
    {
        if ( xDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.right);
        }

        return MoveTo(m_Cell + Vector2Int.left);
    }

    private bool TryMoveInY(int yDist)
    {
        if( yDist > 0)
        {
           return MoveTo(m_Cell + Vector2Int.up);
        }
        return MoveTo(m_Cell + Vector2Int.down);
    }
}
