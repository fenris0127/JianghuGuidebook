using UnityEngine;
using TMPro;

namespace JianghuGuidebook.UI
{
    /// <summary>
    /// 피해, 방어도, 치유 등의 숫자를 팝업으로 표시하는 컴포넌트
    /// </summary>
    public class DamagePopup : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Animation Settings")]
        [SerializeField] private float lifetime = 1.5f;
        [SerializeField] private float moveSpeed = 100f;
        [SerializeField] private float fadeSpeed = 2f;
        [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Visual Settings")]
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private Color healColor = Color.green;
        [SerializeField] private Color blockColor = Color.blue;
        [SerializeField] private float scaleMultiplier = 1.5f;

        private float elapsedTime = 0f;
        private Vector3 startPosition;
        private Vector3 targetOffset;

        public enum PopupType
        {
            Damage,
            Heal,
            Block,
            Miss
        }

        private void Awake()
        {
            if (damageText == null)
            {
                damageText = GetComponentInChildren<TextMeshProUGUI>();
            }

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }

            startPosition = transform.position;
            targetOffset = new Vector3(Random.Range(-50f, 50f), moveSpeed, 0);
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;

            // 위로 떠오르는 애니메이션
            float curveValue = movementCurve.Evaluate(elapsedTime / lifetime);
            transform.position = startPosition + targetOffset * curveValue;

            // 페이드 아웃
            canvasGroup.alpha = 1f - (elapsedTime / lifetime);

            // 수명이 다하면 파괴
            if (elapsedTime >= lifetime)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 팝업 초기화 및 표시
        /// </summary>
        public void Initialize(int value, PopupType type)
        {
            switch (type)
            {
                case PopupType.Damage:
                    SetupDamage(value);
                    break;
                case PopupType.Heal:
                    SetupHeal(value);
                    break;
                case PopupType.Block:
                    SetupBlock(value);
                    break;
                case PopupType.Miss:
                    SetupMiss();
                    break;
            }
        }

        /// <summary>
        /// 피해 팝업 설정
        /// </summary>
        private void SetupDamage(int damage)
        {
            damageText.text = damage.ToString();
            damageText.color = damageColor;
            damageText.fontSize = 36 * scaleMultiplier;
            damageText.fontStyle = FontStyles.Bold;
        }

        /// <summary>
        /// 치유 팝업 설정
        /// </summary>
        private void SetupHeal(int heal)
        {
            damageText.text = $"+{heal}";
            damageText.color = healColor;
            damageText.fontSize = 32 * scaleMultiplier;
            damageText.fontStyle = FontStyles.Bold;
        }

        /// <summary>
        /// 방어도 팝업 설정
        /// </summary>
        private void SetupBlock(int block)
        {
            damageText.text = $"+{block}";
            damageText.color = blockColor;
            damageText.fontSize = 32 * scaleMultiplier;
            damageText.fontStyle = FontStyles.Bold;
        }

        /// <summary>
        /// 빗나감 팝업 설정
        /// </summary>
        private void SetupMiss()
        {
            damageText.text = "Miss!";
            damageText.color = Color.gray;
            damageText.fontSize = 28 * scaleMultiplier;
            damageText.fontStyle = FontStyles.Italic;
        }

        /// <summary>
        /// 정적 팝업 생성 메서드
        /// </summary>
        public static void Create(Vector3 position, int value, PopupType type, Transform parent = null)
        {
            // 프리팹 로드 (Resources 폴더에서)
            GameObject prefab = Resources.Load<GameObject>("Prefabs/DamagePopup");

            if (prefab == null)
            {
                Debug.LogWarning("DamagePopup prefab not found in Resources/Prefabs/");
                return;
            }

            GameObject popup = Instantiate(prefab, position, Quaternion.identity, parent);
            DamagePopup damagePopup = popup.GetComponent<DamagePopup>();

            if (damagePopup != null)
            {
                damagePopup.Initialize(value, type);
            }
        }

        /// <summary>
        /// 프리팹 없이 동적으로 팝업 생성
        /// </summary>
        public static void CreateDynamic(Vector3 position, int value, PopupType type, Canvas canvas)
        {
            // 동적으로 GameObject 생성
            GameObject popupObj = new GameObject("DamagePopup");
            popupObj.transform.SetParent(canvas.transform, false);

            // RectTransform 설정
            RectTransform rectTransform = popupObj.AddComponent<RectTransform>();
            rectTransform.position = position;

            // TextMeshProUGUI 추가
            TextMeshProUGUI text = popupObj.AddComponent<TextMeshProUGUI>();
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 36;

            // DamagePopup 컴포넌트 추가 및 초기화
            DamagePopup popup = popupObj.AddComponent<DamagePopup>();
            popup.damageText = text;
            popup.Initialize(value, type);
        }
    }
}
