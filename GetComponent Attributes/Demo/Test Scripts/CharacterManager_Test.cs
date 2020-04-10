using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito.Attributes;

/// <summary> 
/// <para/> GetComponent Attributes Demo
/// </summary>
public class CharacterManager_Test : MonoBehaviour
{
    /* ******************************************************************************** *
     *                                    참고사항                                      *
     * ******************************************************************************** */

    /* 공통 파라미터 AllowOverwrite : 지정하지 않으면 기본 값은 false
       false : 해당 필드/프로퍼티가 null인 경우에만 컴포넌트 초기화 동작을 수행합니다.
       true  : 해당 필드/프로퍼티가 null이 아닌 경우에도 컴포넌트 초기화를 수행하여, 덮어 쓰기*/
    [GetComponent(false)] public Transform _whenAllowOverwriteIsFalse;
    [GetComponent(true)]  public Transform _whenAllowOverwriteIsTrue;

    /* Private, Protected, Public 등 모든 접근지정자에 대해 정상적으로 동작합니다.
       하이라키에서 확인하고 싶은 경우, SerializeField를 함께 사용합니다. */
    [GetComponent]                 private Transform _privateMember1;
    [SerializeField, GetComponent] private Transform _privateMember2;

    /* 프로퍼티에 대해서도 동일하게 동작하지만,
       반드시 Getter와 Setter가 모두 존재해야 합니다. */
    [GetComponent] public Collider PropertyMember { get => _col; private set => _col = value; }
    public Collider _col;


    /* ******************************************************************************** *
     *                                     기능들                                       *
     * ******************************************************************************** */

    /* 게임 오브젝트 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다. */
    [GetComponent]                public CharacterMovement_Test _movement;

    /* 자신 및 자식 게임 오브젝트 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다. */
    [GetComponentInChildren]      public CharacterInventory_Test _inventory;

    /* 지정한 이름의 자식 게임 오브젝트 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다.
       해당 이름의 자식 게임 오브젝트가 존재하지 않을 경우, 아무런 동작을 하지 않습니다. */
    [GetComponentInChild("Mesh")] public Transform _meshTransform;

    /* 자신 및 부모 게임 오브젝트 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다. */
    [GetComponentInParent]        public PlayerManager_Test _playerManager;


    /* 게임 오브젝트 내에서 해당 타입의 컴포넌트들을 찾아 배열 형태로 가져옵니다. */
    [GetComponents]               public Component[] _components;

    /* 게임 오브젝트 내에서 해당 타입의 컴포넌트들을 찾아 리스트 형태로 가져옵니다. */
    [GetComponents]               public List<Component> _componentList;

    /* 자신 및 자식 게임 오브젝트 내에서 해당 타입의 컴포넌트들을 찾아 배열 형태로 가져옵니다. */
    [GetComponentsInChildren]     public CharacterWeapon_Test[] _childWeapons;

    /* 자신 및 자식 게임 오브젝트 내에서 해당 타입의 컴포넌트들을 찾아 리스트 형태로 가져옵니다. */
    [GetComponentsInChildren]     public List<CharacterWeapon_Test> _childWeaponList;

    /* 자신 및 부모 게임 오브젝트 내에서 해당 타입의 컴포넌트들을 찾아 배열 형태로 가져옵니다. */
    [GetComponentsInParent]       public Component[] _parentComponents;

    /* 자신 및 부모 게임 오브젝트 내에서 해당 타입의 컴포넌트들을 찾아 리스트 형태로 가져옵니다. */
    [GetComponentsInParent]       public List<Component> _parentComponentList;


    /* 자신을 제외한 자식 게임 오브젝트들에서 해당 타입의 컴포넌트를 찾아 가져옵니다. */
    [GetComponentInChildrenOnly]  public Collider _childOnlyCollider;

    /* 자신을 제외한 부모 게임 오브젝트들에서 해당 타입의 컴포넌트를 찾아 가져옵니다. */
    [GetComponentInParentOnly]    public Transform _parentOnlyTransform;

    /* 자신을 제외한 자식 게임 오브젝트들에서 해당 타입의 컴포넌트를 모두 찾아 배열 형태로 가져옵니다. */
    [GetComponentsInChildrenOnly] public Collider[] _childOnlyColliders;

    /* 자신을 제외한 자식 게임 오브젝트들에서 해당 타입의 컴포넌트를 모두 찾아 리스트 형태로 가져옵니다. */
    [GetComponentsInChildrenOnly] public List<Collider> _childOnlyColliderList;

    /* 자신을 제외한 부모 게임 오브젝트들에서 해당 타입의 컴포넌트를 모두 찾아 배열 형태로 가져옵니다. */
    [GetComponentsInParentOnly]   public Component[] _parentOnlyComponents;

    /* 자신을 제외한 부모 게임 오브젝트들에서 해당 타입의 컴포넌트를 모두 찾아 리스트 형태로 가져옵니다. */
    [GetComponentsInParentOnly]   public List<Component> _parentOnlyComponentList;


    /* 게임 오브젝트 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다.
       만약 해당 컴포넌트가 존재하지 않을 경우, 컴포넌트를 새롭게 생성한 후 가져옵니다. */
    [GetOrAddComponent]                    public Rigidbody _rigidbody;

    /* 지정한 이름의 자식 게임 오브젝트 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다.
       만약 해당 컴포넌트가 존재하지 않을 경우, 자식 게임 오브젝트에 컴포넌트를 새롭게 생성한 후 가져옵니다.
       만약 해당 이름의 자식 게임오브젝트가 존재하지 않을 경우, 지정한 이름으로 자식 게임오브젝트를 생성합니다. */
    [GetOrAddComponentInChild("Pet")]     public PetController_Test _pet;

    /* 자신 및 자식 게임 오브젝트 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다.
       만약 해당 컴포넌트가 존재하지 않을 경우, 자식 게임 오브젝트에 컴포넌트를 새롭게 생성한 후 가져옵니다.
       만약 해당 이름의 자식 게임오브젝트가 존재하지 않을 경우, 지정한 이름으로 자식 게임오브젝트를 생성합니다. */
    [GetOrAddComponentInChildren("Armor")] public CharacterArmor_Test _armor;

    /* 자신 및 부모 게임 오브젝트 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다.
       만약 해당 컴포넌트가 존재하지 않을 경우, 해당 부모 게임 오브젝트에 컴포넌트를 새롭게 생성한 후 가져옵니다.
       만약 해당 이름의 부모 게임오브젝트가 존재하지 않을 경우, 아무런 동작을 하지 않습니다. */
    [GetOrAddComponentInParent("Player")]  public PlayerData_Test _playerData;


    /* 현재 씬 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다. */
    [Find]                    public GameManager_Test _gameManager;

    /* 현재 씬 내에서 해당 타입의 컴포넌트를 찾아 가져옵니다.
       만약 해당 타입의 컴포넌트가 하나도 존재하지 않을 경우, 
       지정한 이름으로 게임 오브젝트를 생성한 뒤 컴포넌트 추가*/
    [FindOrAdd("UI Manager")] public UIManager_Test _uiManager;

    /* 현재 씬 내에서 지정한 이름의 게임 오브젝트를 찾고, 해당 타입의 컴포넌트를 찾아 가져옵니다.
       만약 해당 이름의 게임 오브젝트가 존재하지 않거나 해당 컴포넌트가 존재하지 않는 경우,
       아무런 동작을 하지 않습니다. */
    [FindByName("Spawner")]   public Spawner_Test _spawner;

    /* 현재 씬 내에서 지정한 이름의 게임 오브젝트를 찾고, 해당 타입의 컴포넌트를 찾아 가져옵니다.
       만약 해당 이름의 게임 오브젝트가 존재하지 않거나 해당 컴포넌트가 존재하지 않는 경우,
       해당 이름으로 새로운 게임 오브젝트를 생성한 뒤, 컴포넌트를 추가합니다. */
    [FindByNameOrAdd("Pool")] public ObjectPool_Test _objPool;

    /* 현재 씬 내에서 해당 타입의 컴포넌트를 모두 찾아 배열 형태로 가져옵니다. */
    [FindAll] public Transform[] _allTransforms;

    /* 현재 씬 내에서 해당 타입의 컴포넌트를 모두 찾아 리스트 형태로 가져옵니다. */
    [FindAll] public List<Collider> _allColliderList;
}
