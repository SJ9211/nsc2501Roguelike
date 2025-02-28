using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObject : MonoBehaviour
{
   protected Vector2Int m_Cell;

   public virtual void Init(Vector2Int cell)
   {
      m_Cell = cell;  // 내용이있으면 실행시켜줄 필요가 있음
   }
   public virtual void PlayerEnterd()     // 가상 메서드 = virtual 상속받으면 오버라이드 써야함
   {

   }


   public virtual bool PlayerWantsToEnter()  // 
   {
      return true;
   }
}
