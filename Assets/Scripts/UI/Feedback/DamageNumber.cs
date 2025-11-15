using UnityEngine;
using TMPro;

/// <summary>
/// 피해량을 표시하는 숫자가 떠올랐다가 사라지는 효과를 제어하는 클래스입니다.
/// </summary>
public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float horizontalSpeed = 0.5f;

    private float timer;

    void Awake()
    {
        // 생성될 때 무작위한 수평 속도를 가짐
        horizontalSpeed = Random.Range(-horizontalSpeed, horizontalSpeed);
    }

    void Update()
    {
        transform.position += new Vector3(horizontalSpeed, moveSpeed, 0) * Time.deltaTime;

        timer += Time.deltaTime;
        damageText.alpha = Mathf.Lerp(1f, 0f, timer / lifeTime);

        if (timer >= lifeTime)
            Destroy(gameObject);
    }

    public void SetDamage(int damage) => damageText.text = damage.ToString();
}