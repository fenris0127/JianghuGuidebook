using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using JianghuGuidebook.Cards;

namespace JianghuGuidebook.Shop
{
    /// <summary>
    /// 카드 제거 서비스 UI를 관리합니다.
    /// 덱의 카드를 보여주고 선택하여 제거할 수 있게 합니다.
    /// </summary>
    public class CardRemovalUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private GameObject cardPrefab; // 카드 UI 프리팹 (CardUI 사용)
        [SerializeField] private Button closeButton;

        private ShopUI shopUI;
        private List<Card> currentDeck;

        public void Initialize(ShopUI ui)
        {
            this.shopUI = ui;
            panel.SetActive(false);
            
            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
        }

        /// <summary>
        /// 카드 제거 UI를 엽니다.
        /// </summary>
        public void Open()
        {
            panel.SetActive(true);
            PopulateCards();
        }

        /// <summary>
        /// 덱의 카드를 UI에 표시합니다.
        /// </summary>
        private void PopulateCards()
        {
            // 기존 카드 제거
            foreach (Transform child in cardContainer)
            {
                Destroy(child.gameObject);
            }

            // 덱 가져오기 (DeckManager가 싱글톤이라고 가정)
            // 실제로는 DeckManager.Instance.Deck 또는 유사한 방식으로 접근해야 함
            // 여기서는 DeckManager가 있다고 가정하고 구현
            if (DeckManager.Instance != null)
            {
                // DeckManager의 전체 덱 리스트를 가져와야 함 (현재 DeckManager 구조 확인 필요)
                // 임시로 빈 리스트 사용
                currentDeck = new List<Card>(); 
                // TODO: DeckManager에서 전체 덱 가져오기 구현 필요
                // currentDeck = DeckManager.Instance.GetAllCards();
            }

            if (currentDeck == null) return;

            foreach (Card card in currentDeck)
            {
                GameObject cardObj = Instantiate(cardPrefab, cardContainer);
                
                // CardUI 컴포넌트 설정
                // CardUI cardUI = cardObj.GetComponent<CardUI>();
                // cardUI.Initialize(card);
                
                // 클릭 이벤트 추가
                Button btn = cardObj.GetComponent<Button>();
                if (btn == null) btn = cardObj.AddComponent<Button>();
                
                btn.onClick.AddListener(() => OnCardSelected(card));
            }
        }

        /// <summary>
        /// 카드가 선택되었을 때 호출됩니다.
        /// </summary>
        private void OnCardSelected(Card card)
        {
            // 카드 제거 로직
            Debug.Log($"카드 제거 선택됨: {card.Name}");
            
            // DeckManager에서 카드 제거
            // DeckManager.Instance.RemoveCard(card);
            
            // UI 닫기
            Close();
            
            // 상점 UI 갱신 (필요하다면)
            shopUI.OnCardRemoved();
        }

        public void Close()
        {
            panel.SetActive(false);
        }
    }
}
