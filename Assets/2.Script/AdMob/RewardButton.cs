using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardButton : MonoBehaviour
{
    public void ShowReward() {

        RewardAdManager.Instance.ShowRewardAd();

    }

    public void Vibration() {

        //バイブ振動
        HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Light);

    }
}
