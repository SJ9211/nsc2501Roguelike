using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : CellObject
{
    public int AmountGranted = 10;
    public AudioClip clip;

    public override void PlayerEnterd()
    {
        // Food 없애기
        Destroy(gameObject);

        // 플레이어의 체력(Food) 증가
        GameManager.Instance.ChangeFood(AmountGranted);

        // 음향효과 발생
        GameManager.Instance.PlaySound(clip);
    }

}
