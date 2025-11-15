using UnityEngine;
using TMPro;

/// <summary>
/// 뽑을 덱, 버린 덱, 절초(소멸) 덱의 카드 개수를 표시하는 UI 클래스입니다.
/// </summary>
public class PileUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI drawPileCountText;
    [SerializeField] private TextMeshProUGUI discardPileCountText;
    [SerializeField] private GameObject exhaustPileGroup;
    [SerializeField] private TextMeshProUGUI exhaustPileCountText;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>(true);
        if (player != null)
        {
            player.OnPilesChanged += UpdatePileCounts;
            UpdatePileCounts(player.drawPile.Count, player.discardPile.Count, player.exhaustPile.Count);
        }
    }

    private void UpdatePileCounts(int drawCount, int discardCount, int exhaustCount)
    {
        drawPileCountText.text = drawCount.ToString();
        discardPileCountText.text = discardCount.ToString();
        
        exhaustPileGroup.SetActive(exhaustCount > 0);
        exhaustPileCountText.text = exhaustCount.ToString();
    }
}