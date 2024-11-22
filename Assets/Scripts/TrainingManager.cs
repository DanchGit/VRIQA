using Newtonsoft.Json.Bson;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Michsky.MUIP;
using Newtonsoft.Json.Linq;
using Varjo.XR;
using System.Xml.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using TMPro;

public class TrainingManager : MonoBehaviour
{
    public EnvironmentLibrary m_EnvironmentLibrary = null;
    public XROrigin m_XROrigin = null;
    public GameObject Canvas = null;
    public GameObject RayCaster = null;
    public TMP_Text Percent = null;
    [SerializeField] private Slider inputSlider = null;

    public UnityEvent OnTrainingFinish = null;
    public UnityEvent OnRetrieveScore = null;
    public UnityEvent OnCanvasActivation = null;
    [Serializable] public class NewEnvironment : UnityEvent<Environment> { }
    public NewEnvironment OnNewEnvironment = null;
    private float timePerTestImage = 5.0f;
    private int environmentIndex = 0;
    private int userId;
    private int session;
    private int envsLength;

    private void Start()
    {
        userId = PlayerPrefs.GetInt("userId");
        session = PlayerPrefs.GetInt("session");
        envsLength = m_EnvironmentLibrary.m_RandomTrainingEnvironments.Count;
        StartCoroutine("waitAndSelectNext");
    }

    private IEnumerator waitAndSelectNext()
    {
        // Selects a new omnidirectional image
        Select();

        // Wait for a specific time before changing the image
        yield return new WaitForSeconds(timePerTestImage);

        // Retrieve Score for the image apllied to skybox
        RetrieveScore();
    }
    private void Select()
    {
        // Deactivate canvas and raycaster
        Canvas.SetActive(false);

        // Send new environment - to be assingned to the skybox
        RayCaster.SetActive(false);

        // Start logging eye-tracking
        OnNewEnvironment.Invoke(m_EnvironmentLibrary.m_RandomTrainingEnvironments[environmentIndex]);
    }

    private void RetrieveScore()
    {
        // Event for getting the score
        OnRetrieveScore.Invoke();

        // Set foveated rendering parameters back to 1
        VarjoRendering.SetFocusScalingFactor(1);
        VarjoRendering.SetContextScalingFactor(1);

        // Activate canvas and raycaster
        OnCanvasActivation.Invoke();
        UpdateProgress();
        Canvas.SetActive(true);
        RayCaster.SetActive(true);
    }

    public void Submit()
    {
        SaveQualityRating();

        // Add to the index number
        environmentIndex++;

        // checks if all of the images are shown. If all are already shown, goes to the next scence
        if (environmentIndex == m_EnvironmentLibrary.m_RandomTrainingEnvironments.Count)
            OnTrainingFinish.Invoke();

        StartCoroutine("waitAndSelectNext");
    }

    public void SaveQualityRating()
    {
        string path = $"{Application.dataPath}/Static/Training/" + "Training-UsersQualityScores.csv";
        if (!File.Exists(path))
        {
            string header = "UserId,Session,ContentIndex,FocusScalingFactor,ContextScalingFactor,Score\n";
            File.AppendAllText(path, header);
        }
        string values = $"{userId},{session},{m_EnvironmentLibrary.m_RandomTrainingEnvironments[environmentIndex].m_ContentIndex},{m_EnvironmentLibrary.m_RandomTrainingEnvironments[environmentIndex].m_FocusScalingFactor},{m_EnvironmentLibrary.m_RandomTrainingEnvironments[environmentIndex].m_ContextScalingFactor},{inputSlider.value}\n";
        File.AppendAllText(path, values);
    }

    public void UpdateProgress()
    {
        Percent.text = (environmentIndex * 100 / envsLength).ToString() + "%";
    }
}
