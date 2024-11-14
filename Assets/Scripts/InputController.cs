using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Varjo.XR;

public class InputController : MonoBehaviour
{
    [Header("Select hand")]
    public XRNode XRNode = XRNode.LeftHand;

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    private Quaternion deviceRotation; //Controller rotation
    private Vector3 devicePosition; //Controller position
    private Vector3 deviceAngularVelocity; // Controller angular velocity
    private Vector3 deviceVelocity; // Controller velocity
    private Vector3 triggerRotation; // Controller trigger rotation
    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    private void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }

        // Get values for device position, rotation and buttons.
        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition))
        {
            transform.localPosition = devicePosition;
        }

        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation))
        {
            transform.localRotation = deviceRotation;
        }

        device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out deviceAngularVelocity);

        device.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity);
    }
    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XRNode, devices);
        device = devices.FirstOrDefault();
    }

}
