using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [System.Serializable]
    public class StageBGM {
        public string stageName;         // ステージ名（例: "Stage1"）
        public AudioClip bgmClip;        // 再生したいBGM
    }

    public List<StageBGM> stageBGMList;     // ステージごとのBGMリスト
    private AudioSource bgmSource;          // BGM用AudioSource

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {

        PlayBGMForStage("Title");
    }

    // 指定ステージ名のBGMを再生する
    public void PlayBGMForStage(string stageName) {
        StageBGM bgm = stageBGMList.Find(s => s.stageName == stageName);
        if (bgm != null && bgm.bgmClip != null) {
            bgmSource.clip = bgm.bgmClip;
            bgmSource.volume = 1f;
            bgmSource.Play();
        } else {
            Debug.LogWarning($"BGMが見つかりません: {stageName}");
        }
    }

    public void MuteAll() {
        bgmSource.volume = 0f;
    }

    public void UnmuteAll() {
        bgmSource.volume = 1f;
    }

    public void StopBGM() {
        bgmSource.Stop();
    }
}
