using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFly : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] int magnitude;

    void Update() {
        transform.localPosition += direction * magnitude * Time.deltaTime;
    }
}
