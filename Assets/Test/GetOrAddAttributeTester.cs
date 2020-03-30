using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito;

public class GetOrAddAttributeTester : MonoBehaviour
{
    // 1-1. 해당 컴포넌트를 갖고 있는 부모 오브젝트
    [GetOrAddComponentInParent("Parent1", EventFlow.Start)]
    public SphereCollider m_parent1_SphereCollider;

    // 1-2. 해당 컴포넌트를 갖고 있지 않는 부모 오브젝트
    [GetOrAddComponentInParent("Parent2")]
    public Rigidbody m_parent2_Rigidbody;

    // 1-3. 해당 컴포넌트를 갖고 있지 않으며, 이름을 못찾는 경우 (None으로 유지)
    [GetOrAddComponentInParent("")]
    public BoxCollider m_parent3_BoxCollider_none;

    // 2-1. 해당 컴포넌트를 갖고 있는 자기 오브젝트
    [GetOrAddComponent()]
    public CapsuleCollider m_capsuleCollider;

    // 2-2. 해당 컴포넌트를 갖고 있지 않는 자기 오브젝트 대상
    [GetOrAddComponent()]
    public Light m_light;

    // 3-1. 해당 컴포넌트를 갖고 있는 자식 오브젝트
    [GetOrAddComponentInChildren("Child1")]
    public WheelCollider m_child1_WheelCollider;

    // 3-2. 해당 컴포넌트를 갖고 있지 않는 자식 오브젝트
    [GetOrAddComponentInChildren("Child2")]
    public Rigidbody m_child2_Rigidbody;

    // 3-3. 해당 컴포넌트를 갖고 있지 않으며, 이름을 못찾는 경우 (새로 생성)
    [GetOrAddComponentInChildren("Child3")]
    public MeshCollider m_child3_MeshCollider;

    private void Start()
    {
        if (m_parent2_Rigidbody) m_parent2_Rigidbody.useGravity = false;
        if (m_child2_Rigidbody) m_child2_Rigidbody.useGravity = false;
    }
}
