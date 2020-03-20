using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito;

// 2020. 03. 20. 작성
// 2020. 03. 20. GetComponentExtension 메소드들 모두 기능 테스트 완료

public class GetOrAddTester : MonoBehaviour
{
    // 1-1. 해당 컴포넌트를 갖고 있는 부모 오브젝트
    public SphereCollider m_parent1_SphereCollider;

    // 1-2. 해당 컴포넌트를 갖고 있지 않는 부모 오브젝트
    public Rigidbody m_parent2_Rigidbody;

    // 1-3. 해당 컴포넌트를 갖고 있지 않으며, 이름을 못찾는 경우 (None으로 유지)
    public BoxCollider m_parent3_BoxCollider;

    // 2-1. 해당 컴포넌트를 갖고 있는 자기 오브젝트
    public CapsuleCollider m_capsuleCollider;

    // 2-1. 해당 컴포넌트를 갖고 있지 않는 자기 오브젝트 대상
    public  Light m_light;

    // 3-1. 해당 컴포넌트를 갖고 있는 자식 오브젝트
    public WheelCollider m_child1_WheelCollider;

    // 3-2. 해당 컴포넌트를 갖고 있지 않는 자식 오브젝트
    public Rigidbody m_child2_Rigidbody;

    // 3-3. 해당 컴포넌트를 갖고 있지 않으며, 이름을 못찾는 경우 (새로 생성)
    public MeshCollider m_child3_MeshCollider;

    void Start()
    {
        m_parent1_SphereCollider = gameObject.GetOrAddComponentInParent<SphereCollider>("Parent1");
        m_parent2_Rigidbody = gameObject.GetOrAddComponentInParent<Rigidbody>("Parent2");
        m_parent3_BoxCollider = gameObject.GetOrAddComponentInParent<BoxCollider>("");

        m_capsuleCollider = gameObject.GetOrAddComponent<CapsuleCollider>();
        m_light = gameObject.GetOrAddComponent<Light>();

        m_child1_WheelCollider = gameObject.GetOrAddComponentInChildren<WheelCollider>("Child1");
        m_child2_Rigidbody = gameObject.GetOrAddComponentInChildren<Rigidbody>("Child2");
        m_child3_MeshCollider = gameObject.GetOrAddComponentInChildren<MeshCollider>("Child3 (New)");



        if (m_parent2_Rigidbody) m_parent2_Rigidbody.useGravity = false;
        if (m_child2_Rigidbody) m_child2_Rigidbody.useGravity = false;
    }
}
