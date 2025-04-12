using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgePanelCloseButton : MonoBehaviour
{
    public GameObject badgePanel;

    public void HideBadgePanel() {

        badgePanel.SetActive(false);

    }
}
