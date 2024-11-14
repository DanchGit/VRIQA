using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PreManager : MonoBehaviour
{
    public UnityEvent onSceneLoad = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitAndAlign());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator waitAndAlign()
    {
        yield return new WaitForSeconds(0.01f);
        onSceneLoad.Invoke();
    }
}
