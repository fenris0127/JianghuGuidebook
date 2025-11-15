using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 적의 체력과 다음 행동(Intent)을 표시하는 UI 클래스입니다.
public class EnemyUI : MonoBehaviour
{
    [Header("UI 요소 연결")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject intentPanel;
    [SerializeField] private Image intentIcon;
    [SerializeField] private TextMeshProUGUI intentValueText;

    [Header("스프라이트 리소스")]
    [SerializeField] private Sprite attackIconSprite;
    [SerializeField] private Sprite defendIconSprite;
    [SerializeField] private Sprite buffIconSprite;
    [SerializeField] private Sprite debuffIconSprite;

    private Enemy currentEnemy;

    public void Setup(Enemy enemy)
    {
        if (currentEnemy != null) { currentEnemy.OnStateChanged -= UpdateUI; }

        currentEnemy = enemy;

        if (currentEnemy != null)
        {
            currentEnemy.OnStateChanged += UpdateUI;
            UpdateUI(currentEnemy.currentHealth, currentEnemy.maxHealth, currentEnemy.GetNextAction());
        }

        gameObject.SetActive(enemy != null);
    }
    
    private void OnDestroy()
    {
        if (currentEnemy != null) { currentEnemy.OnStateChanged -= UpdateUI; }
    }

    private void UpdateUI(int currentHp, int maxHp, EnemyAction nextAction)
    {
        healthSlider.maxValue = maxHp;
        healthSlider.value = currentHp;
        healthText.text = $"{currentHp} / {maxHp}";

        if (intentPanel != null && nextAction != null)
        {
            intentPanel.SetActive(true);
            intentValueText.text = nextAction.value.ToString();

            switch (nextAction.actionType)
            {
                case EnemyActionType.Attack:
                    intentIcon.sprite = attackIconSprite;
                    intentValueText.text = nextAction.value.ToString();
                    break;
                case EnemyActionType.Defend:
                    intentIcon.sprite = defendIconSprite;
                    intentValueText.text = nextAction.value.ToString();
                    break;
                case EnemyActionType.Buff:
                    intentIcon.sprite = buffIconSprite;
                    break;
                case EnemyActionType.Debuff:
                    intentIcon.sprite = debuffIconSprite;
                    break;
                default:
                    intentPanel.SetActive(false);
                    break;
            }
        }
    }
}