using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static bool isVR = false;
    public static float sensitivity = 0.2f;
    GameObject sensObj;
    private void Start()
    {
        sensObj = GameObject.Find("Canvas/Sensitivity");
        sensObj.GetComponent<Slider>().value = sensitivity;
        isVR = false;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("BasicScene");
    }
    public void ChangeVROption()
    {
        sensObj.SetActive(isVR);
        isVR = !isVR;
        Debug.Log(isVR);
    }
    public void ChangeSensitivity(float _sensitivity)
    {
        sensitivity = _sensitivity;
    }
}
