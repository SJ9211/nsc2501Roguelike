using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : CellObject
{
    public override void PlayerEnterd()
    {
        // Food 없애기
        Destroy(gameObject);

        // 플레이어의 체력(Food) 증가
        Debug.Log("Food increased");
    }
}
