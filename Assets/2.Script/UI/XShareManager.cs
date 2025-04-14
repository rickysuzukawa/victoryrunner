using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

public class XShareManager : MonoBehaviour {

    // 投稿文（URLエンコードされます）
    public string tweetMessage = "バッジをゲットしたよ！ #VictoryRunner #バッジ取得";

    public void ShareToX() {
        string encodedText = UnityWebRequest.EscapeURL(tweetMessage);
        string tweetURL = "https://twitter.com/intent/tweet?text=" + encodedText;

        Application.OpenURL(tweetURL);

    }

}
