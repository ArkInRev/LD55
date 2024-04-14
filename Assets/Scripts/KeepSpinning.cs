using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSpinning : MonoBehaviour
{
    public float spinSpeed;
    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(0, spinSpeed * Time.fixedDeltaTime, 0);
    }
}