using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class EnvironmentLibrary : MonoBehaviour
{
    // number of contents
    private static int contentCount = 10;

    // number of distortions
    private static int distortionCount = 5;

    // Initialize training environments, they need to be set through inspector manually. Randomized to mitigate order bias.
    public List<Environment> m_TrainingEnvironments = null;
    public List<Environment> m_RandomTrainingEnvironments = null;

    // Initialize main environments - they are assigned in Awake function automatically. Ranodmized to mitigate order bias.
    public List<Environment> m_MainEnvironments = new List<Environment>();
    public List<Environment> m_RandomMainEnvironments = new List<Environment>();

    // starting position for the observer - we change in each test image to make sure all of the angles are explored by the user
    // public int[] MainWorldRotations = Enumerable.Range(0, contentCount).Select(x => x * 36).ToArray<int>();

    // just a content id to keep track of the test images
    public int[] MainContentIndex = Enumerable.Range(1, contentCount).ToArray<int>();

    public UnityEngine.Object[] textures = null;

    // a list of tuples that contains scaling factors
    public List<Tuple<float, float>> scalingFactors = new List<Tuple<float, float>>();
    private int session;



    private void Awake()
    {
        // Retrieve session id
        session = PlayerPrefs.GetInt("session");

        // Based on the session id decide which combination of the scaling factors should be used. First: focus, Second: context.
        if (session == 1)
        {
            scalingFactors.Add(new Tuple<float, float>(1.0f, 1.0f));
            scalingFactors.Add(new Tuple<float, float>(1.0f, 0.8f));
            scalingFactors.Add(new Tuple<float, float>(1.0f, 0.7f));
            scalingFactors.Add(new Tuple<float, float>(1.0f, 0.6f));
            scalingFactors.Add(new Tuple<float, float>(1.0f, 0.5f));
        } else
        {
            scalingFactors.Add(new Tuple<float, float>(1.0f, 1.0f));
            scalingFactors.Add(new Tuple<float, float>(1.0f, 0.9f));
            scalingFactors.Add(new Tuple<float, float>(1.0f, 0.8f));
            scalingFactors.Add(new Tuple<float, float>(1.0f, 0.7f));
            scalingFactors.Add(new Tuple<float, float>(1.0f, 0.6f));
        }

        // loads all the omnidirectional images from the folder named Resournce
        textures = Resources.LoadAll("Textures");

        // generated a list of environments
        for (int i = 0; i < contentCount; ++i)
        {
            for (int j = 0; j < distortionCount; ++j)
            {
                m_MainEnvironments.Add(new Environment
                {
                    m_WorldRotation = 0,
                    m_ContentIndex = MainContentIndex[i],
                    m_FocusScalingFactor = scalingFactors[j].Item1,
                    m_ContextScalingFactor = scalingFactors[j].Item2,
                    m_Background = (Texture2D)textures[i]
                });
            }
        }

        System.Random rand = new System.Random();

        // add random duplicate elements (10 percent) to the list for user consistency analysis
        int k = 0;
        while (k < 5)
        {
            int rI = rand.Next(0, contentCount - 1);
            int rJ = rand.Next(0, distortionCount - 1);
            m_MainEnvironments.Add(new Environment
            {
                m_WorldRotation = 0,
                m_ContentIndex = MainContentIndex[rI],
                m_FocusScalingFactor = scalingFactors[rJ].Item1,
                m_ContextScalingFactor = scalingFactors[rJ].Item2,
                m_Background = (Texture2D)textures[rI]
            });

            k++;
        }

        // shuffle the environment list to ensure that order doesn't affect user ratings
        m_RandomMainEnvironments = m_MainEnvironments.OrderBy(_ => rand.Next()).ToList();
        m_RandomTrainingEnvironments = m_TrainingEnvironments.OrderBy(_ => rand.Next()).ToList();
    }
}

[Serializable]
public class Environment
{
    // world rotation determines the rotation of the 360-image
    public int m_WorldRotation = 0;
    public int m_ContentIndex = 0;
    public float m_FocusScalingFactor = 1;
    public float m_ContextScalingFactor = 1;
    public Texture m_Background = null;
}