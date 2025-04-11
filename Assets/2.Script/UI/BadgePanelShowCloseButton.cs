using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgePanelShowCloseButton : MonoBehaviour
{
    public GameObject badgePanel;
    public BadgeManager badgeManager;

    private void Start() {

        badgePanel.SetActive(false);

    }

    public void ShowBadgePanel() {

        badgePanel.SetActive(!badgePanel.activeSelf);
        badgeManager.UpdateBadgeUI();

    }

    public void HideBadgePanel() {

        badgePanel.SetActive(false);

    }
}
