# Unity_GetComponentAttributes_and_Extensions
 - GetComponent~() 메소드들을 애트리뷰트로 제공
 - GetComponent~()와 AddComponent~()를 결합하여, 게임오브젝트에 원하는 컴포넌트가 존재하지 않을 경우 자동적으로 추가해주는 기능 제공
 
<br>

## 1. GetComponent Attributes
 ### [1] 설명
  - GetComponent() GetComponentInChildren(), GetComponentInParent(), GetComponents(), GetComponentsInChildren(), GetComponentsInParent() 메소드를 애트리뷰트화하였습니다.
  - 따로 GetComponent~() 메소드를 호출하여 멤버에 할당할 필요 없이, 멤버 선언부에 원하는 애트리뷰트를 사용함으로써 해당 기능이 동작합니다.
  - 각 멤버에 대해 GetComponent 기능이 동작하는 타이밍은 Awake, Start 중 하나를 선택하여 애트리뷰트 파라미터로 명시할 수 있습니다.
    <br>(명시하지 않을 경우, 기본적으로 Awake)
  - 해당 애트리뷰트를 인식하고 각각의 기능을 실행시키는 주체는 싱글톤 클래스로 구현하였습니다.
  - 기본적인 GetComponent~() 메소드에 AddComponent() 메소드를 결합하여, 원하는 컴포넌트가 존재하지 않을 경우 자동적으로 컴포넌트를 추가하고 멤버에 할당하게 해주는 GetOrAddComponent, GetOrAddInChildren, GetOrAddInParent 애트리뷰트를 제공합니다.
  
 ### [2] 주의사항
  - public 멤버들에 대해서만 동작합니다.
  - 필드와 프로퍼티에 대해서만 동작합니다.
  - Component 클래스를 상속하는 타입들에 대해서만 동작합니다.
  - GetComponent, GetComponentInChildren, GetComponentInParent, GetOrAddComponent, GetOrAddComponentInChildren, GetOrAddComponentInParent 애트리뷰트는 Array나 List, Dictionary 등 컨테이너 또는 제네릭 타입의 멤버에 대해 동작하지 않습니다.
  - GetComponents, GetComponentsInChildren, GetComponentsInParent 애트리뷰트는 요소의 타입이 Component를 상속하는 경우의 Array 또는 List 타입 멤버에 대해 동작합니다.
  - GetComponentController 클래스가 씬 내에 활성화된 컴포넌트의 형태로 존재하는 경우에만 모든 기능이 올바르게 동작 합니다.
  
 ### [3] 애트리뷰트 종류
  - [GetComponent]
  - [GetComponentInChildren]
  - [GetComponentInParent]
  
  <br>
  
  - [GetComponents]
  - [GetComponentsInChildren]
  - [GetComponentsInParent]
  
  <br>
  
  - [GetOrAddComponent]
  - [GetOrAddComponentInChildren]
  - [GetOrAddComponentInParent]
  
 ### [3] 사용 예시
  - .
  
<br>

## 2. GetOrAddComponent Extensions
 ### [1] 설명
  - .
 
 ### [2] 메소드 종류
  - .
  
 ### [3] 사용 예시
  - .
