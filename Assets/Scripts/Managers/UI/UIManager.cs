using UnityEngine;
using System;
using System.Collections.Generic;
using GangHoBiGeup.Core;
using GangHoBiGeup.Data;

namespace GangHoBiGeup.Managers
{
    /// <summary>
    /// 모든 UI 패널과 화면 전환을 관리하는 매니저
    /// GameManager에서 UI 로직을 분리했습니다.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject mapUI;
        [SerializeField] private GameObject battleUI;
        [SerializeField] private GameObject cardRewardUI;
        [SerializeField] private GameObject relicRewardUI;
        [SerializeField] private GameObject shopUI;
        [SerializeField] private GameObject eventUI;
        [SerializeField] private GameObject restSiteUI;
        [SerializeField] private GameObject victoryUI;
        [SerializeField] private GameObject gameOverUI;

        [Header("Reward UI Components")]
        [SerializeField] private GameObject cardRewardSlotPrefab;
        [SerializeField] private Transform cardRewardContainer;
        [SerializeField] private GameObject relicRewardSlotPrefab;
        [SerializeField] private Transform relicRewardContainer;

        // UI 상태 매핑
        private Dictionary<GameState, GameObject> uiPanels;

        protected override void OnAwake()
        {
            InitializeUIPanels();
        }

        private void InitializeUIPanels()
        {
            uiPanels = new Dictionary<GameState, GameObject>
            {
                { GameState.MainMenu, mainMenuUI },
                { GameState.MapView, mapUI },
                { GameState.Battle, battleUI },
                { GameState.Shop, shopUI },
                { GameState.Event, eventUI },
                { GameState.RestSite, restSiteUI },
                { GameState.Victory, victoryUI },
                { GameState.GameOver, gameOverUI }
            };
        }

        /// <summary>
        /// 게임 상태에 맞는 UI 패널을 표시합니다.
        /// </summary>
        public void ShowState(GameState state)
        {
            // 모든 UI 패널 비활성화
            foreach (var panel in uiPanels.Values)
            {
                if (panel != null)
                    panel.SetActive(false);
            }

            // 현재 상태에 맞는 UI 패널 활성화
            if (uiPanels.TryGetValue(state, out GameObject activePanel) && activePanel != null)
            {
                activePanel.SetActive(true);
            }

            // Reward 상태는 특별 처리 (cardRewardUI 또는 relicRewardUI)
            // ShowCardRewardScreen()이나 ShowRelicRewardScreen()에서 처리됨
        }

        /// <summary>
        /// 카드 보상 선택 화면을 표시합니다.
        /// </summary>
        /// <param name="rewards">보상 카드 목록</param>
        /// <param name="onCardSelected">카드 선택 시 콜백</param>
        public void ShowCardRewardScreen(List<CardData> rewards, Action<CardData> onCardSelected)
        {
            if (cardRewardUI == null || cardRewardContainer == null)
            {
                Debug.LogWarning("CardRewardUI 또는 Container가 설정되지 않았습니다.");
                return;
            }

            cardRewardUI.SetActive(true);

            // 기존 슬롯 제거
            foreach (Transform child in cardRewardContainer)
                Destroy(child.gameObject);

            // 새 카드 슬롯 생성
            foreach (var card in rewards)
            {
                GameObject slotObj = Instantiate(cardRewardSlotPrefab, cardRewardContainer);
                slotObj.GetComponent<CardUI>().Setup(card);

                // 로컬 변수로 캡처하여 클로저 문제 방지
                CardData selectedCard = card;
                slotObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    onCardSelected?.Invoke(selectedCard);
                    cardRewardUI.SetActive(false);
                });
            }
        }

        /// <summary>
        /// 유물 보상 선택 화면을 표시합니다.
        /// </summary>
        /// <param name="rewards">보상 유물 목록</param>
        /// <param name="onRelicSelected">유물 선택 시 콜백</param>
        public void ShowRelicRewardScreen(List<RelicData> rewards, Action<RelicData> onRelicSelected)
        {
            if (relicRewardUI == null || relicRewardContainer == null)
            {
                Debug.LogWarning("RelicRewardUI 또는 Container가 설정되지 않았습니다.");
                return;
            }

            relicRewardUI.SetActive(true);

            // 기존 슬롯 제거
            foreach (Transform child in relicRewardContainer)
                Destroy(child.gameObject);

            // 새 유물 슬롯 생성
            foreach (var relic in rewards)
            {
                GameObject slotObj = Instantiate(relicRewardSlotPrefab, relicRewardContainer);
                slotObj.GetComponent<UnityEngine.UI.Image>().sprite = relic.icon;

                // 로컬 변수로 캡처하여 클로저 문제 방지
                RelicData selectedRelic = relic;
                slotObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    onRelicSelected?.Invoke(selectedRelic);
                    relicRewardUI.SetActive(false);
                });
            }
        }

        /// <summary>
        /// 모든 UI 패널을 숨깁니다.
        /// </summary>
        public void HideAllPanels()
        {
            foreach (var panel in uiPanels.Values)
            {
                if (panel != null)
                    panel.SetActive(false);
            }

            if (cardRewardUI != null) cardRewardUI.SetActive(false);
            if (relicRewardUI != null) relicRewardUI.SetActive(false);
        }
    }
}
