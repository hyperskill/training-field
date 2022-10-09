using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

[Description(""), Category("4")]
public class Stage4_Tests
{
    private GameObject player, camera, secondcamera;
    private GameObject target;
    private List<Vector3> positions = new List<Vector3>();

    [UnityTest, Order(0)]
    public IEnumerator TargetCheck()
    {
        SceneManager.LoadScene("Application");
        yield return new WaitForSeconds(0.2f);
        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
        secondcamera = GameObject.Instantiate(new GameObject());
        secondcamera.transform.parent = player.transform;
        secondcamera.transform.localPosition = camera.transform.localPosition;
        secondcamera.transform.localRotation = camera.transform.localRotation;
        secondcamera.transform.localScale = camera.transform.localScale;
        camera.transform.parent = secondcamera.transform;
        yield return null;
        GameObject[] targets = PMHelper.FindObjectsWithLayer("Target");
        if (targets.Length == 0)
        {
            Assert.Fail("After the scene has been loaded there should be a target spawned on a \"Target\" layer");
        }
        if (targets.Length != 1)
        {
            Assert.Fail("After the scene has been loaded there should be only one target on a scene");
        }

        target = targets[0];
        MeshFilter filter = PMHelper.Exist<MeshFilter>(target); 
        if (filter == null)
        {
            Assert.Fail("There should be <MeshFilter> component applied to target object");
        }
        GameObject tmpPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        if (!filter.mesh == tmpPlane.GetComponent<MeshFilter>().mesh)
        {
            Assert.Fail("Target's mesh should be a plane");   
        }
        if (PMHelper.Exist<MeshRenderer>(target) == null)
        {
            Assert.Fail("There should be <MeshRenderer> component applied to target object");
        }

        GameObject.Destroy(tmpPlane);
        
        Collider col = PMHelper.Exist<MeshCollider>(target);
        if (col == null)
        {
            Assert.Fail("There should be <MeshCollider> component applied to target object");
        }
        if (!col.isTrigger)
        {
            Assert.Fail("Target's <MeshCollider> component should be triggerable");
        }
        
        secondcamera.transform.LookAt(target.transform.position);
        Vector3 dir = (target.transform.position - camera.transform.position).normalized;
        RaycastHit hit = PMHelper.FindHit(camera.transform.position, dir, "Bounds");
        RaycastHit hit2 = PMHelper.FindHit(camera.transform.position, dir, "Target");
        //Debug.DrawLine(player.transform.position,hit.point, Color.red,1000);
        yield return null;
        if (hit.normal != hit2.normal)
        {
            Assert.Fail("Spawned target should be parallel with the wall it's hanging on");
        }

        if (Vector3.Distance(hit2.point, player.transform.position) >=
            Vector3.Distance(hit.point, player.transform.position))
        {
            Assert.Fail("Target should be spawned a little closer to a center of a room, than walls, in order" +
                        "not to clip with them");
        }
        
        if (Vector3.Distance(hit.point,hit2.point)>0.5f)
        {
            Assert.Fail("Target should be spawned closer to a wall, like it is hanging on a wall");
        }
    }

    [UnityTest, Order(1)]
    public IEnumerator TargetDestroyCheck()
    {
        yield return null;
        Time.timeScale = 20;
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

        if (!game)
        {
            Assert.Fail("Please, open, the \"Game\" window!");
        }

        game.maximized = true;
        yield return null;
        float X = game.position.center.x;
        X = X * 65535 / Screen.width/2;
        float Y = game.position.center.y;
        Y = Y * 65535 / Screen.height/2;
        VInput.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        yield return null;
        VInput.LeftButtonClick();
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 9; i++)
        {
            GameObject[] targets = PMHelper.FindObjectsWithLayer("Target");
            yield return null;
            if (targets.Length == 0)
            {
                Assert.Fail("When the target is been destroyed there should be spawned another one");
            }

            if (targets.Length != 1)
            {
                Assert.Fail("There should always be only one target on a scene");
            }

            if (targets[0] == target)
            {
                Assert.Fail("Target is not being destroyed by clicking on it");
            }

            target = targets[0];
            if (positions.Contains(target.transform.position))
            {
                Assert.Fail("Targets should be spawned randomly");
            }
            positions.Add(target.transform.position);
            yield return null;
            secondcamera.transform.LookAt(target.transform.position);
            //Repeat target-correct check
            Vector3 dir = (target.transform.position - camera.transform.position).normalized;
            RaycastHit hit = PMHelper.FindHit(camera.transform.position, dir, "Bounds");
            RaycastHit hit2 = PMHelper.FindHit(camera.transform.position, dir, "Target");
            yield return null;
            if (hit.normal != hit2.normal)
            {
                Assert.Fail("Spawned target should be parallel with the wall it's hanging on");
            }

            if (Vector3.Distance(hit2.point, player.transform.position) >=
                Vector3.Distance(hit.point, player.transform.position))
            {
                Assert.Fail("Target should be spawned a little closer to a center of a room, than walls, in order" +
                            "not to clip with them");
            }
            if (Vector3.Distance(hit.point,hit2.point)>0.5f)
            {
                Assert.Fail("Target should be spawned closer to a wall, like it is hanging on a wall");
            }
            //
            yield return new WaitForSeconds(1);
            VInput.LeftButtonClick();
            yield return new WaitForSeconds(1);
        }
        game.maximized = false;
    }
}