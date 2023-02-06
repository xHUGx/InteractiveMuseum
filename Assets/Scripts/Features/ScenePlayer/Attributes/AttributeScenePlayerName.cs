#if UNITY_EDITOR
using System;
using Features.ScenePlayer.Data;
using UnityEditor;
using UnityEngine;
using ViewSystem.Attributes;

namespace Features.ScenePlayer.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AttributeScenePlayerName : PropertyAttribute
    {
    }

    [CustomPropertyDrawer(typeof(AttributeScenePlayerName))]
    public class VoiceProperty : BaseFieldDrawer<ScenePlayerConst>
    {
    }
}

#endif