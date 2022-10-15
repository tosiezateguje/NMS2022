using UnityEngine;
using UnityEngine.Rendering;
 
public class RuntimeUIFix : MonoBehaviour
{
    private void Awake()
    {
        DebugManager.instance.enableRuntimeUI = false;
    }
}

