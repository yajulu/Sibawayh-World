using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using PROJECT.Scripts.Game.Controllers;
using Sirenix.OdinInspector;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.UI.Screens;

public class UIHeaderPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RTLTextMeshPro coinText;
    [SerializeField] private RTLTextMeshPro gemText;
    [SerializeField] private RectTransform profilePanel;
    [SerializeField] private RectTransform navigationButtons;

    public void Awake()
    {
        //Debug.Log("AWAKE");
        DataPersistenceManager.OnPlayerInventoryUpdated += DataPersistenceManager_OnPlayerInventoryUpdated;
        //DataPersistenceManager_OnPlayerInventoryUpdated();
        UIScreenManager.OnScreenOpenStarted += UIScreenManager_OnScreenOpenStarted; 
    }

    private void UIScreenManager_OnScreenOpenStarted(UIScreenBase obj)
    {
        Debug.Log(obj.GetType().Equals(typeof(Screen_GameMode)));
        if (obj.GetType().Equals(typeof(Screen_GameMode)))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            if (obj.GetType().Equals(typeof(Screen_HomeScreen)))
            {
                profilePanel.gameObject.SetActive(true);
                navigationButtons.gameObject.SetActive(false);
            }
            else
            {
                profilePanel.gameObject.SetActive(false);
                navigationButtons.gameObject.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        DataPersistenceManager.OnPlayerInventoryUpdated -= DataPersistenceManager_OnPlayerInventoryUpdated;
        UIScreenManager.OnScreenOpenStarted -= UIScreenManager_OnScreenOpenStarted;
    }

    private void DataPersistenceManager_OnPlayerInventoryUpdated()
    {
        coinText.text = DataPersistenceManager.Instance.VirtualCurrency["YC"].ToString();
        gemText.text = DataPersistenceManager.Instance.VirtualCurrency["YG"].ToString();
    }

    [Button]
    private void SetRefs()
    {
        coinText = transform.FindDeepChild<RTLTextMeshPro>("WalletCoin_Text");
        gemText = transform.FindDeepChild<RTLTextMeshPro>("WalletGem_Text");
        profilePanel = transform.FindDeepChild<RectTransform>("HeaderProfileButton_Panel");
        navigationButtons = transform.FindDeepChild<RectTransform>("HeaderNavigation_Panel");
    }
}
