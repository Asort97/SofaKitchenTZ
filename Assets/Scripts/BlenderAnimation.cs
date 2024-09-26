using System;
using UnityEngine;

public class BlenderAnimation : MonoBehaviour
{
    public static Action OnEndBlending;
    public void EndAnimation()
    {
        OnEndBlending?.Invoke();
    }
}