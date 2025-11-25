using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using JianghuGuidebook.Data;
using JianghuGuidebook.Core;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 분파 선택 화면 UI
    /// </summary>
    public class FactionSelectionUI : MonoBehaviour
    {
        [Header("분파 선택 패널")]
        [SerializeField] private GameObject factionSelectionPanel;

        [Header("분파 카드 프리팹")]
        [SerializeField] private GameObject factionCardPrefab;
        [SerializeField] private Transform factionCardContainer;

        [Header("선택된 분파 정보")]
        [SerializeField] private TextMeshProUGUI selectedFactionNameText;
        [SerializeField] private TextMeshProUGUI selectedFactionDescriptionText;
        [SerializeField] private TextMeshProUGUI selectedFactionSpecialtyText;
        [SerializeField] private TextMeshProUGUI selectedFactionStatsText;

        [Header("버튼")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button backButton;

        private FactionData selectedFaction;
        private int selectedSlot = 0;
        private List<GameObject> factionCards = new List<GameObject>();

        // 이벤트
        public System.Action<FactionData, int> OnFactionConfirmed;

        private void Start()
        {
            // 버튼 리스너
            if (confirmButton != null)
                confirmButton.onClick.AddListener(OnConfirmClicked);

            if (backButton != null)
                backButton.onClick.AddListener(OnBackClicked);

            // 초기 상태
            if (factionSelectionPanel != null)
                factionSelectionPanel.SetActive(false);

            if (confirmButton != null)
                confirmButton.interactable = false;
        }

        /// <summary>
        /// 분파 선택 화면을 표시합니다
        /// </summary>
        public void Show(int slotIndex)
        {
            selectedSlot = slotIndex;

            if (factionSelectionPanel != null)
            {
                factionSelectionPanel.SetActive(true);
            }

            // 분파 목록 로드
            LoadFactions();
        }

        /// <summary>
        /// 분파 선택 화면을 닫습니다
        /// </summary>
        public void Hide()
        {
            if (factionSelectionPanel != null)
            {
                factionSelectionPanel.SetActive(false);
            }

            ClearFactionCards();
            selectedFaction = null;
        }

        /// <summary>
        /// 분파 목록을 로드하여 표시합니다
        /// </summary>
        private void LoadFactions()
        {
            if (DataManager.Instance == null)
            {
                Debug.LogError("DataManager가 없습니다");
                return;
            }

            // 기존 카드 제거
            ClearFactionCards();

            // 모든 분파 데이터 가져오기
            FactionData[] factions = DataManager.Instance.GetAllFactions();

            if (factions == null || factions.Length == 0)
            {
                Debug.LogError("분파 데이터가 없습니다");
                return;
            }

            // 분파 카드 생성
            foreach (var faction in factions)
            {
                CreateFactionCard(faction);
            }

            Debug.Log($"[FactionSelectionUI] {factions.Length}개 분파 로드 완료");
        }

        /// <summary>
        /// 분파 카드를 생성합니다
        /// </summary>
        private void CreateFactionCard(FactionData faction)
        {
            if (factionCardPrefab == null || factionCardContainer == null)
            {
                Debug.LogWarning("분파 카드 프리팹 또는 컨테이너가 설정되지 않았습니다");
                return;
            }

            GameObject cardObj = Instantiate(factionCardPrefab, factionCardContainer);

            // 카드 정보 설정
            TextMeshProUGUI nameText = cardObj.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = faction.name;
            }

            TextMeshProUGUI specialtyText = cardObj.transform.Find("SpecialtyText")?.GetComponent<TextMeshProUGUI>();
            if (specialtyText != null)
            {
                specialtyText.text = $"특화: {faction.specialty}";
            }

            // 버튼 설정
            Button cardButton = cardObj.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.AddListener(() => OnFactionCardClicked(faction));
            }

            factionCards.Add(cardObj);
        }

        /// <summary>
        /// 분파 카드를 모두 제거합니다
        /// </summary>
        private void ClearFactionCards()
        {
            foreach (var card in factionCards)
            {
                if (card != null)
                {
                    Destroy(card);
                }
            }

            factionCards.Clear();
        }

        /// <summary>
        /// 분파 카드 클릭
        /// </summary>
        private void OnFactionCardClicked(FactionData faction)
        {
            Debug.Log($"[FactionSelectionUI] 분파 선택: {faction.name}");

            selectedFaction = faction;
            UpdateSelectedFactionInfo();

            // 확인 버튼 활성화
            if (confirmButton != null)
            {
                confirmButton.interactable = true;
            }
        }

        /// <summary>
        /// 선택된 분파 정보를 업데이트합니다
        /// </summary>
        private void UpdateSelectedFactionInfo()
        {
            if (selectedFaction == null)
                return;

            if (selectedFactionNameText != null)
            {
                selectedFactionNameText.text = selectedFaction.name;
            }

            if (selectedFactionDescriptionText != null)
            {
                selectedFactionDescriptionText.text = selectedFaction.description;
            }

            if (selectedFactionSpecialtyText != null)
            {
                selectedFactionSpecialtyText.text = $"<b>특화:</b> {selectedFaction.specialty}";
            }

            if (selectedFactionStatsText != null)
            {
                selectedFactionStatsText.text =
                    $"<b>시작 스탯</b>\n" +
                    $"체력: {selectedFaction.startingHealth}\n" +
                    $"골드: {selectedFaction.startingGold}\n" +
                    $"최대 내공: {selectedFaction.startingMaxEnergy}\n" +
                    $"시작 덱: {selectedFaction.startingDeck.Count}장";
            }
        }

        /// <summary>
        /// 확인 버튼 클릭
        /// </summary>
        private void OnConfirmClicked()
        {
            if (selectedFaction == null)
            {
                Debug.LogWarning("선택된 분파가 없습니다");
                return;
            }

            Debug.Log($"[FactionSelectionUI] 분파 확정: {selectedFaction.name}");

            // 이벤트 발생
            OnFactionConfirmed?.Invoke(selectedFaction, selectedSlot);

            // 화면 닫기
            Hide();
        }

        /// <summary>
        /// 뒤로 버튼 클릭
        /// </summary>
        private void OnBackClicked()
        {
            Debug.Log("[FactionSelectionUI] 분파 선택 취소");
            Hide();
        }
    }
}
