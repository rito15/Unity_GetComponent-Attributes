# Unity_GetComponentAttributes
 - ```GetComponent()``` 메소드들을 애트리뷰트로 제공합니다.
 - ```GetComponent()```와 ```AddComponent()``` 메소드를 결합하여,
   <br>게임오브젝트에  컴포넌트가 존재하지 않을 경우 자동적으로 추가해주는 ```[GetOrAddComponent]``` 애트리뷰트를 제공합니다.
 
<br>

## GetComponent Attributes
 ### [1] 설명
  - 스크립팅의 편의성을 위해
  <br>```GetComponent()```, ```GetComponentInChildren()```, ```GetComponentInParent()```, ```GetComponents()```, ```GetComponentsInChildren()```, ```GetComponentsInParent()``` 메소드를 애트리뷰트화하였습니다.
  - 따로 ```GetComponent()``` 종류의 메소드를 호출하여 멤버 변수에  할당할 필요 없이, 필드 또는 프로퍼티에 원하는 애트리뷰트를 사용함으로써 자동으로 해당 컴포넌트를 할당하도록 합니다.
  - 각 멤버에 대해 GetComponent 기능이 동작하는 타이밍을
    <br>```EventFlow.Awake```, ```EventFlow.Start``` 중 하나를 선택하여 애트리뷰트 파라미터로 명시할 수 있습니다.
  - 해당 애트리뷰트를 인식하고 각각의 기능을 실행시키는 주체는 싱글톤 클래스로 구현하였습니다.
  - 기본적인 ```GetComponent~()``` 메소드와 ```AddComponent()``` 메소드의 기능을 결합하여,
  <br>원하는 컴포넌트가 존재하지 않을 경우 자동적으로 컴포넌트를 추가하고 멤버에 할당하게 해주는
  <br>```[GetOrAddComponent]```, ```[GetOrAddInChildren]```, ```[GetOrAddInParent]``` 애트리뷰트를 제공합니다.
  
  <br>
  
 ### [2] 주의사항
  - public 멤버들에 대해서만 동작합니다.
  - 필드와 프로퍼티에 대해서만 동작합니다.
  - ```Component``` 클래스를 상속하는 타입들에 대해서만 동작합니다.
  - ```[GetComponent]```, ```[GetComponentInChildren]```, ```[GetComponentInParent]```, ```[GetOrAddComponent]```, ```[GetOrAddComponentInChildren]```, ```[GetOrAddComponentInParent]``` 애트리뷰트는 ```Array```나 ```List```, ```Dictionary``` 등 컨테이너 또는 제네릭 타입의 멤버에 대해 동작하지 않습니다.
  - ```[GetComponents]```, ```[GetComponentsInChildren]```, ```[GetComponentsInParent]``` 애트리뷰트는
  <br>요소의 타입이 ```Component```를 상속하는 경우의 ```Array``` 또는 ```List``` 타입 멤버에 대해 동작합니다.
  - ```GetComponentController``` 클래스가 씬 내에 활성화된 컴포넌트의 형태로 존재하는 경우에만 모든 기능이 올바르게 동작 합니다.
  
  <br>
  
 ### [3] 애트리뷰트 종류
  *(공통 매개변수 : ```EventFlow```(Enum) - ```EventFlow.Awake```(기본) 또는 ```EventFlow.Start``` 선택)*
 
  - ```[GetComponent]```
   <br>: 게임오브젝트 내에서 해당 타입의 컴포넌트를 찾아 멤버 변수에 초기화
   <br>
   
  - ```[GetComponentInChildren]```
   <br>: 자기 또는 자식 게임오브젝트들 내에서 해당 타입의 컴포넌트를 찾아 멤버 변수에 초기화
   <br>
   
  - ```[GetComponentInParent]```
   <br>: 자기 또는 부모 게임오브젝트들 내에서 해당 타입의 컴포넌트를 찾아 멤버 변수에 초기화
  
  <br>
  
  - ```[GetComponents]```
   <br>: 게임오브젝트 내에서 해당 타입의 컴포넌트들을 모두 찾아 멤버 변수에 초기화
   <br>* *Array, List에 대해서 동작*
   <br>
   
  - ```[GetComponentsInChildren]```
   <br>: 자기 또는 자식 게임오브젝트들 내에서 해당 타입의 컴포넌트들을 모두 찾아 멤버 변수에 초기화
   <br>* *Array, List에 대해서 동작*
   <br>
   
  - ```[GetComponentsInParent]```
   <br>: 자기 또는 부모 게임오브젝트들 내에서 해당 타입의 컴포넌트들을 모두 찾아 멤버 변수에 초기화
   <br>* *Array, List에 대해서 동작*
  
  <br>
  
  - ```[GetOrAddComponent]```
   <br>: 게임오브젝트 내에서 해당 타입의 컴포넌트를 찾아 멤버 변수에 초기화
   <br>* 해당 컴포넌트가 존재하지 않을 경우, 오브젝트 내에 생성 및 추가하여 멤버 변수에 초기화
   <br>
   
  - ```[GetOrAddComponentInChildren]```
   <br>: 자기 또는 자식 게임오브젝트들 내에서 해당 타입의 컴포넌트를 찾아 멤버 변수에 초기화
   <br><br>* **해당 컴포넌트가 존재하지 않을 경우**
   >> (1) 매개변수로 지정한 이름의 자식 게임오브젝트 탐색
   <br> (2) 해당 이름의 자식 게임오브젝트가 존재할 경우,
   <br>     해당 게임오브젝트에 컴포넌트를 생성/추가하여 멤버 변수에 초기화
   <br> (3) 해당 이름의 자식 게임오브젝트가 존재하지 않을 경우, 
   <br>     해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 생성 및 추가하여 멤버 변수에 초기화
   <br>
   
  - ```[GetOrAddComponentInParent]```
   <br>: 자기 또는 부모 게임오브젝트들 내에서 해당 타입의 컴포넌트를 찾아 멤버 변수에 초기화
   <br><br>* **해당 컴포넌트가 존재하지 않을 경우**
   >> (1) 매개변수로 지정한 이름의 부모 게임오브젝트 탐색
   <br> (2) 해당 이름의 부모 게임오브젝트가 존재할 경우, 
   <br>     해당 게임오브젝트에 컴포넌트를 생성 및 추가하여 멤버 변수에 초기화
   <br> (3) 해당 이름의 부모 게임오브젝트가 존재하지 않을 경우 아무런 동작을 하지 않습니다.
  
  <br>
  
 ### [4] 사용 예시
  #### ```GetComponentController``` 싱글톤 컴포넌트 할당
   ![image](https://user-images.githubusercontent.com/42164422/77653822-9225c780-6fb3-11ea-8b5d-b98fcf3d7ac7.png)
 
  #### (1) 기존의 코드 사용(```GetComponent()```, ```AddComponent()```)
   - 소스 코드<br>
    ![image](https://user-images.githubusercontent.com/42164422/77655161-66a3dc80-6fb5-11ea-97f1-d7d2474c0baf.png)
  
   - 실행 전<br>
    ![image](https://user-images.githubusercontent.com/42164422/77662476-3feaa380-6fbf-11ea-8db4-743b8a78d995.png)
  
   - 실행 후<br>
    ![image](https://user-images.githubusercontent.com/42164422/77662543-58f35480-6fbf-11ea-8819-9dd66cbe2af3.png)

  #### (2) 애트리뷰트 사용
   - 소스 코드<br>
    ![image](https://user-images.githubusercontent.com/42164422/77661752-65c37880-6fbe-11ea-999c-42a721208843.png)
  
   - 실행 전<br>
    ![image](https://user-images.githubusercontent.com/42164422/77661775-6d831d00-6fbe-11ea-9da7-5ce73119947e.png)
  
   - 실행 후<br>
    ![image](https://user-images.githubusercontent.com/42164422/77661848-825fb080-6fbe-11ea-9d13-570e56207605.png)
