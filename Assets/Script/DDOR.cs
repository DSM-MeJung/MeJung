using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOR : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
