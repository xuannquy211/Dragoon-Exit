using System;
using System.Reflection;
using DG.Tweening;
using UnityEngine;

public static class QuickUtil
{
    private static Sequence cameraShakeEffect;
    
    public static T Clone<T>(this object source) where T : new()
    {
        var type = typeof(T);
        if (type.IsClass)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var target = new T();
            
            foreach (var prop in properties)
            {
                var value = prop.GetValue((T) source);
                prop.SetValue(target, value);
            }

            return target;
        }
        
        if (type.IsValueType && !type.IsEnum) return (T)Convert.ChangeType(source, typeof(T));
        throw new Exception("Wrong Type");
    }
    
    public static void CameraShake(Vector3 fixedPosition, float duration = 0.5f, float strength = 0.5f, bool zoomEffect = false)
    {
        var camera = Camera.main;
        if (cameraShakeEffect is { active: true } && cameraShakeEffect.IsPlaying())
        {
            cameraShakeEffect.Kill();
            camera.transform.position = fixedPosition;
        }

        cameraShakeEffect = DOTween.Sequence();
        cameraShakeEffect.Append(camera.transform.DOShakePosition(duration, Vector3.right * strength).SetEase(Ease.Linear));
        if(zoomEffect) cameraShakeEffect.Join(camera.DOOrthoSize(11f, duration).From(10f));
    }
}