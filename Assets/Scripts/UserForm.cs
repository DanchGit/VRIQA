using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.MUIP;
using System;
using System.IO;
using System.Text;
using UnityEngine.Events;

public class UserForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputUserId = null;
    [SerializeField] private TMP_InputField inputAge = null;
    [SerializeField] private TMP_InputField inputSession = null;
    [SerializeField] private CustomDropdown inputGender = null;
    [SerializeField] private CustomDropdown inputEducation = null;
    [SerializeField] private CustomDropdown inputExperience = null;
    [SerializeField] private ButtonManager inputButton = null;
    public UnityEvent onSceneLoad = new UnityEvent();

    private IEnumerator waitAndAlign()
    {
        yield return new WaitForSeconds(0.01f);
        onSceneLoad.Invoke();
    }

    private void Start()
    {
        StartCoroutine(waitAndAlign());
        inputButton.Interactable(false);

        inputUserId.onValueChanged.AddListener(delegate
        {
            Validate();
        });

        inputAge.onValueChanged.AddListener(delegate
        {
            Validate();
        });

        inputSession.onValueChanged.AddListener(delegate
        {
            Validate();
        });

        inputButton.onClick.AddListener(delegate
        {
            SaveUsersDemographics();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("userId", Int32.Parse(inputUserId.text));
            PlayerPrefs.SetInt("session", Int32.Parse(inputSession.text));
        });
    }
    private void Validate()
    {
        if (inputUserId.text != "" && inputAge.text != "" && inputSession.text != "")
            inputButton.Interactable(true);
    }

    public void SaveUsersDemographics()
    {
        string path = $"{Application.dataPath}/Static/Demographics/" + "UsersDemographics.csv";
        if (!File.Exists(path))
        {
            string header = "UserId,Age,Session,Gender,Education,Experience\n";
            File.AppendAllText(path, header);
        }
        string values = $"{Int32.Parse(inputUserId.text)},{Int32.Parse(inputAge.text)},{Int32.Parse(inputSession.text)},{inputGender.items[inputGender.index].itemName},{inputEducation.items[inputEducation.index].itemName},{inputExperience.items[inputExperience.index].itemName}\n";
        File.AppendAllText(path, values);
    }
}
