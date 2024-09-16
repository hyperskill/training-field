using System;
using System.Collections;
using UnityEngine;
using WindowsInput;
using UnityEditor;

public class ShootScript : MonoBehaviour
{
    private InputSimulator IS = new InputSimulator();
    private GameObject secondCamera;

    private GameObject camera;

    public void shoot(Vector3 where, bool scope = false)
    {
        if (camera == null)
        {
            camera = GameObject.Find("Main Camera");
        }
        if (secondCamera == null)
        {
            secondCamera = Instantiate(new GameObject());
            secondCamera.transform.parent = transform;
            secondCamera.transform.localPosition = camera.transform.localPosition;
            secondCamera.transform.localRotation = camera.transform.localRotation;
            secondCamera.transform.localScale = camera.transform.localScale;
            camera.transform.parent = secondCamera.transform;
        }
        StartCoroutine(hitTarget(where, scope));
    }

    public IEnumerator hitTarget(Vector3 where, bool scope)
    {
        EditorWindow game=null;
        var windows = (EditorWindow[])Resources.FindObjectsOfTypeAll(typeof(EditorWindow));
        foreach(var window in windows)
        {
            if(window != null && window.GetType().FullName == "UnityEditor.GameView")
            {
                game = window;
                break;
            }
        }
        game.maximized = true;
        yield return null;
        float X = game.position.center.x;
        X = X * 65535 / Screen.width/2;
        float Y = game.position.center.y;
        Y = Y * 65535 / Screen.height/2;
        IS.Mouse.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        secondCamera.transform.LookAt(where);
        yield return null;
        if(!scope)
            IS.Mouse.LeftButtonClick();
    }
}
