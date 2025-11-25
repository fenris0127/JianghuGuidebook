using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Data;
using JianghuGuidebook.Rewards;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 보상 화면 UI를 관리합니다.
    /// </summary>
    public class RewardUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform rewardsContainer;
        [SerializeField] private GameObject rewardItemPrefab; // 버튼 형태의 프리팹
        [SerializeField] private Button proceedButton;

        [Header("Card Selection")]
        [SerializeField] private GameObject cardSelectionPanel;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private GameObject cardPrefab; // CardUI 프리팹
        [SerializeField] private TextMeshProUGUI cardSelectionTitle;
        [SerializeField] private Button skipCardButton;

        private List<RewardItem> currentRewards;
        private RewardItem currentCardRewardItem; // 현재 선택 중인 카드 보상 항목

        private void Start()
        {
            if (proceedButton != null)
                proceedButton.onClick.AddListener(OnProceedClicked);

            if (skipCardButton != null)
                skipCardButton.onClick.AddListener(OnSkipCardClicked);

            // 초기에는 숨김
            if (panel != null) panel.SetActive(false);
            if (cardSelectionPanel != null) cardSelectionPanel.SetActive(false);
        }

        /// <summary>
        /// 보상 목록을 표시합니다.
        /// </summary>
        public void ShowRewards(List<RewardItem> rewards)
        {
            currentRewards = rewards;
            panel.SetActive(true);
            cardSelectionPanel.SetActive(false);

            // 기존 항목 제거
            foreach (Transform child in rewardsContainer)
            {
                Destroy(child.gameObject);
            }

            // 보상 항목 생성
            foreach (var reward in rewards)
            {
                GameObject obj = Instantiate(rewardItemPrefab, rewardsContainer);
                Button btn = obj.GetComponent<Button>();
                TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();

                if (text != null)
                {
                    text.text = reward.ToString();
                }

                if (btn != null)
                {
                    btn.onClick.AddListener(() => OnRewardClicked(reward, obj));
                }
            }
        }

        private void OnRewardClicked(RewardItem reward, GameObject buttonObj)
        {
            if (reward.type == RewardType.Card)
            {
                // 카드 선택 화면 표시
                currentCardRewardItem = reward;
                ShowCardSelection(RewardManager.Instance.CurrentCardChoices);
                
                // 카드 보상은 선택 후 버튼 제거 (여기서는 일단 유지하고 선택 완료 시 제거)
            }
            else
            {
                // 즉시 획득 (골드, 유물)
                RewardManager.Instance.ClaimReward(reward);
                
                // 버튼 제거 또는 비활성화
                Destroy(buttonObj);
                
                // 리스트에서 제거 (옵션)
                currentRewards.Remove(reward);
            }
        }

        private void ShowCardSelection(List<CardData> cards)
        {
            cardSelectionPanel.SetActive(true);
            panel.SetActive(false); // 보상 목록 잠시 숨김

            foreach (Transform child in cardContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var cardData in cards)
            {
                GameObject obj = Instantiate(cardPrefab, cardContainer);
                // CardUI 초기화 (CardUI 스크립트가 있다고 가정)
                // var cardUI = obj.GetComponent<CardUI>();
                // cardUI.Initialize(cardData);

                Button btn = obj.GetComponent<Button>();
                if (btn == null) btn = obj.AddComponent<Button>();
                
                btn.onClick.AddListener(() => OnCardSelected(cardData));
            }
        }

        private void OnCardSelected(CardData cardData)
        {
            RewardManager.Instance.SelectCard(cardData);
            
            // 카드 보상 획득 처리
            if (currentCardRewardItem != null)
            {
                RewardManager.Instance.ClaimReward(currentCardRewardItem);
                currentRewards.Remove(currentCardRewardItem);
                currentCardRewardItem = null;
            }

            CloseCardSelection();
        }

        private void OnSkipCardClicked()
        {
            // 카드 보상 포기
            if (currentCardRewardItem != null)
            {
                currentRewards.Remove(currentCardRewardItem);
                currentCardRewardItem = null;
            }
            
            CloseCardSelection();
        }

        private void CloseCardSelection()
        {
            cardSelectionPanel.SetActive(false);
            
            // 보상 목록 다시 표시 (남은 보상이 있다면)
            if (currentRewards.Count > 0)
            {
                ShowRewards(currentRewards); // 다시 그리기 (제거된 항목 반영)
            }
            else
            {
                // 모든 보상 획득함
                OnProceedClicked();
            }
        }

        private void OnProceedClicked()
        {
            RewardManager.Instance.CompleteRewards();
            
            // 맵으로 복귀
            JianghuGuidebook.Map.MapManager.Instance.ReturnToMap();
        }
    }
}
