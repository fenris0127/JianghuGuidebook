using UnityEngine;

// 데미지 숫자 등 시각적 피드백 효과를 생성하고 관리하는 클래스입니다.
public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private Canvas worldCanvas; // Render Mode가 World Space인 Canvas

    void Awake()
    {
        Instance = this;
    }

    public void ShowDamageNumber(int damage, Vector3 position)
    {
        if (damageNumberPrefab == null || worldCanvas == null) return;
        
        // 월드 좌표에 약간의 랜덤성을 더해 숫자가 겹치지 않게 함
        Vector3 spawnPosition = position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);
        
        GameObject numberObj = Instantiate(damageNumberPrefab, spawnPosition, Quaternion.identity, worldCanvas.transform);
        numberObj.GetComponent<DamageNumber>().SetDamage(damage);
    }
}