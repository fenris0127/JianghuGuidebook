using UnityEngine;
using System.Linq;

// 전투 중 플레이어의 클릭 입력을 받아 처리하는 클래스입니다.
public class BattleInteractionManager : MonoBehaviour
{
    public static BattleInteractionManager Instance;

    [Header("타겟팅 UI")]
    [SerializeField] private GameObject targetingArrowPrefab;
    private GameObject currentArrow;

    private CardData selectedCard;
    private Player player;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        player = FindObjectOfType<Player>(true); 
    }

    void Update()
    {
        if (currentArrow != null)
        {
            Vector3 startPos = player.transform.position;
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0;
            
            currentArrow.transform.position = (startPos + endPos) / 2;
            currentArrow.transform.right = endPos - startPos;
        }

        if (Input.GetMouseButtonDown(1))
            DeselectAll();
    }

    public void SelectCard(CardData card)
    {
        if (player.currentNaegong < card.cost) return;
        
        DeselectAll(); 
        selectedCard = card;
        
        if (card.effects == null || card.effects.Count == 0)
        {
            DeselectAll();
            return;
        }
        
        EffectTarget primaryTargetType = card.effects[0].target;
        
        if(primaryTargetType == EffectTarget.Self)
        {
            player.PlayCard(selectedCard, player); // 타겟이 정해져 있으므로 null 전달
            DeselectAll();
        }
        else if(primaryTargetType == EffectTarget.AllEnemies || primaryTargetType == EffectTarget.RandomEnemy)
        {
            player.PlayCard(selectedCard, null); // 대상 지정이 필요 없음
            DeselectAll();
        }
        else
        {
            if (targetingArrowPrefab != null)
                currentArrow = Instantiate(targetingArrowPrefab, transform);
        }
    }

    public void SelectTarget(IDamageable target)
    {
        if (selectedCard == null) return;

        EffectTarget primaryTargetType = selectedCard.effects.FirstOrDefault()?.target ?? EffectTarget.Self;
        if (primaryTargetType != EffectTarget.SingleEnemy) return;

        player.PlayCard(selectedCard, target as Component);
        DeselectAll();
    }

    private void DeselectAll()
    {
        selectedCard = null;
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }
    }
}