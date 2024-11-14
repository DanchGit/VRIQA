using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varjo;
using Varjo.XR;

public class SkyboxController : BaseController
{
    protected override IEnumerator Apply(Environment environment)
    {
        // Fade out
        float startValue = RenderSettings.skybox.GetFloat("_Exposure");
        yield return StartCoroutine(Interpolate(0.25f, startValue, 0.0f, UpdateExposureCallback));

        // Set texture
        RenderSettings.skybox.SetFloat("_Rotation", environment.m_WorldRotation);
        RenderSettings.skybox.mainTexture = environment.m_Background;
        VarjoRendering.SetFocusScalingFactor(environment.m_FocusScalingFactor);
        VarjoRendering.SetContextScalingFactor(environment.m_ContextScalingFactor);

        // Fade in
        startValue = RenderSettings.skybox.GetFloat("_Exposure");
        yield return StartCoroutine(Interpolate(0.25f, startValue, 1.0f, UpdateExposureCallback));
    }

    protected IEnumerator Retrieve()
    {
        // Fade out
        float startValue = RenderSettings.skybox.GetFloat("_Exposure");
        yield return StartCoroutine(Interpolate(0.25f, startValue, 0.0f, UpdateExposureCallback));

        // Remove texture
        RenderSettings.skybox.mainTexture = null;

        // Fade in
        startValue = RenderSettings.skybox.GetFloat("_Exposure");
        yield return StartCoroutine(Interpolate(0.25f, startValue, 1.0f, UpdateExposureCallback));
    }

    private void UpdateExposureCallback(float value)
    {
        RenderSettings.skybox.SetFloat("_Exposure", value);
    }

    public void NewRetrieveScore()
    {
        StopAllCoroutines();
        StartCoroutine(Retrieve());
    }
}
