﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泛型 和 单例

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = this as T;
    }
}
