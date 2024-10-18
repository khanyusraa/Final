using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSupport : MonoBehaviour
{
    public Material pulseMaterial;

    private void Start()
    {
        pulseMaterial.SetFloat("_PulseSpeed", 0f);  // End pulse
        pulseMaterial.SetFloat("_BorderThickness", 0f);  // Make border invisible
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pulseMaterial != null)
        {
            // Apply the pulsing shader
            Graphics.Blit(src, dest, pulseMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);  // Fallback to standard rendering
        }
    }
}

