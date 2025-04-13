using UnityEngine;
using System.IO;
using System.Collections;

using UnityEngine.Networking;

public class XShareManager : MonoBehaviour {
    public string shareHashtags = "#VictoryRunner #バッジ取得";

    public void ShareScreenshotToX() {
        StartCoroutine(CaptureAndShare());
    }

    IEnumerator CaptureAndShare() {
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        string filename = "badge_screenshot.png";
        string filepath = Path.Combine(Application.temporaryCachePath, filename);
        File.WriteAllBytes(filepath, screenImage.EncodeToPNG());

        Destroy(screenImage);

        // X投稿用のURLを生成
        string tweetText = UnityWebRequest.EscapeURL("バッジをゲットしたよ！ " + shareHashtags);
        string tweetURL = "https://twitter.com/intent/tweet?text=" + tweetText;

#if UNITY_IOS
        // iOSはファイル添付ができないので画像なし投稿＋写真はユーザーに案内
        Application.OpenURL(tweetURL);
#elif UNITY_ANDROID
        // Androidは画像添付できるように intent を使って共有
        new NativeShare().AddFile(filepath)
            .SetText("バッジをゲットしたよ！ " + shareHashtags)
            .SetTitle("VictoryRunnerをプレイ中！")
            .SetSubject("VictoryRunner Badge")
            .SetCallback((result, shareTarget) => {
                Debug.Log("Share result: " + result + ", target: " + shareTarget);
            })
            .Share();
#else
        // PC等ならリンクだけ開く
        Application.OpenURL(tweetURL);
#endif
    }
}
