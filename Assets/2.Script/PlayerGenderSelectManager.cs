using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//キャラクターの性別を指定するためのクラス
//開発初期は男の子か女の子を選択できるシーンがありましたが廃止を検討中(20241224)
//自動的に男の子を選択状態でステージシーンに遷移するようにしています
public class PlayerGenderSelectManager : MonoBehaviour
{
    private void Start() {

        //デフォルトは0(プレイヤーは男の子)
        //男の子キャラを保存
        PlayerPrefs.SetInt("PlayerSelectNumber", 0);

    }
}
