namespace JianghuGuidebook.Save
{
    /// <summary>
    /// 전체 게임 세이브 데이터
    /// 현재 런 데이터와 메타 진행 데이터를 포함합니다
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        // 버전 상수
        public const string CURRENT_VERSION = "3.0.0";          // Phase 3 버전
        public const string PHASE2_VERSION = "2.0.0";           // Phase 2 버전
        public const string PHASE1_VERSION = "1.0.0";           // Phase 1 버전

        public string version;              // 세이브 파일 버전
        public long saveTimestamp;          // 저장 시간
        public string checksum;             // 무결성 검증용 체크섬

        public RunData currentRun;          // 현재 런 데이터 (null이면 진행 중인 런 없음)
        public MetaSaveData metaData;       // 메타 진행 데이터

        public SaveData()
        {
            version = CURRENT_VERSION;
            saveTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            checksum = "";
            currentRun = null;
            metaData = new MetaSaveData();
        }

        /// <summary>
        /// 체크섬을 생성합니다
        /// </summary>
        public string GenerateChecksum()
        {
            // 간단한 체크섬 생성 (실제로는 더 강력한 해시 알고리즘 사용 권장)
            string data = version + saveTimestamp.ToString();

            if (currentRun != null)
            {
                data += currentRun.runId + currentRun.currentHealth.ToString() + currentRun.currentGold.ToString();
            }

            data += metaData.currentEssence.ToString() + metaData.totalEssence.ToString();

            // 간단한 해시
            int hash = data.GetHashCode();
            return hash.ToString("X8");
        }

        /// <summary>
        /// 체크섬을 업데이트합니다
        /// </summary>
        public void UpdateChecksum()
        {
            checksum = GenerateChecksum();
        }

        /// <summary>
        /// 체크섬을 검증합니다
        /// </summary>
        public bool ValidateChecksum()
        {
            if (string.IsNullOrEmpty(checksum))
            {
                UnityEngine.Debug.LogWarning("SaveData: 체크섬이 없습니다");
                return false;
            }

            string expectedChecksum = GenerateChecksum();
            bool isValid = checksum == expectedChecksum;

            if (!isValid)
            {
                UnityEngine.Debug.LogError($"SaveData: 체크섬 불일치 (예상: {expectedChecksum}, 실제: {checksum})");
            }

            return isValid;
        }

        /// <summary>
        /// 저장 시간을 업데이트합니다
        /// </summary>
        public void UpdateSaveTimestamp()
        {
            saveTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        /// <summary>
        /// 진행 중인 런이 있는지 확인합니다
        /// </summary>
        public bool HasActiveRun()
        {
            return currentRun != null;
        }

        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(version))
            {
                UnityEngine.Debug.LogError("SaveData: 버전 정보가 없습니다");
                return false;
            }

            // 메타 데이터 검증
            if (metaData == null)
            {
                UnityEngine.Debug.LogError("SaveData: metaData가 null입니다");
                return false;
            }

            if (!metaData.Validate())
            {
                return false;
            }

            // 런 데이터 검증 (있을 경우)
            if (currentRun != null && !currentRun.Validate())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 마이그레이션이 필요한지 확인합니다
        /// </summary>
        public bool NeedsMigration()
        {
            return version != CURRENT_VERSION;
        }

        /// <summary>
        /// 버전을 최신으로 업데이트합니다
        /// </summary>
        public void UpdateVersion()
        {
            version = CURRENT_VERSION;
        }

        /// <summary>
        /// 특정 버전인지 확인합니다
        /// </summary>
        public bool IsVersion(string targetVersion)
        {
            return version == targetVersion;
        }

        /// <summary>
        /// 버전 비교 (현재 버전이 목표 버전보다 낮으면 true)
        /// </summary>
        public bool IsOlderThan(string targetVersion)
        {
            return CompareVersions(version, targetVersion) < 0;
        }

        /// <summary>
        /// 버전 문자열 비교 (v1 < v2: -1, v1 == v2: 0, v1 > v2: 1)
        /// </summary>
        private int CompareVersions(string v1, string v2)
        {
            if (string.IsNullOrEmpty(v1) || string.IsNullOrEmpty(v2))
            {
                UnityEngine.Debug.LogWarning($"SaveData: 유효하지 않은 버전 비교 (v1: {v1}, v2: {v2})");
                return 0;
            }

            string[] parts1 = v1.Split('.');
            string[] parts2 = v2.Split('.');

            for (int i = 0; i < System.Math.Max(parts1.Length, parts2.Length); i++)
            {
                int num1 = i < parts1.Length && int.TryParse(parts1[i], out int n1) ? n1 : 0;
                int num2 = i < parts2.Length && int.TryParse(parts2[i], out int n2) ? n2 : 0;

                if (num1 < num2) return -1;
                if (num1 > num2) return 1;
            }

            return 0;
        }

        public override string ToString()
        {
            string runInfo = HasActiveRun() ? currentRun.ToString() : "진행 중인 런 없음";
            return $"SaveData v{version}: {runInfo}, Meta: {metaData}";
        }
    }
}
