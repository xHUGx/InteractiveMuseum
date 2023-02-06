#if UNITY_EDITOR
using System;
using Features.Ar.Data;
using UnityEditor;
using UnityEngine;
using ViewSystem.Attributes;

namespace Features.Ar.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AttributeArImageTypeName : PropertyAttribute
    {
    }

    [CustomPropertyDrawer(typeof(AttributeArImageTypeName))]
    public class ArImageTypeProperty : BaseFieldDrawer<ArImageTypesConst>
    {
    }
}

#endif