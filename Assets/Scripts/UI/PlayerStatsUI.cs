using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 플레이어의 체력, 방어도, 내공 등 핵심 능력치를 표시하는 UI 클래스입니다.
public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject defenseIconAndText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI naegongText;
    [SerializeField] private Slider healthSlider;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>(true);
        if (player != null)
        {
            player.OnStatsChanged += UpdateUI;
            // 초기값 설정
            UpdateUI(player.currentHealth, player.maxHealth, player.defense, player.currentNaegong, player.maxNaegong);
        }
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnStatsChanged -= UpdateUI;
    }

    private void UpdateUI(int currentHp, int maxHp, int defense, int currentNaegong, int maxNaegong)
    {
        healthText.text = $"{currentHp} / {maxHp}";

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHp;
            healthSlider.value = currentHp;
        }

        defenseIconAndText.SetActive(defense > 0);
        defenseText.text = defense.ToString();

        naegongText.text = $"{currentNaegong} / {maxNaegong}";
    }
}