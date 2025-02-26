using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class TurnManager
{
    public event System.Action OnTick;
    private int m_TurnCount;
    

    public TurnManager()
    {
        m_TurnCount = 1;
    }
    
    public void Tick()
    {
        m_TurnCount += 1;
        Debug.Log("Current turn count :" + m_TurnCount);
        OnTick?.Invoke();
        /* ^위에 있는걸 줄여서 쓴방법^
        if (OnTick != null)
        {
            OnTick.Invoke();
        }
        */
    }
}
