using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace JianghuGuidebook.Map
{
    /// <summary>
    /// 지역 관리 및 전환을 담당하는 매니저
    /// </summary>
    public class RegionManager : MonoBehaviour
    {
        private static RegionManager _instance;

        public static RegionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RegionManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("RegionManager");
                        _instance = go.AddComponent<RegionManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("지역 데이터")]
        [SerializeField] private TextAsset[] regionConfigFiles; // 5개 지역 JSON 파일

        private Dictionary<string, Region> regionDatabase = new Dictionary<string, Region>();
        private List<Region> regionOrder = new List<Region>(); // 지역 순서
        private Region currentRegion;
        private int currentRegionIndex = 0;

        // Properties
        public Region CurrentRegion => currentRegion;
        public int CurrentRegionIndex => currentRegionIndex;
        public int TotalRegionCount => regionOrder.Count;
        public bool IsLastRegion => currentRegionIndex >= regionOrder.Count - 1;

        // Events
        public System.Action<Region> OnRegionEnter;
        public System.Action<Region> OnRegionComplete;
        public System.Action<Region, Region> OnRegionTransition; // (이전 지역, 새 지역)

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            LoadRegionData();
        }

        /// <summary>
        /// 지역 데이터를 로드합니다
        /// </summary>
        private void LoadRegionData()
        {
            Debug.Log("=== 지역 데이터 로드 시작 ===");

            // JSON 파일에서 지역 데이터 로드
            if (regionConfigFiles != null && regionConfigFiles.Length > 0)
            {
                foreach (var configFile in regionConfigFiles)
                {
                    if (configFile != null)
                    {
                        Region region = JsonUtility.FromJson<Region>(configFile.text);
                        if (region != null && region.Validate())
                        {
                            regionDatabase[region.id] = region;
                            regionOrder.Add(region);
                            Debug.Log($"지역 로드 완료: {region}");
                        }
                    }
                }

                // 지역 타입 순서대로 정렬
                regionOrder = regionOrder.OrderBy(r => r.regionType).ToList();
            }

            if (regionOrder.Count == 0)
            {
                Debug.LogWarning("로드된 지역 데이터가 없습니다. 기본 지역 생성");
                CreateDefaultRegion();
            }

            Debug.Log($"총 {regionOrder.Count}개 지역 로드 완료");
        }

        /// <summary>
        /// 기본 지역 생성 (데이터 없을 때)
        /// </summary>
        private void CreateDefaultRegion()
        {
            Region defaultRegion = new Region
            {
                id = "region_gangnam",
                name = "강남",
                regionType = RegionType.Gangnam,
                description = "강호 유랑의 시작",
                theme = "강호의 낭만",
                layerCount = 12,
                minNodesPerLayer = 3,
                maxNodesPerLayer = 5,
                bossId = "boss_blood_wolf",
                difficultyMultiplier = 1.0f,
                enemyHealthBonus = 0,
                enemyDamageBonus = 0
            };

            regionDatabase[defaultRegion.id] = defaultRegion;
            regionOrder.Add(defaultRegion);
        }

        /// <summary>
        /// 첫 지역으로 시작합니다
        /// </summary>
        public void StartFirstRegion()
        {
            if (regionOrder.Count == 0)
            {
                Debug.LogError("지역 데이터가 없습니다");
                return;
            }

            currentRegionIndex = 0;
            EnterRegion(regionOrder[0]);
        }

        /// <summary>
        /// 지역에 진입합니다
        /// </summary>
        public void EnterRegion(Region region)
        {
            if (region == null)
            {
                Debug.LogError("진입할 지역이 null입니다");
                return;
            }

            Debug.Log($"=== 지역 진입: {region.name} ===");
            currentRegion = region;

            OnRegionEnter?.Invoke(region);
        }

        /// <summary>
        /// 지역 ID로 진입합니다
        /// </summary>
        public void EnterRegion(string regionId)
        {
            if (regionDatabase.TryGetValue(regionId, out Region region))
            {
                // 지역 순서 업데이트
                currentRegionIndex = regionOrder.IndexOf(region);
                EnterRegion(region);
            }
            else
            {
                Debug.LogError($"지역 ID를 찾을 수 없습니다: {regionId}");
            }
        }

        /// <summary>
        /// 현재 지역을 완료하고 다음 지역으로 전환합니다
        /// </summary>
        public void CompleteCurrentRegion()
        {
            if (currentRegion == null)
            {
                Debug.LogError("현재 지역이 없습니다");
                return;
            }

            Debug.Log($"지역 완료: {currentRegion.name}");
            OnRegionComplete?.Invoke(currentRegion);

            if (IsLastRegion)
            {
                Debug.Log("=== 모든 지역 완료! 게임 클리어! ===");
                // 게임 클리어 처리는 GameManager에서
                return;
            }

            // 다음 지역으로 전환
            TransitionToNextRegion();
        }

        /// <summary>
        /// 다음 지역으로 전환합니다
        /// </summary>
        public void TransitionToNextRegion()
        {
            if (IsLastRegion)
            {
                Debug.LogWarning("마지막 지역입니다");
                return;
            }

            Region previousRegion = currentRegion;
            currentRegionIndex++;
            Region nextRegion = regionOrder[currentRegionIndex];

            Debug.Log($"=== 지역 전환: {previousRegion.name} → {nextRegion.name} ===");

            OnRegionTransition?.Invoke(previousRegion, nextRegion);
            EnterRegion(nextRegion);
        }

        /// <summary>
        /// 지역 ID로 지역 데이터를 가져옵니다
        /// </summary>
        public Region GetRegionById(string regionId)
        {
            if (regionDatabase.TryGetValue(regionId, out Region region))
            {
                return region;
            }

            Debug.LogError($"지역 ID를 찾을 수 없습니다: {regionId}");
            return null;
        }

        /// <summary>
        /// 모든 지역 리스트를 반환합니다
        /// </summary>
        public List<Region> GetAllRegions()
        {
            return new List<Region>(regionOrder);
        }

        /// <summary>
        /// 지역 진행도를 반환합니다 (1-5 중 몇 번째)
        /// </summary>
        public string GetRegionProgress()
        {
            return $"{currentRegionIndex + 1}/{regionOrder.Count}";
        }

        /// <summary>
        /// RegionManager를 리셋합니다 (새 게임 시작 시)
        /// </summary>
        public void ResetRegionManager()
        {
            currentRegion = null;
            currentRegionIndex = 0;
            Debug.Log("RegionManager 리셋 완료");
        }

        /// <summary>
        /// 현재 지역 정보를 출력합니다
        /// </summary>
        public void PrintCurrentRegionInfo()
        {
            if (currentRegion == null)
            {
                Debug.Log("현재 지역 없음");
                return;
            }

            Debug.Log("=== 현재 지역 정보 ===");
            Debug.Log(currentRegion);
            Debug.Log($"진행도: {GetRegionProgress()}");
            Debug.Log($"난이도 배율: {currentRegion.difficultyMultiplier}");
            Debug.Log($"적 풀 크기: {currentRegion.enemyPool.Count}");
            Debug.Log($"이벤트 풀 크기: {currentRegion.eventPool.Count}");
        }
    }
}
