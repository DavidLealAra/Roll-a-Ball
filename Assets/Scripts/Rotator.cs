using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Update se llama una vez por frame.
    void Update()
    {
        // Rota el objeto en los ejes X, Y y Z con una velocidad proporcional al tiempo delta.
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
