using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : CellObject
{
    public int AmountGranted = 10;
    public override void PlayerEnterd()
    {
        // Food 없애기
        Destroy(gameObject);

        // 플레이어의 체력(Food) 증가
        GameManager.Instance.ChangeFood(AmountGranted);
    }
}
