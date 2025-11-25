using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using JianghuGuidebook.Relics;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 유물 툴팁을 표시하는 고급 컴포넌트
    /// </summary>
    public class RelicTooltip : MonoBehaviour
    {
        private static RelicTooltip _instance;

        public static RelicTooltip Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RelicTooltip>();
                }
                return _instance;
            }
        }

        [Header("UI 참조")]
        [SerializeField] private GameObject tooltipPanel;
        [SerializeField] private RectTransform tooltipRect;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("텍스트")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI effectTypeText;
        [SerializeField] private TextMeshProUGUI triggerCountText;

        [Header("이미지")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image borderImage;

        [Header("설정")]
        [SerializeField] private float fadeSpeed = 5f;
        [SerializeField] private Vector2 offset = new Vector2(10f, 10f);
        [SerializeField] private float edgePadding = 20f;

        [Header("색상")]
        [SerializeField] private Color commonColor = Color.white;
        [SerializeField] private Color uncommonColor = Color.green;
        [SerializeField] private Color rareColor = Color.blue;
        [SerializeField] private Color legendaryColor = new Color(1f, 0.5f, 0f);

        private bool isVisible = false;
        private Canvas canvas;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            // Canvas 찾기
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                canvas = FindObjectOfType<Canvas>();
            }

            // 초기 상태 설정
            if (tooltipPanel != null)
                tooltipPanel.SetActive(false);

            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }

        private void Update()
        {
            // 페이드 인/아웃 애니메이션
            if (canvasGroup != null)
            {
                float targetAlpha = isVisible ? 1f : 0f;
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

                // 완전히 사라지면 패널 비활성화
                if (!isVisible && canvasGroup.alpha < 0.01f)
                {
                    tooltipPanel.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 유물 툴팁을 표시합니다
        /// </summary>
        public void Show(Relic relic, Vector2 screenPosition)
        {
            if (relic == null)
            {
                Hide();
                return;
            }

            // 패널 활성화
            if (tooltipPanel != null)
                tooltipPanel.SetActive(true);

            isVisible = true;

            // 유물 정보 설정
            SetRelicInfo(relic);

            // 위치 설정
            SetPosition(screenPosition);
        }

        /// <summary>
        /// 툴팁을 숨깁니다
        /// </summary>
        public void Hide()
        {
            isVisible = false;
        }

        /// <summary>
        /// 유물 정보를 설정합니다
        /// </summary>
        private void SetRelicInfo(Relic relic)
        {
            // 이름
            if (nameText != null)
            {
                nameText.text = relic.name;
                nameText.color = GetRarityColor(relic.rarity);
            }

            // 희귀도
            if (rarityText != null)
            {
                rarityText.text = GetRarityText(relic.rarity);
                rarityText.color = GetRarityColor(relic.rarity);
            }

            // 설명
            if (descriptionText != null)
            {
                descriptionText.text = relic.description;
            }

            // 효과 타입
            if (effectTypeText != null)
            {
                effectTypeText.text = $"타입: {GetEffectTypeText(relic.effectType)}";
            }

            // 트리거 횟수 (통계)
            if (triggerCountText != null)
            {
                if (relic.triggerCount > 0)
                {
                    triggerCountText.text = $"발동 횟수: {relic.triggerCount}회";
                    triggerCountText.gameObject.SetActive(true);
                }
                else
                {
                    triggerCountText.gameObject.SetActive(false);
                }
            }

            // 아이콘
            if (iconImage != null && RelicIconManager.Instance != null)
            {
                iconImage.sprite = RelicIconManager.Instance.GetRelicIcon(relic);
            }

            // 배경 색상 (반투명)
            if (backgroundImage != null)
            {
                Color bgColor = GetRarityColor(relic.rarity);
                bgColor.a = 0.1f;
                backgroundImage.color = bgColor;
            }

            // 테두리 색상
            if (borderImage != null)
            {
                borderImage.color = GetRarityColor(relic.rarity);
            }
        }

        /// <summary>
        /// 툴팁 위치를 설정합니다 (화면 경계 체크 포함)
        /// </summary>
        private void SetPosition(Vector2 screenPosition)
        {
            if (tooltipRect == null || canvas == null)
                return;

            // 스크린 좌표를 캔버스 좌표로 변환
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPosition,
                canvas.worldCamera,
                out localPosition
            );

            // 오프셋 적용
            localPosition += offset;

            // 툴팁 크기
            Vector2 tooltipSize = tooltipRect.sizeDelta;

            // 캔버스 크기
            RectTransform canvasRect = canvas.transform as RectTransform;
            Vector2 canvasSize = canvasRect.sizeDelta;

            // 화면 경계 체크 및 조정
            // 오른쪽 경계
            if (localPosition.x + tooltipSize.x / 2 > canvasSize.x / 2 - edgePadding)
            {
                localPosition.x = screenPosition.x - offset.x - tooltipSize.x;
            }

            // 왼쪽 경계
            if (localPosition.x - tooltipSize.x / 2 < -canvasSize.x / 2 + edgePadding)
            {
                localPosition.x = -canvasSize.x / 2 + tooltipSize.x / 2 + edgePadding;
            }

            // 위쪽 경계
            if (localPosition.y + tooltipSize.y / 2 > canvasSize.y / 2 - edgePadding)
            {
                localPosition.y = canvasSize.y / 2 - tooltipSize.y / 2 - edgePadding;
            }

            // 아래쪽 경계
            if (localPosition.y - tooltipSize.y / 2 < -canvasSize.y / 2 + edgePadding)
            {
                localPosition.y = -canvasSize.y / 2 + tooltipSize.y / 2 + edgePadding;
            }

            tooltipRect.localPosition = localPosition;
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
                    return "일반";
                case RelicRarity.Uncommon:
                    return "고급";
                case RelicRarity.Rare:
                    return "진귀";
                case RelicRarity.Legendary:
                    return "전설";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 효과 타입 텍스트를 반환합니다
        /// </summary>
        private string GetEffectTypeText(RelicEffectType effectType)
        {
            switch (effectType)
            {
                case RelicEffectType.OnCombatStart:
                    return "전투 시작 시";
                case RelicEffectType.OnTurnStart:
                    return "턴 시작 시";
                case RelicEffectType.OnTurnEnd:
                    return "턴 종료 시";
                case RelicEffectType.OnCardPlay:
                    return "카드 사용 시";
                case RelicEffectType.OnAttack:
                    return "공격 시";
                case RelicEffectType.OnDefend:
                    return "방어 시";
                case RelicEffectType.OnDamageReceived:
                    return "피해 받을 시";
                case RelicEffectType.OnEnemyDeath:
                    return "적 사망 시";
                case RelicEffectType.OnDraw:
                    return "드로우 시";
                case RelicEffectType.OnDiscard:
                    return "카드 버릴 시";
                case RelicEffectType.Passive:
                    return "지속 효과";
                case RelicEffectType.OnRest:
                    return "휴식 시";
                case RelicEffectType.OnShop:
                    return "상점 진입 시";
                case RelicEffectType.OnVictory:
                    return "전투 승리 시";
                default:
                    return "알 수 없음";
            }
        }
    }
}
