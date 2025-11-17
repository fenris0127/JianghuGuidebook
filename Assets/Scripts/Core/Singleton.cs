using UnityEngine;

namespace GangHoBiGeup.Core
{
    /// <summary>
    /// Unity MonoBehaviour를 위한 Singleton 추상 클래스
    /// 씬 전환 시에도 파괴되지 않습니다 (DontDestroyOnLoad).
    /// </summary>
    /// <typeparam name="T">Singleton으로 만들 클래스 타입</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        Debug.LogWarning($"{typeof(T).Name} Singleton 인스턴스가 씬에 없습니다.");
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
                OnAwake();
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"{typeof(T).Name} Singleton이 이미 존재합니다. 중복된 인스턴스를 제거합니다.");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Awake에서 호출되는 초기화 메서드입니다.
        /// 자식 클래스에서 Awake를 오버라이드하는 대신 이 메서드를 사용하세요.
        /// </summary>
        protected virtual void OnAwake() { }
    }
}
