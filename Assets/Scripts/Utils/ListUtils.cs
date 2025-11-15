using System;
using System.Collections.Generic;

// 모든 리스트(IList<T>)에서 공통적으로 사용할 수 있는 유용한 기능들을 모아놓은 정적 클래스입니다.
public static class ListUtils
{
    // System.Random은 한 번만 생성하여 여러 번 재사용하는 것이 성능에 유리합니다.
    // 'static'으로 선언하여 프로그램 전체에서 단 하나의 인스턴스만 유지되도록 합니다.
    private static Random rng = new Random();

    // 'this' 키워드를 사용하여 모든 IList<T> 타입(List<T>, T[] 등)에 .Shuffle() 메서드를 추가합니다. (확장 메서드)
    // 현재 리스트의 순서를 무작위로 섞습니다.
    // 피셔-예이츠 셔플(Fisher-Yates Shuffle) 알고리즘을 사용합니다.
    public static void Shuffle<T>(this IList<T> list)
    {
        // 리스트의 마지막 요소부터 처음 요소까지 역순으로 순회합니다.
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            
            // list[k]와 list[n]의 위치를 서로 바꿉니다.
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
            return default(T); // 리스트가 비어있으면 null 또는 0 등을 반환
            
        return list[rng.Next(list.Count)];
    }
}