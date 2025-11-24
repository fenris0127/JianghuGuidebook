using UnityEngine;
using System.IO;
using JianghuGuidebook.Meta;

namespace JianghuGuidebook.Save
{
    /// <summary>
    /// 세이브/로드 시스템을 관리하는 매니저
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        private static SaveManager _instance;

        public static SaveManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SaveManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("SaveManager");
                        _instance = go.AddComponent<SaveManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("세이브 설정")]
        [SerializeField] private string saveFileName = "savegame";
        [SerializeField] private string saveFileExtension = ".json";
        [SerializeField] private int maxSaveSlots = 3;
        [SerializeField] private bool useEncryption = false; // 간단한 난독화 (실제 암호화 아님)

        private SaveData currentSaveData;

        // Properties
        public SaveData CurrentSaveData => currentSaveData;
        public bool HasSaveData => currentSaveData != null;

        // Events
        public System.Action<SaveData> OnGameSaved;
        public System.Action<SaveData> OnGameLoaded;
        public System.Action<int> OnAutoSaveTriggered;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSaveData();
        }

        /// <summary>
        /// 세이브 데이터를 초기화합니다
        /// </summary>
        private void InitializeSaveData()
        {
            currentSaveData = new SaveData();
            Debug.Log("SaveManager 초기화 완료");
        }

        /// <summary>
        /// 게임을 저장합니다
        /// </summary>
        public bool SaveGame(int slotIndex = 0)
        {
            if (slotIndex < 0 || slotIndex >= maxSaveSlots)
            {
                Debug.LogError($"유효하지 않은 세이브 슬롯: {slotIndex} (최대: {maxSaveSlots})");
                return false;
            }

            if (currentSaveData == null)
            {
                Debug.LogError("저장할 세이브 데이터가 없습니다");
                return false;
            }

            try
            {
                // 타임스탬프 업데이트
                currentSaveData.UpdateSaveTimestamp();

                // 런 데이터 타임스탬프 업데이트
                if (currentSaveData.currentRun != null)
                {
                    currentSaveData.currentRun.UpdateSaveTimestamp();
                }

                // 체크섬 생성
                currentSaveData.UpdateChecksum();

                // 데이터 검증
                if (!currentSaveData.Validate())
                {
                    Debug.LogError("세이브 데이터 유효성 검증 실패");
                    return false;
                }

                // JSON 직렬화
                string json = JsonUtility.ToJson(currentSaveData, true);

                // 난독화 (옵션)
                if (useEncryption)
                {
                    json = ObfuscateString(json);
                }

                // 파일 경로
                string filePath = GetSaveFilePath(slotIndex);

                // 디렉토리 생성
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 파일 저장
                File.WriteAllText(filePath, json);

                Debug.Log($"게임 저장 완료: {filePath}");
                Debug.Log($"SaveData: {currentSaveData}");

                OnGameSaved?.Invoke(currentSaveData);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"게임 저장 중 오류 발생: {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 게임을 불러옵니다
        /// </summary>
        public bool LoadGame(int slotIndex = 0)
        {
            if (slotIndex < 0 || slotIndex >= maxSaveSlots)
            {
                Debug.LogError($"유효하지 않은 세이브 슬롯: {slotIndex}");
                return false;
            }

            string filePath = GetSaveFilePath(slotIndex);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"세이브 파일이 없습니다: {filePath}");
                return false;
            }

            try
            {
                // 파일 읽기
                string json = File.ReadAllText(filePath);

                // 난독화 해제 (옵션)
                if (useEncryption)
                {
                    json = DeobfuscateString(json);
                }

                // JSON 역직렬화
                SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

                if (loadedData == null)
                {
                    Debug.LogError("세이브 파일 역직렬화 실패");
                    return false;
                }

                // 데이터 검증
                if (!loadedData.Validate())
                {
                    Debug.LogError("로드된 세이브 데이터 유효성 검증 실패");
                    return false;
                }

                // 체크섬 검증
                if (!loadedData.ValidateChecksum())
                {
                    Debug.LogWarning("세이브 파일 무결성 검증 실패 (체크섬 불일치)");
                    // 경고만 하고 로드는 진행 (선택적으로 실패 처리 가능)
                }

                currentSaveData = loadedData;

                Debug.Log($"게임 로드 완료: {filePath}");
                Debug.Log($"SaveData: {currentSaveData}");

                // 메타 데이터 적용
                ApplyMetaData();

                OnGameLoaded?.Invoke(currentSaveData);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"게임 로드 중 오류 발생: {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 세이브 파일이 존재하는지 확인합니다
        /// </summary>
        public bool SaveFileExists(int slotIndex)
        {
            string filePath = GetSaveFilePath(slotIndex);
            return File.Exists(filePath);
        }

        /// <summary>
        /// 세이브 파일을 삭제합니다
        /// </summary>
        public bool DeleteSaveFile(int slotIndex)
        {
            string filePath = GetSaveFilePath(slotIndex);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"삭제할 세이브 파일이 없습니다: {filePath}");
                return false;
            }

            try
            {
                File.Delete(filePath);
                Debug.Log($"세이브 파일 삭제 완료: {filePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"세이브 파일 삭제 중 오류 발생: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 자동 저장을 실행합니다
        /// </summary>
        public void AutoSave(int slotIndex = 0)
        {
            Debug.Log("=== 자동 저장 실행 ===");
            bool success = SaveGame(slotIndex);

            if (success)
            {
                OnAutoSaveTriggered?.Invoke(slotIndex);
            }
        }

        /// <summary>
        /// 세이브 파일 경로를 반환합니다
        /// </summary>
        private string GetSaveFilePath(int slotIndex)
        {
            string fileName = $"{saveFileName}_{slotIndex}{saveFileExtension}";
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        /// <summary>
        /// 메타 데이터를 시스템에 적용합니다
        /// </summary>
        private void ApplyMetaData()
        {
            if (currentSaveData.metaData == null)
            {
                Debug.LogWarning("메타 데이터가 없습니다");
                return;
            }

            // 무공 정수 적용
            MugongEssence.Instance.SetEssence(
                currentSaveData.metaData.currentEssence,
                currentSaveData.metaData.totalEssence
            );

            // 업그레이드 해금 상태 적용
            foreach (var upgradeUnlock in currentSaveData.metaData.unlockedUpgrades)
            {
                MetaProgressionManager.Instance.SetUpgradeUnlockState(
                    upgradeUnlock.upgradeId,
                    true,
                    upgradeUnlock.timesPurchased
                );
            }

            Debug.Log("메타 데이터 적용 완료");
        }

        /// <summary>
        /// 현재 게임 상태를 세이브 데이터에 업데이트합니다
        /// </summary>
        public void UpdateSaveDataFromGameState()
        {
            // TODO: 게임 상태를 currentSaveData에 반영
            // 플레이어 상태, 맵 진행, 덱, 유물 등

            Debug.Log("세이브 데이터 업데이트 (구현 필요)");
        }

        /// <summary>
        /// 문자열 난독화 (간단한 XOR)
        /// </summary>
        private string ObfuscateString(string input)
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte key = 0x42; // 간단한 키

            for (int i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] ^= key;
            }

            return System.Convert.ToBase64String(inputBytes);
        }

        /// <summary>
        /// 문자열 난독화 해제
        /// </summary>
        private string DeobfuscateString(string input)
        {
            byte[] inputBytes = System.Convert.FromBase64String(input);
            byte key = 0x42;

            for (int i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] ^= key;
            }

            return System.Text.Encoding.UTF8.GetString(inputBytes);
        }

        /// <summary>
        /// 현재 세이브 데이터 정보를 출력합니다
        /// </summary>
        public void PrintSaveInfo()
        {
            Debug.Log("=== 세이브 데이터 정보 ===");
            if (currentSaveData == null)
            {
                Debug.Log("세이브 데이터 없음");
                return;
            }

            Debug.Log($"버전: {currentSaveData.version}");
            Debug.Log($"저장 시간: {System.DateTimeOffset.FromUnixTimeSeconds(currentSaveData.saveTimestamp).ToString("yyyy-MM-dd HH:mm:ss")}");
            Debug.Log($"체크섬: {currentSaveData.checksum}");
            Debug.Log($"진행 중인 런: {(currentSaveData.HasActiveRun() ? "있음" : "없음")}");

            if (currentSaveData.HasActiveRun())
            {
                Debug.Log($"  - {currentSaveData.currentRun}");
                Debug.Log($"  - 플레이 시간: {currentSaveData.currentRun.GetFormattedPlayTime()}");
            }

            Debug.Log($"메타 데이터: {currentSaveData.metaData}");
        }
    }
}
