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
                Destroy(child.gameObject);
            }

            // 덱 가져오기
            if (DeckManager.Instance != null)
            {
                // TODO: DeckManager에서 전체 덱 가져오기
                // List<Card> deck = DeckManager.Instance.GetAllCards();
                // 임시 코드
                List<Card> deck = new List<Card>(); 

                foreach (Card card in deck)
                {
                    // 이미 업그레이드된 카드는 제외할 수도 있음 (기획에 따라 다름)
                    // 여기서는 모든 카드 표시

                    GameObject cardObj = Instantiate(cardPrefab, cardContainer);
                    
                    // CardUI 초기화
                    // CardUI cardUI = cardObj.GetComponent<CardUI>();
                    // cardUI.Initialize(card);

                    // 클릭 이벤트
                    Button btn = cardObj.GetComponent<Button>();
                    if (btn == null) btn = cardObj.AddComponent<Button>();
                    
                    btn.onClick.AddListener(() => ToggleCardSelection(card, cardObj));
                }
            }
        }

        private void ToggleCardSelection(Card card, GameObject cardObj)
        {
            if (selectedCards.Contains(card))
            {
                selectedCards.Remove(card);
                // 선택 해제 시각 효과 (예: 테두리 제거)
            }
            else
            {
                if (selectedCards.Count < requiredSelectionCount)
                {
                    selectedCards.Add(card);
                    // 선택 시각 효과 (예: 테두리 강조)
                }
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
