using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JianghuGuidebook.Map;
using System.Collections;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 지역 전환 UI
    /// 지역 간 이동 시 전환 화면을 표시합니다
    /// </summary>
    public class RegionTransitionUI : MonoBehaviour
    {
        [Header("UI 요소")]
        [SerializeField] private GameObject transitionPanel;
        [SerializeField] private TextMeshProUGUI previousRegionText;
        [SerializeField] private TextMeshProUGUI nextRegionText;
        [SerializeField] private TextMeshProUGUI transitionMessageText;
        [SerializeField] private Image fadeImage;

        [Header("전환 설정")]
        [SerializeField] private float fadeInDuration = 1.0f;
        [SerializeField] private float displayDuration = 2.0f;
        [SerializeField] private float fadeOutDuration = 1.0f;

        [Header("색상 설정")]
        [SerializeField] private Color fadeColor = Color.black;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            // CanvasGroup 초기화
            if (transitionPanel != null)
            {
                canvasGroup = transitionPanel.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = transitionPanel.AddComponent<CanvasGroup>();
                }
            }

            // 초기 상태
            HideTransition();
        }

        private void Start()
        {
            // RegionManager 이벤트 구독
            if (RegionManager.Instance != null)
            {
                RegionManager.Instance.OnRegionTransition += OnRegionTransition;
            }
        }

        /// <summary>
        /// 지역 전환 이벤트 핸들러
        /// </summary>
        private void OnRegionTransition(Region previousRegion, Region nextRegion)
        {
            Debug.Log($"[RegionTransitionUI] 지역 전환: {previousRegion.name} → {nextRegion.name}");
            ShowTransition(previousRegion, nextRegion);
        }

        /// <summary>
        /// 지역 전환 화면 표시
        /// </summary>
        public void ShowTransition(Region previousRegion, Region nextRegion)
        {
            if (transitionPanel == null)
            {
                Debug.LogWarning("전환 패널이 설정되지 않았습니다");
                return;
            }

            StartCoroutine(TransitionCoroutine(previousRegion, nextRegion));
        }

        /// <summary>
        /// 전환 애니메이션 코루틴
        /// </summary>
        private IEnumerator TransitionCoroutine(Region previousRegion, Region nextRegion)
        {
            // 패널 활성화
            transitionPanel.SetActive(true);

            // 텍스트 설정
            if (previousRegionText != null)
            {
                previousRegionText.text = $"{previousRegion.name} 지역 완료";
            }

            if (nextRegionText != null)
            {
                nextRegionText.text = $"다음 지역: {nextRegion.name}";
            }

            if (transitionMessageText != null)
            {
                transitionMessageText.text = GetTransitionMessage(nextRegion);
            }

            // 페이드 이미지 색상 설정
            if (fadeImage != null)
            {
                fadeImage.color = fadeColor;
            }

            // 페이드 인
            yield return StartCoroutine(FadeIn());

            // 표시 시간
            yield return new WaitForSeconds(displayDuration);

            // 페이드 아웃
            yield return StartCoroutine(FadeOut());

            // 패널 비활성화
            HideTransition();
        }

        /// <summary>
        /// 페이드 인 애니메이션
        /// </summary>
        private IEnumerator FadeIn()
        {
            if (canvasGroup == null) yield break;

            float elapsed = 0f;
            canvasGroup.alpha = 0f;

            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// 페이드 아웃 애니메이션
        /// </summary>
        private IEnumerator FadeOut()
        {
            if (canvasGroup == null) yield break;

            float elapsed = 0f;
            canvasGroup.alpha = 1f;

            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }

        /// <summary>
        /// 지역별 전환 메시지 반환
        /// </summary>
        private string GetTransitionMessage(Region region)
        {
            switch (region.regionType)
            {
                case RegionType.Gangnam:
                    return "강호의 낭만이 가득한 강남으로...";

                case RegionType.Hubei:
                    return "무림맹의 본거지, 호북으로...";

                case RegionType.Shaanxi:
                    return "무림 고수들의 성지, 섬서로...";

                case RegionType.Sichuan:
                    return "독의 고장이자 비무의 땅, 사천으로...";

                case RegionType.Hebei:
                    return "황제의 발치, 최후의 결전지 하북으로...";

                default:
                    return $"{region.name}으로 향합니다...";
            }
        }

        /// <summary>
        /// 전환 화면 숨김
        /// </summary>
        private void HideTransition()
        {
            if (transitionPanel != null)
            {
                transitionPanel.SetActive(false);
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (RegionManager.Instance != null)
            {
                RegionManager.Instance.OnRegionTransition -= OnRegionTransition;
            }
        }
    }
}
