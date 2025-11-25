using UnityEngine;
using System.Collections.Generic;

namespace JianghuGuidebook.Relics
{
    /// <summary>
    /// 유물 아이콘을 관리하는 매니저
    /// </summary>
    public class RelicIconManager : MonoBehaviour
    {
        private static RelicIconManager _instance;

        public static RelicIconManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RelicIconManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("RelicIconManager");
                        _instance = go.AddComponent<RelicIconManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("기본 아이콘")]
        [SerializeField] private Sprite defaultIcon;
        [SerializeField] private Sprite commonIcon;
        [SerializeField] private Sprite uncommonIcon;
        [SerializeField] private Sprite rareIcon;
        [SerializeField] private Sprite legendaryIcon;

        [Header("유물별 아이콘")]
        [SerializeField] private List<RelicIconData> relicIcons = new List<RelicIconData>();

        private Dictionary<string, Sprite> iconCache = new Dictionary<string, Sprite>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            // 아이콘 캐시 초기화
            InitializeIconCache();
        }

        /// <summary>
        /// 아이콘 캐시를 초기화합니다
        /// </summary>
        private void InitializeIconCache()
        {
            iconCache.Clear();

            foreach (var iconData in relicIcons)
            {
                if (!string.IsNullOrEmpty(iconData.relicId) && iconData.icon != null)
                {
                    iconCache[iconData.relicId] = iconData.icon;
                }
            }

            Debug.Log($"RelicIconManager 초기화: {iconCache.Count}개 아이콘 로드됨");
        }

        /// <summary>
        /// 유물 ID로 아이콘을 가져옵니다
        /// </summary>
        public Sprite GetRelicIcon(string relicId)
        {
            if (iconCache.TryGetValue(relicId, out Sprite icon))
            {
                return icon;
            }

            // 아이콘이 없으면 기본 아이콘 반환
            return defaultIcon;
        }

        /// <summary>
        /// 유물 객체로 아이콘을 가져옵니다
        /// </summary>
        public Sprite GetRelicIcon(Relic relic)
        {
            if (relic == null)
                return defaultIcon;

            // 먼저 유물 ID로 찾기
            Sprite icon = GetRelicIcon(relic.id);

            // 아이콘이 없으면 희귀도에 맞는 기본 아이콘 반환
            if (icon == defaultIcon || icon == null)
            {
                return GetRarityIcon(relic.rarity);
            }

            return icon;
        }

        /// <summary>
        /// 희귀도에 따른 기본 아이콘을 반환합니다
        /// </summary>
        public Sprite GetRarityIcon(RelicRarity rarity)
        {
            switch (rarity)
            {
                case RelicRarity.Common:
                    return commonIcon != null ? commonIcon : defaultIcon;
                case RelicRarity.Uncommon:
                    return uncommonIcon != null ? uncommonIcon : defaultIcon;
                case RelicRarity.Rare:
                    return rareIcon != null ? rareIcon : defaultIcon;
                case RelicRarity.Legendary:
                    return legendaryIcon != null ? legendaryIcon : defaultIcon;
                default:
                    return defaultIcon;
            }
        }

        /// <summary>
        /// 런타임에 아이콘을 추가합니다
        /// </summary>
        public void AddIcon(string relicId, Sprite icon)
        {
            if (string.IsNullOrEmpty(relicId) || icon == null)
                return;

            iconCache[relicId] = icon;
            Debug.Log($"아이콘 추가: {relicId}");
        }

        /// <summary>
        /// Resources 폴더에서 아이콘을 로드합니다
        /// </summary>
        public void LoadIconFromResources(string relicId, string resourcePath)
        {
            Sprite icon = Resources.Load<Sprite>(resourcePath);
            if (icon != null)
            {
                AddIcon(relicId, icon);
            }
            else
            {
                Debug.LogWarning($"아이콘을 찾을 수 없습니다: {resourcePath}");
            }
        }

        /// <summary>
        /// 모든 유물 아이콘을 Resources 폴더에서 로드합니다
        /// </summary>
        public void LoadAllIconsFromResources(string folderPath = "Icons/Relics")
        {
            Sprite[] icons = Resources.LoadAll<Sprite>(folderPath);

            foreach (Sprite icon in icons)
            {
                // 파일 이름을 유물 ID로 사용
                string relicId = icon.name;
                AddIcon(relicId, icon);
            }

            Debug.Log($"{folderPath}에서 {icons.Length}개 아이콘 로드 완료");
        }
    }

    /// <summary>
    /// 유물 아이콘 데이터
    /// </summary>
    [System.Serializable]
    public class RelicIconData
    {
        public string relicId;
        public Sprite icon;
    }
}
