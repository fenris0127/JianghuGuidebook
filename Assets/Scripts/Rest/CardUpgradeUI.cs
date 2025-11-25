using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Cards;

namespace JianghuGuidebook.Rest
{
    /// <summary>
    /// 카드 업그레이드 선택 UI를 관리합니다.
    /// </summary>
    public class CardUpgradeUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private GameObject cardPrefab; // CardUI 사용
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private RestUI restUI;
        private List<Card> selectedCards = new List<Card>();
        private int requiredSelectionCount = 1;

        public void Initialize(RestUI ui)
        {
            this.restUI = ui;
            panel.SetActive(false);
            
            if (confirmButton != null)
                confirmButton.onClick.AddListener(OnConfirm);
            
            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancel);
        }

        /// <summary>
        /// 업그레이드 UI를 엽니다.
        /// </summary>
        /// <param name="count">선택해야 하는 카드 수</param>
        public void Open(int count)
        {
            requiredSelectionCount = count;
            selectedCards.Clear();
            panel.SetActive(true);
            
            titleText.text = $"강화할 카드를 선택하세요 ({selectedCards.Count}/{requiredSelectionCount})";
            confirmButton.interactable = false;

            PopulateCards();
        }

        private void PopulateCards()
        {
            foreach (Transform child in cardContainer)
            {
                    // CardUI 초기화
                    // CardUI cardUI = cardObj.GetComponent<CardUI>();
                    // cardUI.Initialize(card);

                    // 클릭 이벤트
                    Button btn = cardObj.GetComponent<Button>();
                    if (btn == null) btn = cardObj.AddComponent<Button>();
                    
                    btn.onClick.AddListener(() => ToggleCardSelection(card, cardObj));
            }

            UpdateUIState();
        }

        private void UpdateUIState()
        {
            titleText.text = $"강화할 카드를 선택하세요 ({selectedCards.Count}/{requiredSelectionCount})";
            confirmButton.interactable = selectedCards.Count == requiredSelectionCount;
        }

        private void OnConfirm()
        {
            foreach (var card in selectedCards)
            {
                RestManager.Instance.UpgradeCard(card);
            }
            
            Close();
            restUI.OnUpgradeComplete();
        }

        private void OnCancel()
        {
            Close();
        }

        public void Close()
        {
            panel.SetActive(false);
        }
    }
}
