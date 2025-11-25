using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using JianghuGuidebook.Relics;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 보유 유물 목록을 표시하는 UI 컴포넌트
    /// </summary>
    public class RelicDisplayUI : MonoBehaviour
    {
        [Header("UI 참조")]
        [SerializeField] private GameObject relicItemPrefab;
        [SerializeField] private Transform relicContainer;
        [SerializeField] private TextMeshProUGUI relicCountText;

        [Header("시너지 UI")]
        [SerializeField] private GameObject synergyPanel;
        [SerializeField] private Transform synergyContainer;
        [SerializeField] private GameObject synergyItemPrefab;

        [Header("툴팁")]
        [SerializeField] private GameObject tooltipPanel;
        [SerializeField] private TextMeshProUGUI tooltipNameText;
        [SerializeField] private TextMeshProUGUI tooltipDescriptionText;
        [SerializeField] private TextMeshProUGUI tooltipRarityText;
        [SerializeField] private Image tooltipBackground;

        [Header("색상 설정")]
        [SerializeField] private Color commonColor = Color.white;
        [SerializeField] private Color uncommonColor = Color.green;
        [SerializeField] private Color rareColor = Color.blue;
        [SerializeField] private Color legendaryColor = new Color(1f, 0.5f, 0f); // 주황색

        private List<RelicItemUI> relicItems = new List<RelicItemUI>();

        private void Start()
        {
            // 툴팁 비활성화
            if (tooltipPanel != null)
                tooltipPanel.SetActive(false);

            // RelicManager 이벤트 구독
            if (RelicManager.Instance != null)
            {
                RelicManager.Instance.OnRelicAdded += OnRelicAdded;
                RelicManager.Instance.OnRelicRemoved += OnRelicRemoved;
                RelicManager.Instance.OnSynergyActivated += OnSynergyActivated;
                RelicManager.Instance.OnSynergyDeactivated += OnSynergyDeactivated;

                // 초기 유물 표시
                RefreshRelicDisplay();
                RefreshSynergyDisplay();
            }
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (RelicManager.Instance != null)
            {
                RelicManager.Instance.OnRelicAdded -= OnRelicAdded;
                RelicManager.Instance.OnRelicRemoved -= OnRelicRemoved;
                RelicManager.Instance.OnSynergyActivated -= OnSynergyActivated;
                RelicManager.Instance.OnSynergyDeactivated -= OnSynergyDeactivated;
            }
        }

        /// <summary>
        /// 유물 추가 시 호출
        /// </summary>
        private void OnRelicAdded(Relic relic)
        {
            RefreshRelicDisplay();
            RefreshSynergyDisplay();
        }

        /// <summary>
        /// 유물 제거 시 호출
        /// </summary>
        private void OnRelicRemoved(Relic relic)
        {
            RefreshRelicDisplay();
            RefreshSynergyDisplay();
        }

        /// <summary>
        /// 시너지 활성화 시 호출
        /// </summary>
        private void OnSynergyActivated(RelicSynergy synergy)
        {
            RefreshSynergyDisplay();
            Debug.Log($"[UI] 시너지 활성화: {synergy.synergyName}");
        }

        /// <summary>
        /// 시너지 비활성화 시 호출
        /// </summary>
        private void OnSynergyDeactivated(RelicSynergy synergy)
        {
            RefreshSynergyDisplay();
            Debug.Log($"[UI] 시너지 비활성화: {synergy.synergyName}");
        }

        /// <summary>
        /// 유물 목록을 새로고침합니다
        /// </summary>
        public void RefreshRelicDisplay()
        {
            if (RelicManager.Instance == null || relicContainer == null)
                return;

            // 기존 아이템 제거
            foreach (var item in relicItems)
            {
                if (item != null && item.gameObject != null)
                    Destroy(item.gameObject);
            }
            relicItems.Clear();

            // 유물 개수 표시
            List<Relic> relics = RelicManager.Instance.OwnedRelics;
            if (relicCountText != null)
            {
                relicCountText.text = $"보유 유물: {relics.Count}개";
            }

            // 유물 아이템 생성
            if (relicItemPrefab != null)
            {
                foreach (var relic in relics)
                {
                    GameObject itemObj = Instantiate(relicItemPrefab, relicContainer);
                    RelicItemUI itemUI = itemObj.GetComponent<RelicItemUI>();

                    if (itemUI != null)
                    {
                        itemUI.Setup(relic, this);
                        relicItems.Add(itemUI);
                    }
                }
            }

            Debug.Log($"유물 UI 갱신: {relics.Count}개");
        }

        /// <summary>
        /// 시너지 표시를 새로고침합니다
        /// </summary>
        public void RefreshSynergyDisplay()
        {
            if (RelicManager.Instance == null || synergyContainer == null)
                return;

            // 기존 시너지 아이템 제거
            foreach (Transform child in synergyContainer)
            {
                Destroy(child.gameObject);
            }

            // 활성 시너지 가져오기
            List<RelicSynergy> activeSynergies = RelicManager.Instance.GetActiveSynergies();

            // 시너지가 없으면 패널 숨김
            if (synergyPanel != null)
            {
                synergyPanel.SetActive(activeSynergies.Count > 0);
            }

            // 시너지 아이템 생성
            if (synergyItemPrefab != null)
            {
                foreach (var synergy in activeSynergies)
                {
                    GameObject itemObj = Instantiate(synergyItemPrefab, synergyContainer);
                    TextMeshProUGUI text = itemObj.GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.text = $"⚡ {synergy.synergyName}: {synergy.description}";
                    }
                }
            }

            Debug.Log($"시너지 UI 갱신: {activeSynergies.Count}개 활성화");
        }

        /// <summary>
        /// 유물 툴팁을 표시합니다
        /// </summary>
        public void ShowTooltip(Relic relic, Vector3 position)
        {
            if (tooltipPanel == null || relic == null)
                return;

            tooltipPanel.SetActive(true);

            // 툴팁 위치 설정
            tooltipPanel.transform.position = position;

            // 유물 정보 표시
            if (tooltipNameText != null)
            {
                tooltipNameText.text = relic.name;
            }

            if (tooltipDescriptionText != null)
            {
                tooltipDescriptionText.text = relic.description;
            }

            if (tooltipRarityText != null)
            {
                string rarityText = GetRarityText(relic.rarity);
                tooltipRarityText.text = rarityText;
                tooltipRarityText.color = GetRarityColor(relic.rarity);
            }

            // 배경색 설정
            if (tooltipBackground != null)
            {
                Color bgColor = GetRarityColor(relic.rarity);
                bgColor.a = 0.2f; // 반투명
                tooltipBackground.color = bgColor;
            }
        }

        /// <summary>
        /// 툴팁을 숨깁니다
        /// </summary>
        public void HideTooltip()
        {
            if (tooltipPanel != null)
            {
                tooltipPanel.SetActive(false);
            }
        }

        /// <summary>
        /// 희귀도에 따른 색상을 반환합니다
        /// </summary>
        private Color GetRarityColor(RelicRarity rarity)
        {
            switch (rarity)
            {
                case RelicRarity.Common:
                    return commonColor;
                case RelicRarity.Uncommon:
                    return uncommonColor;
                case RelicRarity.Rare:
                    return rareColor;
                case RelicRarity.Legendary:
                    return legendaryColor;
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// 희귀도 텍스트를 반환합니다
        /// </summary>
        private string GetRarityText(RelicRarity rarity)
        {
            switch (rarity)
            {
                case RelicRarity.Common:
                    return "[일반]";
                case RelicRarity.Uncommon:
                    return "[고급]";
                case RelicRarity.Rare:
                    return "[진귀]";
                case RelicRarity.Legendary:
                    return "[전설]";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 유물 목록을 열거나 닫습니다
        /// </summary>
        public void ToggleRelicDisplay()
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                RefreshRelicDisplay();
                RefreshSynergyDisplay();
            }
        }
    }

    /// <summary>
    /// 개별 유물 아이템 UI
    /// </summary>
    public class RelicItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI 참조")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image borderImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject glowEffect;

        [Header("애니메이션")]
        [SerializeField] private float hoverScale = 1.1f;
        [SerializeField] private float animationSpeed = 5f;

        private Relic relic;
        private RelicDisplayUI displayUI;
        private Vector3 originalScale;
        private bool isHovering = false;

        private void Awake()
        {
            originalScale = transform.localScale;

            if (glowEffect != null)
                glowEffect.SetActive(false);
        }

        private void Update()
        {
            // 호버 애니메이션
            float targetScale = isHovering ? hoverScale : 1f;
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale * targetScale,
                Time.deltaTime * animationSpeed
            );
        }

        public void Setup(Relic relic, RelicDisplayUI displayUI)
        {
            this.relic = relic;
            this.displayUI = displayUI;

            // 유물 이름 표시
            if (nameText != null)
            {
                nameText.text = relic.name;
            }

            // 희귀도에 따른 테두리 색상
            if (borderImage != null)
            {
                borderImage.color = GetRarityColor(relic.rarity);
            }

            // 배경 색상 (반투명)
            if (backgroundImage != null)
            {
                Color bgColor = GetRarityColor(relic.rarity);
                bgColor.a = 0.1f;
                backgroundImage.color = bgColor;
            }

            // 아이콘 설정
            if (iconImage != null && RelicIconManager.Instance != null)
            {
                iconImage.sprite = RelicIconManager.Instance.GetRelicIcon(relic);
            }
        }

        /// <summary>
        /// 마우스 호버 시 (IPointerEnterHandler)
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;

            // 글로우 효과 활성화
            if (glowEffect != null)
                glowEffect.SetActive(true);

            // 툴팁 표시 (새로운 RelicTooltip 사용)
            if (RelicTooltip.Instance != null && relic != null)
            {
                RelicTooltip.Instance.Show(relic, eventData.position);
            }
            // 폴백: 기존 툴팁 사용
            else if (displayUI != null && relic != null)
            {
                displayUI.ShowTooltip(relic, transform.position);
            }
        }

        /// <summary>
        /// 마우스 나갈 때 (IPointerExitHandler)
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;

            // 글로우 효과 비활성화
            if (glowEffect != null)
                glowEffect.SetActive(false);

            // 툴팁 숨김
            if (RelicTooltip.Instance != null)
            {
                RelicTooltip.Instance.Hide();
            }
            else if (displayUI != null)
            {
                displayUI.HideTooltip();
            }
        }

        private Color GetRarityColor(RelicRarity rarity)
        {
            switch (rarity)
            {
                case RelicRarity.Common:
                    return Color.white;
                case RelicRarity.Uncommon:
                    return Color.green;
                case RelicRarity.Rare:
                    return Color.blue;
                case RelicRarity.Legendary:
                    return new Color(1f, 0.5f, 0f);
                default:
                    return Color.white;
            }
        }
    }
}
