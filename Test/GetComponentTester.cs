using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito;

// 2020. 03. 18. 작성

public class GetComponentTester : MonoBehaviour
{
    [GetComponent]
    public Transform m_publicTr_Getcompo_Awake;
    [GetComponent]
    public Collider m_publicCol_Getcompo_Awake;

    [GetComponent(EventFlow.Start)]
    public Transform m_publicTr_Getcompo_Start;
    [GetComponent(EventFlow.Start)]
    public Collider m_publicCol_Getcompo_Start;

    [GetComponentInChildren]
    public Transform m_publicTr_GetcompoInChildren_Awake;

    [GetComponentInChildren(EventFlow.Start)]
    public Collider m_publicCol_GetcompoInChildren_Start;

    [GetComponentInParent]
    public Transform m_publicTr_GetcompoInParent_Awake;

    [GetComponentInParent(EventFlow.Start)]
    public Collider m_publicCol_GetcompoInParent_Start;


    [GetComponents]
    public Component[] m_publicCom_Getcompo_Array_Awake;

    [GetComponents]
    public Transform[] m_publicTr_Getcompo_Array_Awake;

    [GetComponents(EventFlow.Start)]
    public Collider[] m_publicCol_Getcompo_Array_Start;


    [GetComponents]
    public List<Component> m_publicCom_Getcompo_List_Awake;

    [GetComponents]
    public List<Transform> m_publicTr_Getcompo_List_Awake;

    [GetComponents(EventFlow.Start)]
    public List<Collider> m_publicCol_Getcompo_List_Start;


    private void Start()
    {
        //m_publicCol_Getcompo_Array_Start = gameObject.GetComponents<Collider>();
        //m_publicCol_Getcompo_Array_Start = gameObject.GetComponents(typeof(Collider)) as Collider[];
        

    }


    // 안되는 것들 (Component가 아닌 필드, public이 아닌 필드)
    [GetComponent]
    public int a;

    [GetComponent]
    protected Transform m_protectedTr_Getcompo;

    [GetComponent]
    protected Collider m_protectedCol_Getcompo;

    [GetComponent]
    private Transform m_privateTr_Getcompo;

    [GetComponent]
    private Collider m_privateCol_Getcompo;

    public Transform m_publicTr;
}
