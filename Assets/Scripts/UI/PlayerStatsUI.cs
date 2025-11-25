using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 플레이어의 체력, 내공, 방어도를 표시하는 UI 컴포넌트
    /// </summary>
    public class PlayerStatsUI : MonoBehaviour
    {
        [Header("Health UI")]
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image healthBarBackground;

        [Header("Energy UI")]
        [SerializeField] private TextMeshProUGUI energyText;
        [SerializeField] private Image energyIcon;
        [SerializeField] private GameObject[] energyOrbs; // 시각적 내공 구슬

        [Header("Block UI")]
        [SerializeField] private GameObject blockDisplay;
        [SerializeField] private TextMeshProUGUI blockText;
        [SerializeField] private Image blockIcon;

        [Header("Animation Settings")]
        [SerializeField] private float healthAnimationSpeed = 5f;
        [SerializeField] private Color lowHealthColor = Color.red;
        [SerializeField] private Color normalHealthColor = Color.green;
        [SerializeField] private float lowHealthThreshold = 0.3f;

        private Combat.Player player;
        private float currentHealthBarValue;
        private int lastDisplayedHealth;
        private int lastDisplayedEnergy;
        private int lastDisplayedBlock;

        private void Start()
        {
            player = FindObjectOfType<Combat.Player>();

            if (player == null)
            {
                Debug.LogError("PlayerStatsUI: Player not found in scene!");
                return;
            }

            // 초기 값 설정
            currentHealthBarValue = (float)player.CurrentHealth / player.MaxHealth;
            lastDisplayedHealth = player.CurrentHealth;
            lastDisplayedEnergy = player.CurrentEnergy;
            lastDisplayedBlock = player.Block;

            UpdateHealthUI(true);
            UpdateEnergyUI();
            UpdateBlockUI();
        }

        private void Update()
        {
            if (player == null) return;

            // 값이 변경되었는지 확인
            bool healthChanged = player.CurrentHealth != lastDisplayedHealth;
            bool energyChanged = player.CurrentEnergy != lastDisplayedEnergy;
            bool blockChanged = player.Block != lastDisplayedBlock;

            if (healthChanged)
            {
                UpdateHealthUI();
                lastDisplayedHealth = player.CurrentHealth;
            }

            if (energyChanged)
            {
                UpdateEnergyUI();
                lastDisplayedEnergy = player.CurrentEnergy;
            }

            if (blockChanged)
            {
                UpdateBlockUI();
                lastDisplayedBlock = player.Block;
            }

            // 체력 바 부드러운 애니메이션
            AnimateHealthBar();
        }

        /// <summary>
        /// 체력 UI 업데이트
        /// </summary>
        private void UpdateHealthUI(bool instant = false)
        {
            if (healthText != null)
            {
                healthText.text = $"{player.CurrentHealth} / {player.MaxHealth}";
            }

            float targetValue = (float)player.CurrentHealth / player.MaxHealth;

            if (instant)
            {
                currentHealthBarValue = targetValue;
                if (healthBarFill != null)
                {
                    healthBarFill.fillAmount = targetValue;
                }
            }

            // 체력 비율에 따른 색상 변경
            UpdateHealthBarColor(targetValue);
        }

        /// <summary>
        /// 체력 바 부드러운 애니메이션
        /// </summary>
        private void AnimateHealthBar()
        {
            if (healthBarFill == null) return;

            float targetValue = (float)player.CurrentHealth / player.MaxHealth;

            if (Mathf.Abs(currentHealthBarValue - targetValue) > 0.001f)
            {
                currentHealthBarValue = Mathf.Lerp(currentHealthBarValue, targetValue, Time.deltaTime * healthAnimationSpeed);
                healthBarFill.fillAmount = currentHealthBarValue;
            }
        }

        /// <summary>
        /// 체력 비율에 따른 체력 바 색상 변경
        /// </summary>
        private void UpdateHealthBarColor(float healthPercent)
        {
            if (healthBarFill == null) return;

            if (healthPercent <= lowHealthThreshold)
            {
                healthBarFill.color = lowHealthColor;
            }
            else
            {
                healthBarFill.color = Color.Lerp(normalHealthColor, lowHealthColor,
                    1f - (healthPercent - lowHealthThreshold) / (1f - lowHealthThreshold));
            }
        }

        /// <summary>
        /// 내공 UI 업데이트
        /// </summary>
        private void UpdateEnergyUI()
        {
            if (energyText != null)
            {
                energyText.text = $"{player.CurrentEnergy} / {player.MaxEnergy}";
            }

            // 내공 구슬 시각화 (있는 경우)
            UpdateEnergyOrbs();
        }

        /// <summary>
        /// 내공 구슬 시각화 업데이트
        /// </summary>
        private void UpdateEnergyOrbs()
        {
            if (energyOrbs == null || energyOrbs.Length == 0) return;

            for (int i = 0; i < energyOrbs.Length; i++)
            {
                if (energyOrbs[i] != null)
                {
                    energyOrbs[i].SetActive(i < player.CurrentEnergy);
                }
            }
        }

        /// <summary>
        /// 방어도 UI 업데이트
        /// </summary>
        private void UpdateBlockUI()
        {
            if (blockDisplay == null) return;

            if (player.Block > 0)
            {
                blockDisplay.SetActive(true);

                if (blockText != null)
                {
                    blockText.text = player.Block.ToString();
                }

                // 방어도 아이콘 펄스 효과
                AnimateBlockIcon();
            }
            else
            {
                blockDisplay.SetActive(false);
            }
        }

        /// <summary>
        /// 방어도 아이콘 펄스 애니메이션
        /// </summary>
        private void AnimateBlockIcon()
        {
            if (blockIcon == null) return;

            float scale = 1f + Mathf.Sin(Time.time * 3f) * 0.1f;
            blockIcon.transform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 피해를 받았을 때 호출 (흔들림 효과 등)
        /// </summary>
        public void OnTakeDamage(int damage)
        {
            StartCoroutine(ShakeEffect());
        }

        /// <summary>
        /// 화면 흔들림 효과
        /// </summary>
        private System.Collections.IEnumerator ShakeEffect()
        {
            Vector3 originalPosition = transform.localPosition;
            float duration = 0.3f;
            float elapsed = 0f;
            float magnitude = 10f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = originalPosition + new Vector3(x, y, 0);

                elapsed += Time.deltaTime;
                magnitude = Mathf.Lerp(10f, 0f, elapsed / duration);

                yield return null;
            }

            transform.localPosition = originalPosition;
        }

        /// <summary>
        /// 치유를 받았을 때 호출 (빛나는 효과 등)
        /// </summary>
        public void OnHeal(int amount)
        {
            StartCoroutine(HealEffect());
        }

        /// <summary>
        /// 치유 효과
        /// </summary>
        private System.Collections.IEnumerator HealEffect()
        {
            if (healthBarFill == null) yield break;

            Color originalColor = healthBarFill.color;
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                healthBarFill.color = Color.Lerp(Color.white, originalColor, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            healthBarFill.color = originalColor;
        }
    }
}
