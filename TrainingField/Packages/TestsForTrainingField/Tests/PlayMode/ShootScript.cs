using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class ShootScript : MonoBehaviour
{
    private GameObject secondCam;

    private GameObject Cam;

    public void shoot(Vector3 where, bool scope = false)
    {
        if (Cam == null)
        {
            Cam = GameObject.Find("Main Camera");
        }
        if (secondCam == null)
        {
            secondCam = Instantiate(new GameObject());
            secondCam.transform.parent = transform;
            secondCam.transform.localPosition = Cam.transform.localPosition;
            secondCam.transform.localRotation = Cam.transform.localRotation;
            secondCam.transform.localScale = Cam.transform.localScale;
            Cam.transform.parent = secondCam.transform;
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
        VInput.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        secondCam.transform.LookAt(where);
        yield return null;
        if(!scope)
            VInput.LeftButtonClick();
    }
}