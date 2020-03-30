using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito;

public class TestCharacter : MonoBehaviour
{
    [GetComponent]
    public Collider  _collider;

    [GetOrAddComponent]
    public Rigidbody _rigidbody;

    [GetOrAddComponent(EventFlow.Start)]
    public Animator  _animator;

    [GetComponentInChildren]
    public Weapon    _weapon;      // 자식에서 찾아 가져오기

    // 자식에서 찾고, 없으면 자식으로 "Inventory" 게임오브젝트 생성 및 컴포넌트 할당
    [GetOrAddComponentInChildren(EventFlow.Start, "Inventory")]
    public Inventory _inventory;

    [GetComponents]
    public Component[]     _components;
    
    [GetComponentsInChildren(EventFlow.Start)]
    public Transform[]     _transforms;

    [GetComponentsInChildren(EventFlow.Start)]
    public List<Transform> _trList;
}


/*
[RequireComponent(typeof(Rigidbody))]
public class TestCharacter : MonoBehaviour
{
    public Collider  _collider;
    public Rigidbody _rigidbody;
    public Animator  _animator;
    public Weapon    _weapon;      // 자식에서 찾아 가져오기

    // 자식에서 찾고, 없으면 자식으로 "Inventory" 게임오브젝트 생성 및 컴포넌트 할당
    public Inventory _inventory;

    public Component[]     _components;
    public Transform[]     _transforms;
    public List<Transform> _trList;

    private void Awake()
    {
        _collider   = GetComponent<Collider>();
        _rigidbody  = GetComponent<Rigidbody>();
        _components = GetComponents<Component>();
        _weapon     = GetComponentInChildren<Weapon>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
            _animator = gameObject.AddComponent<Animator>();

        _inventory = GetComponentInChildren<Inventory>();
        if (_inventory == null)
        {
            Transform inventoryTr = transform.Find("Inventory");
            if (inventoryTr != null)
                _inventory = inventoryTr.gameObject.AddComponent<Inventory>();
            else
            {
                GameObject inventoryGO = new GameObject("Inventory");
                inventoryGO.transform.SetParent(transform);
                _inventory = inventoryGO.AddComponent<Inventory>();
            }
        }

        _transforms = GetComponentsInChildren<Transform>();
        _trList = new List<Transform>(GetComponentsInParent<Transform>());
    }
}
*/
