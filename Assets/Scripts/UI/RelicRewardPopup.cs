using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using JianghuGuidebook.Relics;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 유물 획득 연출 팝업
    /// </summary>
    public class RelicRewardPopup : MonoBehaviour
    {
        private static RelicRewardPopup _instance;

        public static RelicRewardPopup Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RelicRewardPopup>();
                }
                return _instance;
            }
        }

        [Header("UI 참조")]
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform contentRect;

        [Header("유물 정보")]
        [SerializeField] private Image relicIcon;
        [SerializeField] private Image relicGlow;
        [SerializeField] private Image relicBackground;
        [SerializeField] private Image relicBorder;
        [SerializeField] private TextMeshProUGUI relicNameText;
        [SerializeField] private TextMeshProUGUI relicRarityText;
        [SerializeField] private TextMeshProUGUI relicDescriptionText;

        [Header("애니메이션")]
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float displayDuration = 3f;
        [SerializeField] private float fadeOutDuration = 0.5f;
        [SerializeField] private float scaleInDuration = 0.6f;
        [SerializeField] private AnimationCurve scaleInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float glowRotationSpeed = 30f;

        [Header("파티클 효과")]
        [SerializeField] private ParticleSystem sparkleEffect;
        [SerializeField] private ParticleSystem glowEffect;
        [SerializeField] private ParticleSystem rarityEffect;

        [Header("색상")]
        [SerializeField] private Color commonColor = Color.white;
        [SerializeField] private Color uncommonColor = Color.green;
        [SerializeField] private Color rareColor = Color.blue;
        [SerializeField] private Color legendaryColor = new Color(1f, 0.5f, 0f);

        [Header("사운드")]
        [SerializeField] private AudioClip relicObtainSound;
        [SerializeField] private AudioClip legendarySound;

        private Coroutine currentAnimation;
        private bool isShowing = false;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            // 초기 상태 설정
            if (popupPanel != null)
                popupPanel.SetActive(false);

            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }

        private void Update()
        {
            // 글로우 효과 회전
            if (isShowing && relicGlow != null)
            {
                relicGlow.transform.Rotate(Vector3.forward, glowRotationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// 유물 획득 팝업을 표시합니다
        /// </summary>
        public void ShowRelicReward(Relic relic)
        {
            if (relic == null)
            {
                Debug.LogWarning("표시할 유물이 null입니다");
                return;
            }

            // 진행 중인 애니메이션 중지
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }

            currentAnimation = StartCoroutine(ShowRelicAnimation(relic));
        }

        /// <summary>
        /// 유물 획득 애니메이션 코루틴
        /// </summary>
        private IEnumerator ShowRelicAnimation(Relic relic)
        {
            isShowing = true;

            // 패널 활성화
            if (popupPanel != null)
                popupPanel.SetActive(true);

            // 유물 정보 설정
            SetRelicInfo(relic);

            // 초기 크기 설정 (0)
            if (contentRect != null)
                contentRect.localScale = Vector3.zero;

            // 페이드 인 & 스케일 인 애니메이션
            float elapsed = 0f;
            while (elapsed < scaleInDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / scaleInDuration;

                // 페이드
                if (canvasGroup != null)
                    canvasGroup.alpha = t;

                // 스케일
                if (contentRect != null)
                {
                    float scale = scaleInCurve.Evaluate(t);
                    contentRect.localScale = Vector3.one * scale;
                }

                yield return null;
            }

            // 최종 값 보정
            if (canvasGroup != null)
                canvasGroup.alpha = 1f;
            if (contentRect != null)
                contentRect.localScale = Vector3.one;

            // 파티클 효과 재생
            PlayParticleEffects(relic.rarity);

            // 사운드 재생
            PlaySound(relic.rarity);

            // 표시 시간 대기
            yield return new WaitForSeconds(displayDuration);

            // 페이드 아웃
            elapsed = 0f;
            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeOutDuration;

                if (canvasGroup != null)
                    canvasGroup.alpha = 1f - t;

                yield return null;
            }

            // 패널 비활성화
            if (popupPanel != null)
                popupPanel.SetActive(false);

            isShowing = false;
            currentAnimation = null;
        }

        /// <summary>
        /// 유물 정보를 UI에 설정합니다
        /// </summary>
        private void SetRelicInfo(Relic relic)
        {
            Color rarityColor = GetRarityColor(relic.rarity);

            // 이름
            if (relicNameText != null)
            {
                relicNameText.text = relic.name;
                relicNameText.color = rarityColor;
            }

            // 희귀도
            if (relicRarityText != null)
            {
                relicRarityText.text = GetRarityText(relic.rarity);
                relicRarityText.color = rarityColor;
            }

            // 설명
            if (relicDescriptionText != null)
            {
                relicDescriptionText.text = relic.description;
            }

            // 아이콘
            if (relicIcon != null && RelicIconManager.Instance != null)
            {
                relicIcon.sprite = RelicIconManager.Instance.GetRelicIcon(relic);
            }

            // 글로우 효과
            if (relicGlow != null)
            {
                relicGlow.color = rarityColor;
            }

            // 배경
            if (relicBackground != null)
            {
                Color bgColor = rarityColor;
                bgColor.a = 0.2f;
                relicBackground.color = bgColor;
            }

            // 테두리
            if (relicBorder != null)
            {
                relicBorder.color = rarityColor;
            }
        }

        /// <summary>
        /// 파티클 효과를 재생합니다
        /// </summary>
        private void PlayParticleEffects(RelicRarity rarity)
        {
            // 기본 반짝임 효과
            if (sparkleEffect != null)
            {
                sparkleEffect.Play();
            }

            // 글로우 효과
            if (glowEffect != null)
            {
                var main = glowEffect.main;
                main.startColor = GetRarityColor(rarity);
                glowEffect.Play();
            }

            // 희귀도 특별 효과
            if (rarityEffect != null)
            {
                // 진귀 이상에서만 재생
                if (rarity >= RelicRarity.Rare)
                {
                    var main = rarityEffect.main;
                    main.startColor = GetRarityColor(rarity);
                    rarityEffect.Play();
                }
            }
        }

        /// <summary>
        /// 사운드를 재생합니다
        /// </summary>
        private void PlaySound(RelicRarity rarity)
        {
            AudioClip clipToPlay = null;

            // 전설 유물은 특별한 사운드
            if (rarity == RelicRarity.Legendary && legendarySound != null)
            {
                clipToPlay = legendarySound;
            }
            else if (relicObtainSound != null)
            {
                clipToPlay = relicObtainSound;
            }

            if (clipToPlay != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(clipToPlay);
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
                    return "【일반 유물】";
                case RelicRarity.Uncommon:
                    return "【고급 유물】";
                case RelicRarity.Rare:
                    return "【진귀한 유물】";
                case RelicRarity.Legendary:
                    return "【전설의 유물】";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 팝업을 즉시 닫습니다
        /// </summary>
        public void CloseImmediate()
        {
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
                currentAnimation = null;
            }

            isShowing = false;

            if (popupPanel != null)
                popupPanel.SetActive(false);

            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }

        /// <summary>
        /// 현재 표시 중인지 확인합니다
        /// </summary>
        public bool IsShowing => isShowing;
    }
}
