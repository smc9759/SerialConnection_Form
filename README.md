# Async Serial Communication 

## Language
1. C#
2. GUI - Winform

## Function
1. Check Available Ports
2. 6 Ports Supported 

## Sync <-> Async Method
1. Sync
### Data accumulates Over time

Imagine a Mcdonald with one employee. He can manage One Order at a time. In One Day the customers will complain about their late orders.  
In Sync communication Test, similiar problem happened.
Program occupied memory space more and more. So CPU Usage scored upto 100%.

2. Async
### No Accumulation, nor CPU 100%  

## Developed for SQLiteDB Project that uses async method to save data.  

### Below Information source : ChatGPT

## Queue

### What is **`ConcurrentQueue<string>`?**

`ConcurrentQueue<T>`는 .NET 프레임워크에서 제공하는 스레드 안전(Thread-Safe) FIFO(First-In-First-Out) 큐입니다. 이 큐는 멀티스레드 환경에서 사용하도록 설계되었습니다. 당신의 경우, `ConcurrentQueue<string>`은 문자열의 큐를 스레드 안전하게 처리하는 데 사용됩니다.

### **주요 작업:**
### 

- **Enqueue**:
    - 큐의 끝에 아이템을 추가합니다.
    
    ```csharp
    queueForS001.Enqueue("Some data");
    ```
    
- **TryDequeue**:
    - 큐의 시작에서 객체를 제거하고 반환합니다. 큐에서 객체를 성공적으로 제거했는지 여부를 나타내는 불리언 값을 반환합니다.
    
    ```csharp
    if (queueForS001.TryDequeue(out string data))
    {
        // 데이터 처리
    }
    ```
    

## 시리얼 포트 관리

`MonitorSerialPortsAsync` 메서드는 다음과 같은 주요 작업을 수행합니다:

1. **사용 가능한 시리얼 포트 감지**:
    - 시스템에서 사용 가능한 시리얼 포트를 주기적으로 확인합니다.
2. **활성 포트 관리**:
    - 새로 감지된 포트를 열고, 현재 열려 있지 않은 포트를 엽니다.
    - 더 이상 사용 가능한 포트가 아니거나 연결이 끊어진 포트를 닫습니다.
3. **재연결 처리**:
    - 이전에 닫혔거나 연결이 끊어진 포트를 다시 열려고 시도합니다.

## 비동기 키워드

### **1.** `async`는 비동기 메서드를 정의하는 데 사용하는 키워드다.

### **2.** `Task`는 진행 중인 작업을 나타내는 타입이다.

### **3. `async`와 `Task`는 어떻게 함께 작동하나요?**

- **비동기 메서드**: 메서드에 `async` 키워드를 표시하면, 이 메서드가 비동기 작업을 포함한다는 것을 컴파일러에 알립니다. 이 메서드는 `Task`(또는 값을 반환하는 경우 `Task<T>`)를 반환합니다.
- **Await 키워드**: `async` 메서드 내부에서 `await` 키워드를 사용하여 다른 비동기 메서드를 호출합니다. `await`는 비동기 작업이 완료될 때까지 `async` 메서드의 실행을 일시 중지하지만, 메인 스레드를 차단하지는 않습니다.

<aside>
💡

일반적으로 모든 C# 애플리케이션은 하나의 메인 스레드로 시작하여 프로그램의 초기화, 사용자 인터페이스(UI) 업데이트, 이벤트 처리 등을 처리합니다. 

</aside>

## 비동기 Method를 추가하면 메모리가 더 필요한가?

이론상 await가 걸린 동안 메모리 공간이 더 필요하다. 실제로는 최적화가 잘 되어있어서 시스템 자원 사용에 차이가 없다. 

### 

# 데이터 처리

큐에서 하나씩 빼가지고 DB에 넣는다
