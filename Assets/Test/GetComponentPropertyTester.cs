using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito;

public class GetComponentPropertyTester : MonoBehaviour
{
    [GetComponent]
    public Transform Tr_Getcompo_Awake { get => m_tr_Awake; set => m_tr_Awake = value; }
    public Transform m_tr_Awake;
    [GetComponent]
    public Collider Col_Getcompo_Awake { get => m_col_Awake; set => m_col_Awake = value; }
    public Collider m_col_Awake;

    [GetComponent(EventFlow.Start)]
    public Transform Tr_Getcompo_Start { get => m_tr_Start; set => m_tr_Start = value; }
    public Transform m_tr_Start;
    [GetComponent(EventFlow.Start)]
    public Collider Col_Getcompo_Start { get => m_col_Start; set => m_col_Start = value; }
    public Collider m_col_Start;

    [GetComponentInChildren]
    public Transform Tr_GetcompoInChildren_Awake { get => m_tr_child_Awake; set => m_tr_child_Awake = value; }
    public Transform m_tr_child_Awake;

    [GetComponentInChildren(EventFlow.Start)]
    public Collider Col_GetcompoInChildren_Start { get => m_col_child_Start; set => m_col_child_Start = value; }
    public Collider m_col_child_Start;

    [GetComponentInParent]
    public Transform Tr_GetcompoInParent_Awake { get => m_tr_parent_Awake; set => m_tr_parent_Awake = value; }
    public Transform m_tr_parent_Awake;

    [GetComponentInParent(EventFlow.Start)]
    public Collider Col_GetcompoInParent_Start { get => m_col_parent_Start; set => m_col_parent_Start = value; }
    public Collider m_col_parent_Start;


    [GetComponentsInParent]
    public Component[] Com_Getcompo_Array_Awake { get => m_com_array_Awake; set => m_com_array_Awake = value; }
    public Component[] m_com_array_Awake;

    [GetComponentsInChildren]
    public Transform[] Tr_Getcompo_Array_Awake { get => m_tr_array_Awake; set => m_tr_array_Awake = value; }
    public Transform[] m_tr_array_Awake;

    [GetComponents(EventFlow.Start)]
    public Collider[] Col_Getcompo_Array_Start { get => m_col_array_Start; set => m_col_array_Start = value; }
    public Collider[] m_col_array_Start;


    [GetComponents]
    public List<Component> Com_Getcompo_List_Awake { get => m_com_list_Awake; set => m_com_list_Awake = value; }
    public List<Component> m_com_list_Awake;

    [GetComponents]
    public List<Transform> Tr_Getcompo_List_Awake { get => m_tr_list_Awake; set => m_tr_list_Awake = value; }
    public List<Transform> m_tr_list_Awake;

    [GetComponents(EventFlow.Start)]
    public List<Collider> Col_Getcompo_List_Start { get => m_col_list_Start; set => m_col_list_Start = value; }
    public List<Collider> m_col_list_Start;

    // === Get Or Add === //

    // 1-1. 해당 컴포넌트를 갖고 있는 부모 오브젝트
    [GetOrAddComponentInParent("Parent1", EventFlow.Start)]
    public SphereCollider m_parent1_SphereCollider { get => m11; set => m11 = value; }
    [Header("Parent1 - Get")] public SphereCollider m11;

    // 1-2. 해당 컴포넌트를 갖고 있지 않는 부모 오브젝트
    [GetOrAddComponentInParent("Parent2")]
    public Rigidbody m_parent2_Rigidbody { get => m12; set => m12 = value; }
    [Header("Parent2 - Add")] public Rigidbody m12;

    // 1-3. 해당 컴포넌트를 갖고 있지 않으며, 이름을 못찾는 경우 (None으로 유지)
    [GetOrAddComponentInParent("")]
    public BoxCollider m_parent3_BoxCollider { get => m13; set => m13 = value; }
    [Header("Parent3 - cannotFind")] public BoxCollider m13;

    // 2-1. 해당 컴포넌트를 갖고 있는 자기 오브젝트
    [GetOrAddComponent()]
    public CapsuleCollider m_capsuleCollider { get => m21; set => m21 = value; }
    [Header("Self - Get")] public CapsuleCollider m21;

    // 2-2. 해당 컴포넌트를 갖고 있지 않는 자기 오브젝트 대상
    [GetOrAddComponent()]
    public Light m_light { get => m22; set => m22 = value; }
    [Header("Self - Add")]  public Light m22;

    // 3-1. 해당 컴포넌트를 갖고 있는 자식 오브젝트
    [GetOrAddComponentInChildren("Child1")]
    public WheelCollider m_child1_WheelCollider { get => m31; set => m31 = value; }
    [Header("Child1 - Get")] public WheelCollider m31;

    // 3-2. 해당 컴포넌트를 갖고 있지 않는 자식 오브젝트
    [GetOrAddComponentInChildren("Child2")]
    public Rigidbody m_child2_Rigidbody { get => m32; set => m32 = value; }
    [Header("Child2 - Add")] public Rigidbody m32;

    // 3-3. 해당 컴포넌트를 갖고 있지 않으며, 이름을 못찾는 경우 (새로 생성)
    [GetOrAddComponentInChildren("Child3")]
    public MeshCollider m_child3_MeshCollider { get => m33; set => m33 = value; }
    [Header("Child - New")] public MeshCollider m33;


    // 4-1. Setter가 없는 경우 - GetComponent
    [GetOrAddComponent()]
    public Transform m_noSetterMethod { get => m41;}
    [Header("Self - No Setter")] public Transform m41;

    // 4-2. Setter가 없는 경우 - GetComponentInChildren
    [GetOrAddComponentInChildren("Child1")]
    public Transform m_child_noSetterMethod => m42;
    [Header("Child - No Setter")] public Transform m42;

    // 4-3. Setter가 없는 경우 - GetComponentInParent
    [GetOrAddComponentInParent("Parent1")]
    public Transform m_Parent_noSetterMethod { get; }

    private void Start()
    {
        if (m_parent2_Rigidbody) m_parent2_Rigidbody.useGravity = false;
        if (m_child2_Rigidbody) m_child2_Rigidbody.useGravity = false;
    }
}
