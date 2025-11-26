using UnityEngine;
using System.IO;
using JianghuGuidebook.Meta;

namespace JianghuGuidebook.Save
{
    /// <summary>
    /// 세이브 파일 손상 유형
    /// </summary>
    public enum SaveCorruptionType
    {
        None,                   // 손상 없음
        FileNotFound,          // 파일이 존재하지 않음
        JsonParseError,        // JSON 파싱 실패
        ChecksumMismatch,      // 체크섬 불일치
        ValidationFailed,      // 데이터 검증 실패
        DeserializationFailed, // 역직렬화 실패
        Unknown                // 알 수 없는 오류
    }

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
        [SerializeField] private string metaSaveFileName = "metadata";
        [SerializeField] private string saveFileExtension = ".json";
        [SerializeField] private int maxSaveSlots = 3;
        [SerializeField] private bool useEncryption = false; // 간단한 난독화 (실제 암호화 아님)

        private SaveData currentSaveData;

        // Properties
        public SaveData CurrentSaveData => currentSaveData;
        public bool HasSaveData => currentSaveData != null;
        public MetaSaveData MetaData => currentSaveData?.metaData;

        // Events
        public System.Action<SaveData> OnGameSaved;
        public System.Action<SaveData> OnGameLoaded;
        public System.Action<int> OnAutoSaveTriggered;
        public System.Action<MetaSaveData> OnMetaDataSaved;
        public System.Action<MetaSaveData> OnMetaDataLoaded;

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

            // 메타 데이터 자동 로드 (있으면)
            if (MetaSaveFileExists())
            {
                LoadMetaData();
            }

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
                // 파일 경로
                string filePath = GetSaveFilePath(slotIndex);

                // 기존 파일이 있으면 백업 생성
                if (File.Exists(filePath))
                {
                    CreateBackupFile(filePath);
                }

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

            SaveCorruptionType corruptionType = SaveCorruptionType.None;

            try
            {
                // 파일 읽기
                string json = File.ReadAllText(filePath);

                // 난독화 해제 (옵션)
                if (useEncryption)
                {
                    try
                    {
                        json = DeobfuscateString(json);
                    }
                    catch (System.Exception)
                    {
                        corruptionType = SaveCorruptionType.JsonParseError;
                        throw;
                    }
                }

                // JSON 역직렬화
                SaveData loadedData = null;
                try
                {
                    loadedData = JsonUtility.FromJson<SaveData>(json);
                }
                catch (System.Exception)
                {
                    corruptionType = SaveCorruptionType.DeserializationFailed;
                    throw;
                }

                if (loadedData == null)
                {
                    corruptionType = SaveCorruptionType.DeserializationFailed;
                    Debug.LogError("세이브 파일 역직렬화 실패 - null 반환");
                    throw new System.Exception("역직렬화 결과가 null입니다");
                }

                // 데이터 검증
                if (!loadedData.Validate())
                {
                    corruptionType = SaveCorruptionType.ValidationFailed;
                    Debug.LogError("로드된 세이브 데이터 유효성 검증 실패");
                    throw new System.Exception("데이터 검증 실패");
                }

                // 체크섬 검증
                if (!loadedData.ValidateChecksum())
                {
                    corruptionType = SaveCorruptionType.ChecksumMismatch;
                    Debug.LogWarning("세이브 파일 무결성 검증 실패 (체크섬 불일치)");
                    throw new System.Exception("체크섬 불일치");
                }

                currentSaveData = loadedData;

                Debug.Log($"게임 로드 완료: {filePath}");
                Debug.Log($"SaveData: {currentSaveData}");

                // 마이그레이션 체크 및 실행
                if (currentSaveData.NeedsMigration())
                {
                    Debug.LogWarning($"세이브 파일 마이그레이션 필요: {currentSaveData.version} -> {SaveData.CURRENT_VERSION}");
                    MigrateSaveData(currentSaveData);

                    // 마이그레이션 후 저장
                    SaveGame(slotIndex);
                    Debug.Log("마이그레이션 완료 및 저장 완료");
                }

                // 메타 데이터 적용
                ApplyMetaData();

                OnGameLoaded?.Invoke(currentSaveData);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"게임 로드 중 오류 발생 ({corruptionType}): {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");

                // 손상된 파일 처리
                HandleCorruptedSaveFile(filePath, slotIndex, corruptionType);

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

        // ===== 메타 데이터 별도 저장/로드 =====

        /// <summary>
        /// 메타 데이터를 별도 파일로 저장합니다
        /// </summary>
        public bool SaveMetaData()
        {
            if (currentSaveData == null || currentSaveData.metaData == null)
            {
                Debug.LogError("저장할 메타 데이터가 없습니다");
                return false;
            }

            try
            {
                // 파일 경로
                string filePath = GetMetaSaveFilePath();

                // 기존 파일이 있으면 백업 생성
                if (File.Exists(filePath))
                {
                    CreateBackupFile(filePath);
                }

                // 메타 데이터 검증
                if (!currentSaveData.metaData.Validate())
                {
                    Debug.LogError("메타 데이터 유효성 검증 실패");
                    return false;
                }

                // JSON 직렬화
                string json = JsonUtility.ToJson(currentSaveData.metaData, true);

                // 난독화 (옵션)
                if (useEncryption)
                {
                    json = ObfuscateString(json);
                }

                // 디렉토리 생성
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 파일 저장
                File.WriteAllText(filePath, json);

                Debug.Log($"[SaveManager] 메타 데이터 저장 완료: {filePath}");
                Debug.Log($"[SaveManager] {currentSaveData.metaData}");

                OnMetaDataSaved?.Invoke(currentSaveData.metaData);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"메타 데이터 저장 중 오류 발생: {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 메타 데이터를 별도 파일에서 로드합니다
        /// </summary>
        public bool LoadMetaData()
        {
            string filePath = GetMetaSaveFilePath();

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"메타 데이터 파일이 없습니다: {filePath}");
                return false;
            }

            SaveCorruptionType corruptionType = SaveCorruptionType.None;

            try
            {
                // 파일 읽기
                string json = File.ReadAllText(filePath);

                // 난독화 해제 (옵션)
                if (useEncryption)
                {
                    try
                    {
                        json = DeobfuscateString(json);
                    }
                    catch (System.Exception)
                    {
                        corruptionType = SaveCorruptionType.JsonParseError;
                        throw;
                    }
                }

                // JSON 역직렬화
                MetaSaveData loadedMetaData = null;
                try
                {
                    loadedMetaData = JsonUtility.FromJson<MetaSaveData>(json);
                }
                catch (System.Exception)
                {
                    corruptionType = SaveCorruptionType.DeserializationFailed;
                    throw;
                }

                if (loadedMetaData == null)
                {
                    corruptionType = SaveCorruptionType.DeserializationFailed;
                    Debug.LogError("메타 데이터 파일 역직렬화 실패 - null 반환");
                    throw new System.Exception("역직렬화 결과가 null입니다");
                }

                // 데이터 검증
                if (!loadedMetaData.Validate())
                {
                    corruptionType = SaveCorruptionType.ValidationFailed;
                    Debug.LogError("로드된 메타 데이터 유효성 검증 실패");
                    throw new System.Exception("데이터 검증 실패");
                }

                // 현재 세이브 데이터에 메타 데이터 적용
                if (currentSaveData == null)
                {
                    currentSaveData = new SaveData();
                }

                currentSaveData.metaData = loadedMetaData;

                Debug.Log($"[SaveManager] 메타 데이터 로드 완료: {filePath}");
                Debug.Log($"[SaveManager] {loadedMetaData}");

                // 메타 데이터 시스템에 적용
                ApplyMetaData();

                OnMetaDataLoaded?.Invoke(loadedMetaData);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"메타 데이터 로드 중 오류 발생 ({corruptionType}): {e.Message}");
                Debug.LogError($"StackTrace: {e.StackTrace}");

                // 손상된 파일 처리
                HandleCorruptedMetaFile(filePath, corruptionType);

                return false;
            }
        }

        /// <summary>
        /// 메타 세이브 파일이 존재하는지 확인합니다
        /// </summary>
        public bool MetaSaveFileExists()
        {
            string filePath = GetMetaSaveFilePath();
            return File.Exists(filePath);
        }

        /// <summary>
        /// 메타 세이브 파일을 삭제합니다
        /// </summary>
        public bool DeleteMetaSaveFile()
        {
            string filePath = GetMetaSaveFilePath();

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"삭제할 메타 데이터 파일이 없습니다: {filePath}");
                return false;
            }

            try
            {
                File.Delete(filePath);
                Debug.Log($"메타 데이터 파일 삭제 완료: {filePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"메타 데이터 파일 삭제 중 오류 발생: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 메타 세이브 파일 경로를 반환합니다
        /// </summary>
        private string GetMetaSaveFilePath()
        {
            string fileName = $"{metaSaveFileName}{saveFileExtension}";
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        /// <summary>
        /// 현재 게임 상태에서 메타 데이터를 업데이트합니다
        /// </summary>
        public void UpdateMetaDataFromGameState()
        {
            if (currentSaveData == null || currentSaveData.metaData == null)
            {
                Debug.LogWarning("메타 데이터가 초기화되지 않았습니다");
                return;
            }

            // MugongEssence에서 정수 정보 가져오기
            if (MugongEssence.Instance != null)
            {
                currentSaveData.metaData.currentEssence = MugongEssence.Instance.CurrentEssence;
                currentSaveData.metaData.totalEssence = MugongEssence.Instance.TotalEssence;
            }

            // MetaProgressionManager에서 업그레이드 정보 가져오기
            if (MetaProgressionManager.Instance != null)
            {
                currentSaveData.metaData.unlockedUpgrades.Clear();

                var upgrades = MetaProgressionManager.Instance.GetUnlockedUpgrades();
                foreach (var upgrade in upgrades)
                {
                    currentSaveData.metaData.AddUpgradeUnlock(
                        upgrade.id,
                        upgrade.timesPurchased
                    );
                }
            }

            Debug.Log("[SaveManager] 메타 데이터 업데이트 완료");
        }

        /// <summary>
        /// 메타 데이터 정보를 출력합니다
        /// </summary>
        public void PrintMetaInfo()
        {
            Debug.Log("=== 메타 데이터 정보 ===");
            if (currentSaveData == null || currentSaveData.metaData == null)
            {
                Debug.Log("메타 데이터 없음");
                return;
            }

            MetaSaveData meta = currentSaveData.metaData;
            Debug.Log($"무공 정수: {meta.currentEssence} / {meta.totalEssence}");
            Debug.Log($"업그레이드: {meta.unlockedUpgrades.Count}개");
            Debug.Log($"런 통계:");
            Debug.Log($"  - 완료: {meta.totalRunsCompleted}회");
            Debug.Log($"  - 승리: {meta.totalVictories}회");
            Debug.Log($"  - 사망: {meta.totalDeaths}회");
            Debug.Log($"  - 적 처치: {meta.totalEnemiesKilled}명");
            Debug.Log($"  - 보스 처치: {meta.totalBossesDefeated}명");
        }

        // ===== 세이브 파일 손상 처리 =====

        /// <summary>
        /// 손상된 세이브 파일을 처리합니다
        /// </summary>
        private void HandleCorruptedSaveFile(string filePath, int slotIndex, SaveCorruptionType corruptionType)
        {
            Debug.LogWarning($"=== 세이브 파일 손상 감지 ({corruptionType}) ===");
            Debug.LogWarning($"파일: {filePath}");

            // 손상된 파일 이동
            MoveCorruptedFile(filePath, corruptionType);

            // 백업 파일 로드 시도
            bool backupLoaded = TryLoadBackup(slotIndex);

            if (backupLoaded)
            {
                Debug.Log("백업 파일에서 복구 성공!");
            }
            else
            {
                Debug.LogError("백업 파일도 손상되었거나 없습니다. 새 게임을 시작해야 합니다.");
            }
        }

        /// <summary>
        /// 손상된 메타 데이터 파일을 처리합니다
        /// </summary>
        private void HandleCorruptedMetaFile(string filePath, SaveCorruptionType corruptionType)
        {
            Debug.LogWarning($"=== 메타 데이터 파일 손상 감지 ({corruptionType}) ===");
            Debug.LogWarning($"파일: {filePath}");

            // 손상된 파일 이동
            MoveCorruptedFile(filePath, corruptionType);

            // 백업 파일 로드 시도
            bool backupLoaded = TryLoadMetaBackup();

            if (backupLoaded)
            {
                Debug.Log("메타 데이터 백업 파일에서 복구 성공!");
            }
            else
            {
                Debug.LogWarning("메타 데이터 백업 파일도 손상되었거나 없습니다. 새 메타 데이터를 생성합니다.");

                // 새 메타 데이터 생성
                if (currentSaveData == null)
                {
                    currentSaveData = new SaveData();
                }
                currentSaveData.metaData = new MetaSaveData();
            }
        }

        /// <summary>
        /// 백업 파일을 로드합니다
        /// </summary>
        private bool TryLoadBackup(int slotIndex)
        {
            string backupPath = GetBackupFilePath(GetSaveFilePath(slotIndex));

            if (!File.Exists(backupPath))
            {
                Debug.LogWarning($"백업 파일이 없습니다: {backupPath}");
                return false;
            }

            Debug.Log($"백업 파일 로드 시도: {backupPath}");

            try
            {
                // 파일 읽기
                string json = File.ReadAllText(backupPath);

                // 난독화 해제 (옵션)
                if (useEncryption)
                {
                    json = DeobfuscateString(json);
                }

                // JSON 역직렬화
                SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

                if (loadedData == null || !loadedData.Validate())
                {
                    Debug.LogError("백업 파일도 손상되었습니다");
                    return false;
                }

                currentSaveData = loadedData;

                Debug.Log($"백업 파일 로드 성공: {backupPath}");

                // 메타 데이터 적용
                ApplyMetaData();

                // 백업을 메인 파일로 복원
                string mainPath = GetSaveFilePath(slotIndex);
                File.Copy(backupPath, mainPath, true);
                Debug.Log("백업 파일을 메인 파일로 복원했습니다");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"백업 파일 로드 실패: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 메타 데이터 백업 파일을 로드합니다
        /// </summary>
        private bool TryLoadMetaBackup()
        {
            string backupPath = GetBackupFilePath(GetMetaSaveFilePath());

            if (!File.Exists(backupPath))
            {
                Debug.LogWarning($"메타 데이터 백업 파일이 없습니다: {backupPath}");
                return false;
            }

            Debug.Log($"메타 데이터 백업 파일 로드 시도: {backupPath}");

            try
            {
                // 파일 읽기
                string json = File.ReadAllText(backupPath);

                // 난독화 해제 (옵션)
                if (useEncryption)
                {
                    json = DeobfuscateString(json);
                }

                // JSON 역직렬화
                MetaSaveData loadedMetaData = JsonUtility.FromJson<MetaSaveData>(json);

                if (loadedMetaData == null || !loadedMetaData.Validate())
                {
                    Debug.LogError("메타 데이터 백업 파일도 손상되었습니다");
                    return false;
                }

                if (currentSaveData == null)
                {
                    currentSaveData = new SaveData();
                }

                currentSaveData.metaData = loadedMetaData;

                Debug.Log($"메타 데이터 백업 파일 로드 성공: {backupPath}");

                // 메타 데이터 적용
                ApplyMetaData();

                // 백업을 메인 파일로 복원
                string mainPath = GetMetaSaveFilePath();
                File.Copy(backupPath, mainPath, true);
                Debug.Log("메타 데이터 백업 파일을 메인 파일로 복원했습니다");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"메타 데이터 백업 파일 로드 실패: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 백업 파일을 생성합니다
        /// </summary>
        private void CreateBackupFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                string backupPath = GetBackupFilePath(filePath);
                File.Copy(filePath, backupPath, true);
                Debug.Log($"백업 파일 생성: {backupPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"백업 파일 생성 실패: {e.Message}");
            }
        }

        /// <summary>
        /// 백업 파일 경로를 반환합니다
        /// </summary>
        private string GetBackupFilePath(string originalPath)
        {
            return originalPath + ".bak";
        }

        /// <summary>
        /// 손상된 파일을 별도 폴더로 이동합니다
        /// </summary>
        private void MoveCorruptedFile(string filePath, SaveCorruptionType corruptionType)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                // 손상된 파일 보관 폴더 생성
                string corruptedDir = Path.Combine(Application.persistentDataPath, "CorruptedSaves");
                if (!Directory.Exists(corruptedDir))
                {
                    Directory.CreateDirectory(corruptedDir);
                }

                // 타임스탬프 추가
                string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                string corruptedFileName = $"{fileName}_{corruptionType}_{timestamp}{extension}.corrupted";
                string corruptedPath = Path.Combine(corruptedDir, corruptedFileName);

                // 파일 이동
                File.Move(filePath, corruptedPath);

                Debug.LogWarning($"손상된 파일 이동 완료: {corruptedPath}");
                Debug.LogWarning("디버깅을 위해 손상된 파일을 보관했습니다.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"손상된 파일 이동 실패: {e.Message}");
            }
        }

        // ===== 세이브 파일 마이그레이션 =====

        /// <summary>
        /// 세이브 데이터를 최신 버전으로 마이그레이션합니다
        /// </summary>
        private void MigrateSaveData(SaveData saveData)
        {
            if (saveData == null)
            {
                Debug.LogError("MigrateSaveData: saveData가 null입니다");
                return;
            }

            Debug.Log($"=== 세이브 파일 마이그레이션 시작 ===");
            Debug.Log($"현재 버전: {saveData.version}");
            Debug.Log($"목표 버전: {SaveData.CURRENT_VERSION}");

            // Phase 1 -> Phase 2
            if (saveData.IsOlderThan(SaveData.PHASE2_VERSION))
            {
                Debug.Log("Phase 1 -> Phase 2 마이그레이션 실행");
                MigratePhase1ToPhase2(saveData);
            }

            // Phase 2 -> Phase 3
            if (saveData.IsOlderThan(SaveData.CURRENT_VERSION))
            {
                Debug.Log("Phase 2 -> Phase 3 마이그레이션 실행");
                MigratePhase2ToPhase3(saveData);
            }

            // 버전 업데이트
            saveData.UpdateVersion();

            Debug.Log($"=== 마이그레이션 완료: v{saveData.version} ===");
        }

        /// <summary>
        /// Phase 1 -> Phase 2 마이그레이션
        /// </summary>
        private void MigratePhase1ToPhase2(SaveData saveData)
        {
            Debug.Log("[Migration] Phase 1 -> Phase 2");

            // Phase 2에서 추가된 필드 초기화
            if (saveData.currentRun != null)
            {
                // RunData에 Phase 2 필드가 없으면 기본값 설정
                if (saveData.currentRun.visitedNodeIds == null)
                {
                    saveData.currentRun.visitedNodeIds = new System.Collections.Generic.List<string>();
                }

                if (saveData.currentRun.relicIds == null)
                {
                    saveData.currentRun.relicIds = new System.Collections.Generic.List<string>();
                }
            }

            // MetaData에 Phase 2 필드가 없으면 기본값 설정
            if (saveData.metaData != null)
            {
                if (saveData.metaData.unlockedUpgrades == null)
                {
                    saveData.metaData.unlockedUpgrades = new System.Collections.Generic.List<UpgradeUnlockData>();
                }
            }

            Debug.Log("[Migration] Phase 1 -> Phase 2 완료");
        }

        /// <summary>
        /// Phase 2 -> Phase 3 마이그레이션
        /// </summary>
        private void MigratePhase2ToPhase3(SaveData saveData)
        {
            Debug.Log("[Migration] Phase 2 -> Phase 3");

            // RunData 마이그레이션
            if (saveData.currentRun != null)
            {
                // 분파 정보 초기화
                if (string.IsNullOrEmpty(saveData.currentRun.factionId))
                {
                    saveData.currentRun.factionId = "faction_hwasan"; // 기본값: 화산파
                    Debug.Log("[Migration] 기본 분파 설정: 화산파");
                }

                // 내공 경지 진행도 초기화
                if (saveData.currentRun.innerEnergyProgress == null)
                {
                    saveData.currentRun.innerEnergyProgress = new InnerEnergyProgress();
                    Debug.Log("[Migration] 내공 경지 진행도 초기화");
                }

                // 무기술 경지 진행도 초기화
                if (saveData.currentRun.weaponMasteryProgress == null)
                {
                    saveData.currentRun.weaponMasteryProgress = new System.Collections.Generic.List<WeaponMasteryProgress>();
                    Debug.Log("[Migration] 무기술 경지 진행도 초기화");
                }

                // 지역 정보 초기화
                if (string.IsNullOrEmpty(saveData.currentRun.currentRegionId))
                {
                    saveData.currentRun.currentRegionId = "region_1"; // 기본값: 강남
                    Debug.Log("[Migration] 기본 지역 설정: 강남");
                }
            }

            // MetaData 마이그레이션
            if (saveData.metaData != null)
            {
                // 해금된 분파 초기화
                if (saveData.metaData.unlockedFactions == null)
                {
                    saveData.metaData.unlockedFactions = new System.Collections.Generic.List<string>();
                    saveData.metaData.unlockedFactions.Add("faction_hwasan"); // 기본적으로 화산파 해금
                    Debug.Log("[Migration] 해금된 분파 초기화: 화산파");
                }

                // 해금된 난이도 초기화
                if (saveData.metaData.unlockedDifficulties == null)
                {
                    saveData.metaData.unlockedDifficulties = new System.Collections.Generic.List<int>();
                    saveData.metaData.unlockedDifficulties.Add(0); // 기본 난이도 해금
                    Debug.Log("[Migration] 해금된 난이도 초기화: 입문");
                }

                // 업적 달성 내역 초기화
                if (saveData.metaData.unlockedAchievements == null)
                {
                    saveData.metaData.unlockedAchievements = new System.Collections.Generic.List<string>();
                    Debug.Log("[Migration] 업적 달성 내역 초기화");
                }

                // 도감 수집 내역 초기화
                if (saveData.metaData.unlockedCodexEntries == null)
                {
                    saveData.metaData.unlockedCodexEntries = new System.Collections.Generic.List<string>();
                    Debug.Log("[Migration] 도감 수집 내역 초기화");
                }
            }

            Debug.Log("[Migration] Phase 2 -> Phase 3 완료");
        }
    }
}
