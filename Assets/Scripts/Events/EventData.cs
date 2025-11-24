using System.Collections.Generic;

namespace JianghuGuidebook.Events
{
    /// <summary>
    /// 이벤트 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class EventData
    {
        public string id;                   // 이벤트 고유 ID
        public string title;                // 이벤트 제목
        public string description;          // 이벤트 설명 (상황 묘사)
        public List<EventChoice> choices;   // 선택지 리스트

        public EventData()
        {
            choices = new List<EventChoice>();
        }

        public EventData(string id, string title, string description)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            choices = new List<EventChoice>();
        }

        /// <summary>
        /// 선택지 추가
        /// </summary>
        public void AddChoice(EventChoice choice)
        {
            choices.Add(choice);
        }

        /// <summary>
        /// 데이터 유효성 검증
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(id))
            {
                UnityEngine.Debug.LogError($"EventData: ID가 비어있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(title))
            {
                UnityEngine.Debug.LogError($"EventData [{id}]: 제목이 비어있습니다");
                return false;
            }

            if (string.IsNullOrEmpty(description))
            {
                UnityEngine.Debug.LogError($"EventData [{id}]: 설명이 비어있습니다");
                return false;
            }

            if (choices == null || choices.Count == 0)
            {
                UnityEngine.Debug.LogError($"EventData [{id}]: 선택지가 없습니다");
                return false;
            }

            // 각 선택지 검증
            for (int i = 0; i < choices.Count; i++)
            {
                EventChoice choice = choices[i];
                if (string.IsNullOrEmpty(choice.text))
                {
                    UnityEngine.Debug.LogError($"EventData [{id}]: 선택지 {i}의 텍스트가 비어있습니다");
                    return false;
                }

                if (choice.outcomes == null || choice.outcomes.Count == 0)
                {
                    UnityEngine.Debug.LogError($"EventData [{id}]: 선택지 {i}에 결과가 없습니다");
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"{title} ({choices.Count}개 선택지)";
        }
    }

    /// <summary>
    /// 이벤트 데이터베이스 (JSON 로딩용)
    /// </summary>
    [System.Serializable]
    public class EventDatabase
    {
        public List<EventData> events;

        public EventDatabase()
        {
            events = new List<EventData>();
        }
    }
}
