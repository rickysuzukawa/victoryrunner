using System.Collections;
using UnityEngine;
using UnityEngine.iOS;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class ATTManager : MonoBehaviour {
    void Start() {
        #if UNITY_IOS
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED) {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
        #endif

        // StartCoroutine(RequestATT());
    }

    // IEnumerator RequestATT() {
        // iOS 14 以降のみ実行

        // トラッキング許可ダイアログを表示
        // yield return new WaitForSeconds(1f); // 1秒待ってから表示（ゲーム開始直後を避ける）
        // ATTrackingStatusBinding.RequestAuthorizationTracking();

    // }
}
