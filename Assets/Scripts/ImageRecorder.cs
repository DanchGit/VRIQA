using System.ComponentModel;
using System.IO;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UnityEngine.Recorder.Examples
{
    public class ImageRecorder : MonoBehaviour
    {
        RecorderController m_RecorderController;
        public void StartRecording(Environment environment)
        {
            var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            m_RecorderController = new RecorderController(controllerSettings);

            var mediaOutputFolder = Path.Combine(Application.dataPath, "..", "SampleRecordings");

            // Image Sequence
            var imageRecorder = ScriptableObject.CreateInstance<ImageRecorderSettings>();
            imageRecorder.name = "My Image Recorder";
            imageRecorder.Enabled = true;

            imageRecorder.OutputFormat = ImageRecorderSettings.ImageRecorderOutputFormat.JPEG;

            imageRecorder.OutputFile = Path.Combine(mediaOutputFolder, "_JPEG", "image_frame") + "." + DefaultWildcard.Frame + $"-{environment.m_ContentIndex}-{environment.m_FocusScalingFactor}-{environment.m_ContextScalingFactor}";
            imageRecorder.imageInputSettings = new GameViewInputSettings
            {
                OutputWidth = 2*1920,
                OutputHeight = 2*1080
            };

            controllerSettings.AddRecorderSettings(imageRecorder);
            controllerSettings.SetRecordModeToManual();
            controllerSettings.FrameRatePlayback = FrameRatePlayback.Variable;

            RecorderOptions.VerboseMode = true;
            m_RecorderController.PrepareRecording();
            m_RecorderController.StartRecording();
        }

        public void StopRecording()
        {
            m_RecorderController.StopRecording();
        }
    }
}