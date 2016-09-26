using UnityEngine;
using System.Collections;

public delegate void Callback();
public delegate void CallbackString(string param);
public delegate void CallbackColor(Color param);
public delegate void CallbackFloat(float param);
public delegate void CallbackVector3(Vector3 param);
public delegate Vector3 CallbackGetVector3();

public delegate T CallbackReturn<T>();

public delegate void Callback<T>(T param);
public delegate void Callback<T1, T2>(T1 param1, T2 param2);

public delegate bool Condition<T>(T param);
public delegate bool Condition<T1, T2>(T1 param1, T2 param2);
