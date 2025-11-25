using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 적의 의도와 체력을 표시하는 UI 컴포넌트
    /// </summary>
    public class EnemyIntentUI : MonoBehaviour
    {
        [Header("Enemy Reference")]
        [SerializeField] private Combat.Enemy enemy;

        [Header("Health UI")]
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private GameObject healthBarContainer;

        [Header("Intent UI")]
        [SerializeField] private GameObject intentContainer;
        [SerializeField] private Image intentIcon;
        [SerializeField] private TextMeshProUGUI intentValueText;
        [SerializeField] private TextMeshProUGUI enemyNameText;

        [Header("Block UI")]
        [SerializeField] private GameObject blockDisplay;
        [SerializeField] private TextMeshProUGUI blockText;
        [SerializeField] private Image blockIcon;

        [Header("Intent Icons")]
        [SerializeField] private Sprite attackIcon;
        [SerializeField] private Sprite defendIcon;
        [SerializeField] private Sprite buffIcon;
        [SerializeField] private Sprite debuffIcon;
        [SerializeField] private Sprite unknownIcon;

        [Header("Animation Settings")]
        [SerializeField] private float healthAnimationSpeed = 5f;
        [SerializeField] private float intentPulseSpeed = 2f;

        private float currentHealthBarValue;
        private int lastDisplayedHealth;
        private int lastDisplayedBlock;

        private void Start()
        {
            if (enemy == null)
            {
                enemy = GetComponentInParent<Combat.Enemy>();
            }

            if (enemy == null)
            {
                Debug.LogError("EnemyIntentUI: Enemy reference not set!");
                return;
            }

            // 초기 값 설정
            currentHealthBarValue = (float)enemy.CurrentHealth / enemy.MaxHealth;
            lastDisplayedHealth = enemy.CurrentHealth;
            lastDisplayedBlock = enemy.Block;

            if (enemyNameText != null)
            {
                enemyNameText.text = enemy.EnemyName;
            }

            UpdateHealthUI(true);
            UpdateIntentUI();
            UpdateBlockUI();

            // 적 사망 이벤트 구독
            enemy.OnDeath += HandleEnemyDeath;
        }

        private void OnDestroy()
        {
            if (enemy != null)
            {
                enemy.OnDeath -= HandleEnemyDeath;
            }
        }

        private void Update()
        {
            if (enemy == null) return;

            // 값이 변경되었는지 확인
            bool healthChanged = enemy.CurrentHealth != lastDisplayedHealth;
            bool blockChanged = enemy.Block != lastDisplayedBlock;

            if (healthChanged)
            {
                UpdateHealthUI();
                lastDisplayedHealth = enemy.CurrentHealth;
            }

            if (blockChanged)
            {
                UpdateBlockUI();
                lastDisplayedBlock = enemy.Block;
            }

            // 체력 바 부드러운 애니메이션
            AnimateHealthBar();

            // 의도 아이콘 펄스 애니메이션
            AnimateIntentIcon();
        }

        /// <summary>
        /// 체력 UI 업데이트
        /// </summary>
        private void UpdateHealthUI(bool instant = false)
        {
            if (healthText != null)
            {
                healthText.text = $"{enemy.CurrentHealth} / {enemy.MaxHealth}";
            }

            float targetValue = (float)enemy.CurrentHealth / enemy.MaxHealth;

            if (instant)
            {
                currentHealthBarValue = targetValue;
                if (healthBarFill != null)
                {
                    healthBarFill.fillAmount = targetValue;
                }
            }

            // 체력이 0이면 체력 바 숨기기
            if (healthBarContainer != null)
            {
                healthBarContainer.SetActive(enemy.CurrentHealth > 0);
            }
        }

        /// <summary>
        /// 체력 바 부드러운 애니메이션
        /// </summary>
        private void AnimateHealthBar()
        {
            if (healthBarFill == null) return;

            float targetValue = (float)enemy.CurrentHealth / enemy.MaxHealth;

            if (Mathf.Abs(currentHealthBarValue - targetValue) > 0.001f)
            {
                currentHealthBarValue = Mathf.Lerp(currentHealthBarValue, targetValue, Time.deltaTime * healthAnimationSpeed);
                healthBarFill.fillAmount = currentHealthBarValue;
            }
        }

        /// <summary>
        /// 의도 UI 업데이트
        /// </summary>
        public void UpdateIntentUI()
        {
            if (enemy == null || intentContainer == null) return;

            var intent = enemy.CurrentIntent;

            if (intent == null)
            {
                intentContainer.SetActive(false);
                return;
            }

            intentContainer.SetActive(true);

            // 의도 아이콘 설정
            if (intentIcon != null)
            {
                intentIcon.sprite = GetIntentSprite(intent.Type);
                intentIcon.color = GetIntentColor(intent.Type);
            }

            // 의도 값 표시 (공격/방어 등의 수치)
            if (intentValueText != null)
            {
                if (intent.Value > 0)
                {
                    intentValueText.text = intent.Value.ToString();
                    intentValueText.gameObject.SetActive(true);
                }
                else
                {
                    intentValueText.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 의도 타입에 따른 스프라이트 반환
        /// </summary>
        private Sprite GetIntentSprite(AI.IntentType type)
        {
            switch (type)
            {
                case AI.IntentType.Attack:
                    return attackIcon;
                case AI.IntentType.Defend:
                    return defendIcon;
                case AI.IntentType.Buff:
                    return buffIcon;
                case AI.IntentType.Debuff:
                    return debuffIcon;
                default:
                    return unknownIcon;
            }
        }

        /// <summary>
        /// 의도 타입에 따른 색상 반환
        /// </summary>
        private Color GetIntentColor(AI.IntentType type)
        {
            switch (type)
            {
                case AI.IntentType.Attack:
                    return new Color(1f, 0.3f, 0.3f); // 빨강
                case AI.IntentType.Defend:
                    return new Color(0.3f, 0.6f, 1f); // 파랑
                case AI.IntentType.Buff:
                    return new Color(0.3f, 1f, 0.3f); // 초록
                case AI.IntentType.Debuff:
                    return new Color(0.7f, 0.3f, 1f); // 보라
                default:
                    return Color.gray;
            }
        }

        /// <summary>
        /// 의도 아이콘 펄스 애니메이션
        /// </summary>
        private void AnimateIntentIcon()
        {
            if (intentIcon == null || !intentContainer.activeSelf) return;

            float scale = 1f + Mathf.Sin(Time.time * intentPulseSpeed) * 0.15f;
            intentIcon.transform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 방어도 UI 업데이트
        /// </summary>
        private void UpdateBlockUI()
        {
            if (blockDisplay == null) return;

            if (enemy.Block > 0)
            {
                blockDisplay.SetActive(true);

                if (blockText != null)
                {
                    blockText.text = enemy.Block.ToString();
                }
            }
            else
            {
                blockDisplay.SetActive(false);
            }
        }

        /// <summary>
        /// 적이 피해를 받았을 때 호출
        /// </summary>
        public void OnTakeDamage(int damage)
        {
            StartCoroutine(DamageFlashEffect());
        }

        /// <summary>
        /// 피해 받았을 때 깜빡임 효과
        /// </summary>
        private System.Collections.IEnumerator DamageFlashEffect()
        {
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer == null) yield break;

            Color originalColor = spriteRenderer.color;
            float duration = 0.2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                spriteRenderer.color = Color.Lerp(Color.red, originalColor, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = originalColor;
        }

        /// <summary>
        /// 적 사망 처리
        /// </summary>
        private void HandleEnemyDeath()
        {
            StartCoroutine(DeathAnimation());
        }

        /// <summary>
        /// 사망 애니메이션
        /// </summary>
        private System.Collections.IEnumerator DeathAnimation()
        {
            float duration = 0.5f;
            float elapsed = 0f;

            Vector3 originalScale = transform.localScale;
            CanvasGroup canvasGroup = GetComponentInChildren<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
                canvasGroup.alpha = 1f - t;

                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
