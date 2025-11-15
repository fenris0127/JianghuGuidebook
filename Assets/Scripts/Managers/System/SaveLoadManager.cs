using UnityEngine;
using System.IO;

// 현재 진행 중인 한 판(Run)의 데이터를 기기에 파일로 저장하고 불러옵니다.
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    private string saveFilePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Application.persistentDataPath는 각 플랫폼에 맞는 안전한 저장 경로를 제공합니다.
            saveFilePath = Path.Combine(Application.persistentDataPath, "runData.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveRun(RunData data)
    {
        string json = JsonUtility.ToJson(data, true); // true 옵션으로 가독성 좋게 저장
        File.WriteAllText(saveFilePath, json);
        Debug.Log("게임 진행 상황 저장 완료: " + saveFilePath);
    }

    public RunData LoadRun()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Debug.Log("저장된 게임 불러오기 완료.");
            return JsonUtility.FromJson<RunData>(json);
        }

        return null;
    }
    
    public bool DoesSaveFileExist() => File.Exists(saveFilePath);
    
    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("저장 파일 삭제 완료.");
        }
    }
}