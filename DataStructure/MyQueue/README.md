# MyQueue
 
## 생성자 (Constructor)
  - MyQueue()
  - MyQueue(int capacity)
 
## 속성 (Property)
  - Count
    - queue의 길이를 return

## 메소드 (Method)    
  - void Clear()
    - 쓸떼없이 메모리와 연산을 낭비를 하지 않기 위해 원소가 있는 곳만 청소
    - Array.Clear를 이용
    
  - bool ContainsKey(T item)
    - 해당하는 item이 존재하는지 탐색. O(n)
    - T에 해당하는 EqualityComparer를 정의한 후 Equals()를 이용해 비교

  - void CopyTo(T[] array, int arryIndex)
    - 현재 queue의 내용물을 array의 arrayIndex 위치에서부터 복사해옴
    - Array.Copy를 이용
    
  - void Enqueue(T item)
    - item을 queue의 후미에 추가
    - 만약 준비된 사이즈를 벗어나려한다면 growFactor(=2)를 곱한 크기만큼 내부 배열 크기 증가
      - SetCapacity() 함수 이용
    - _tail의 위치가 item을 추가 후 _tail을 한 칸 오른쪽으로 이동
    
  - T Dequeue()
    - _head 위치에 있는 원소를 return
    - _head는 한 칸 오른쪽으로 이동하고 _size도 1 감소

  - T Peek()
    - _head 위치의 원소를 return

  - T[] ToArray()
    - _size 만큼의 배열을 준비한 뒤 복사
    - _head와 _tail의 위치에 따라서 적절하게 복사
    
# MyQueueEnumerator
  - MyQueue 전용 Enumerator