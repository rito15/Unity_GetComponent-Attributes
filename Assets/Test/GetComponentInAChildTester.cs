using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito;

public class GetComponentInAChildTester : MonoBehaviour
{
    [GetComponentInAChild("Child1")]
    public Transform childTr;

    [GetComponentInAChild("Child1")]
    public Collider childcol;
}
