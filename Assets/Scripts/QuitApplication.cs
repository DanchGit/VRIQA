using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void QuitGame()
    {
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}
