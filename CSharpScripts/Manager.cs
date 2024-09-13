using IngameDebugConsole;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static int maxDerbis = 3500;
    public static Manager inst;
    [SerializeField] GameObject vrObject;
    [SerializeField] GameObject pcObject;
    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        if (MainMenuManager.isVR)
            VrPlayerSetup();
        else
            NonVrPlayerSetup();
    }
    public static void DerbisDestroy()
    {
        GameObject[] derbises = GameObject.FindGameObjectsWithTag("Derbis");
        int derbisLen = derbises.Length;
        foreach (GameObject x in derbises)
        {
            if (derbisLen > maxDerbis)
            {
                derbisLen--;
                Destroy(x, 1.5f);
            }
        }
    }
    [ConsoleMethod("deb_m","Set max derbis. Setting value too high can be laggy. Default is 3500")]
    public static void SetDerbis(int derbis)
    {
        maxDerbis = derbis;
    }
    [ConsoleMethod("pc","Go to PC mode instead of VR")]
    public static void NonVrPlayerSetup()
    {
        inst.vrObject.SetActive(false);
        inst.pcObject.SetActive(true);
    }
    [ConsoleMethod("vr", "Go to VR mode instead of PC")]
    public static void VrPlayerSetup()
    {
        inst.pcObject.SetActive(false);
        inst.vrObject.SetActive(true);
    }
}
