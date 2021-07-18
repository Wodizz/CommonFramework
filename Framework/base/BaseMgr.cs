using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseMgr<T> where T : new()
{
    private static T instance = new T();

    public static T Instance => instance;

}
