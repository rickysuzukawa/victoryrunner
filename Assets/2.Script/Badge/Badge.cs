using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Badge {
    public string name;         // バッジの名前
    public int price;           // バッジの価格
    public bool isUnlocked;     // 購入済みかどうか
    public Sprite badgeSprite;   // バッジの画像
}
