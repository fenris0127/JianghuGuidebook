using UnityEngine;

namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 게임의 메인 매니저 - 싱글톤 패턴으로 구현
    /// 전체 게임 루프와 상태를 관리합니다
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("GameManager 초기화 완료");
        }

        public void StartNewRun()
        {
            Debug.Log("새로운 게임 시작");
        }

        public void QuitGame()
        {
            Debug.Log("게임 종료");
            Application.Quit();
        }
    }
}
