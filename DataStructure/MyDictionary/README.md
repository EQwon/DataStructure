# MyDictionary
 
## 생성자 (Constructor)
  - MyDictionary()
  - MyDictionary(int capacity)
  - MyDictionary(IMyEqualityComparer<TKey> comparer)
  - MyDictionary(int capacity, IMyEqualityComparer<TKey> comparer)
 
## 속성 (Property)
  - item[TKey key]
    - 흔히 사용하는 dict["Key"] = "Value"; 의 형태
  - Count
    - dictionary의 길이를 return

## 메소드 (Method)
  - void Add(TKey key, TValue value)
    - key에 대응하는 value를 추가. 중복이라면 추가하지 않음
    
  - void Clear()
    - 메모리의 크기는 변화시키지 않고 초기화, O(n)
    
  - bool ContainsKey(TKey key)
    - 해당하는 key가 존재하는지 탐색. O(1)
    
  - bool ContainsValue(TValue value)
    - 해당하는 value가 존재하는지 탐색, 모든 entry를 순회하기 때문에 O(n)
    
  - bool Remove(TKey key)
    - key가 존재하지 않으면 false
    - key가 존재하면 해당 부분을 초기화 시키고 빈공간임을 등록. O(1)
    
------------------

# HashHelper
  - 해쉬테이블을 만들기 위해 적절한 소수를 찾아주는 클래스

------------------

# IMyEqualityComparer, MyEqualityComparer

## IMyEqualityComparer
  - 비교용 인터페이스
  
## MyEqualityComparer
  - Default
    - 최상위 클래스인 object로 변환시켜서 비교
