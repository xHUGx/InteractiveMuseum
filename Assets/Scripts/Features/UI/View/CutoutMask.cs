﻿using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Features.UI.View
{
    public class CutoutMask : Image
    {

        public override Material materialForRendering
        {
            get
            {
                var material = new Material(base.materialForRendering);
                material.SetInt("_StencilComp",(int)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}