using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections.Generic;

[Description("If it doesn’t challenge you, it won’t change you."), Category("1")]
public class Stage1_Tests
{
    private GameObject player;
    private GameObject playerModel;
    private GameObject camera;
    private GameObject floor;
    private GameObject r_front, r_back, r_left, r_right, r_up;
    private List<RaycastHit> downCheck = new List<RaycastHit>();

    [UnityTest, Order(0)]
    public IEnumerator CheckObjects()
    {
        yield return null;
        if (!Application.CanStreamedLevelBeLoaded("Application"))
        {
            Assert.Fail("\"Application\" scene is misspelled or was not added to build settings");
        }

        SceneManager.LoadScene("Application");
        yield return null;

        player = GameObject.Find("Player");
        if (!player) Assert.Fail("There should be a \"Player\" object on scene");
        playerModel = GameObject.Find("PlayerModel");
        if (!playerModel) Assert.Fail("There should be a \"PlayerModel\" object on scene");
        camera = GameObject.Find("Main Camera");
        if (!camera) Assert.Fail("There should be a \"Main Camera\" object on scene");
        floor = GameObject.Find("Floor");
        if (!floor) Assert.Fail("There should be a \"Floor\" object on scene");

        if (!PMHelper.Child(playerModel, player))
        {
            Assert.Fail("\"PlayerModel\" object should be a child of \"Player\" object");
        }

        if (!PMHelper.Child(camera, player))
        {
            Assert.Fail("\"Camera\" object should be a child of \"Player\" object");
        }

        yield return null;
    }


    [UnityTest, Order(2)]
    public IEnumerator CheckBoundsPlaces()
    {
        RaycastHit hit = PMHelper.FindHit(player.transform.position, Vector3.down, "Floor");
        if (hit.collider != null)
        {
            floor = hit.collider.gameObject;
        }
        else
        {
            Assert.Fail("There should be a floor under the player with layer \"Floor\"");
        }

        Debug.DrawRay(hit.point, Vector3.down * 20, Color.cyan, 10);

        hit = PMHelper.FindHit(player.transform.position, Vector3.up, "Bounds");
        if (hit.collider != null)
        {
            r_up = hit.collider.gameObject;
        }
        else
        {
            Assert.Fail("There should be a ceiling above the player with layer \"Bounds\"");
        }

        Debug.DrawRay(hit.point, Vector3.up * 20, Color.cyan, 10);

        hit = PMHelper.FindHit(player.transform.position, Vector3.forward, "Bounds");
        if (hit.collider != null)
        {
            r_front = hit.collider.gameObject;
            if (Vector3.Angle(Vector3.forward, hit.normal) % 90 > 1)
            {
                Assert.Fail("Walls should be facing player and placed as a box");
            }
        }
        else
        {
            Assert.Fail("There should be a wall in front of the player with layer \"Bounds\"");
        }

        downCheck.Add(hit);

        hit = PMHelper.FindHit(player.transform.position, Vector3.back, "Bounds");
        if (hit.collider != null)
        {
            r_back = hit.collider.gameObject;
            if (Vector3.Angle(Vector3.forward, hit.normal) % 90 > 1)
            {
                Assert.Fail("Walls should be facing player and placed as a box");
            }
        }
        else
        {
            Assert.Fail("There should be a wall behind the player with layer \"Bounds\"");
        }

        downCheck.Add(hit);

        hit = PMHelper.FindHit(player.transform.position, Vector3.left, "Bounds");
        if (hit.collider != null)
        {
            r_left = hit.collider.gameObject;
            if (Vector3.Angle(Vector3.forward, hit.normal) % 90 > 1)
            {
                Assert.Fail("Walls should be facing player and placed as a box");
            }
        }
        else
        {
            Assert.Fail("There should be a wall to the left side of the player with layer \"Bounds\"");
        }

        downCheck.Add(hit);

        hit = PMHelper.FindHit(player.transform.position, Vector3.right, "Bounds");
        if (hit.collider != null)
        {
            r_right = hit.collider.gameObject;
            if (Vector3.Angle(Vector3.forward, hit.normal) % 90 > 1)
            {
                Assert.Fail("Walls should be facing player and placed as a box");
            }
        }
        else
        {
            Assert.Fail("There should be a wall to the right side of the player with layer \"Bounds\"");
        }

        downCheck.Add(hit);

        foreach (RaycastHit v in downCheck)
        {
            RaycastHit new1 = PMHelper.FindHit(v.point, new Vector3(v.normal.z, 0, v.normal.x), "Bounds");
            RaycastHit new2 = PMHelper.FindHit(v.point, new Vector3(-v.normal.z, 0, -v.normal.x), "Bounds");

            if (PMHelper.FindHit(v.point, Vector3.down, "Floor").collider == null)
            {
                Assert.Fail("Player shouldn't be able to fall down in the void");
            }

            if (PMHelper.FindHit(v.point, Vector3.up, "Bounds").collider == null)
            {
                Assert.Fail("There should not be empty spaces in ceiling");
            }

            if (new1.collider == null)
            {
                Assert.Fail("There should not be empty spaces in walls");
            }

            if (new2.collider == null)
            {
                Assert.Fail("There should not be empty spaces in walls");
            }

            if (PMHelper.FindHit(new1.point, Vector3.down, "Floor").collider == null)
            {
                Assert.Fail("Player shouldn't be able to fall down in the void");
            }

            if (PMHelper.FindHit(new1.point, Vector3.up, "Bounds").collider == null)
            {
                Assert.Fail("There should not be empty spaces in ceiling");
            }

            if (PMHelper.FindHit(new2.point, Vector3.down, "Floor").collider == null)
            {
                Assert.Fail("Player shouldn't be able to fall down in the void");
            }

            if (PMHelper.FindHit(new2.point, Vector3.up, "Bounds").collider == null)
            {
                Assert.Fail("There should not be empty spaces in ceiling");
            }

            Debug.DrawRay(v.point, Vector3.down * 20, Color.cyan, 10);
            Debug.DrawRay(v.point, Vector3.up * 20, Color.cyan, 10);
            Debug.DrawRay(v.point, new Vector3(v.normal.z, 0, v.normal.x) * 20, Color.cyan, 10);
            Debug.DrawRay(v.point, new Vector3(-v.normal.z, 0, -v.normal.x) * 20, Color.cyan, 10);
            Debug.DrawRay(new1.point, Vector3.down * 20, Color.cyan, 10);
            Debug.DrawRay(new2.point, Vector3.up * 20, Color.cyan, 10);
            Debug.DrawRay(new2.point, Vector3.down * 20, Color.cyan, 10);
            Debug.DrawRay(new1.point, Vector3.up * 20, Color.cyan, 10);
        }

        //yield return new WaitForSeconds(1);
        yield return null;
    }

    [UnityTest, Order(3)]
    public IEnumerator CheckObjectsComponents()
    {
        MeshFilter mf = PMHelper.Exist<MeshFilter>(playerModel);
        MeshRenderer mr = PMHelper.Exist<MeshRenderer>(playerModel);
        Collider col = PMHelper.Exist<Collider>(playerModel);
        if (mf == null)
        {
            Assert.Fail("There should be enabled \"MeshFilter\" component on \"PlayerModel\"'s object");
        }

        if (mr == null || !mr.enabled)
        {
            Assert.Fail("There should be enabled \"MeshRenderer\" component on \"PlayerModel\"'s object");
        }

        if (col != null)
        {
            Assert.Fail("There should not be any \"Collider\" component on \"PlayerModel\"'s object," +
                        " because \"CharacterController\" (one that we will add later) is already acting as a collider");
        }

        Camera cam = PMHelper.Exist<Camera>(camera);
        if (cam == null || !cam.enabled)
        {
            Assert.Fail("There should be enabled \"Camera\" component on \"Main Camera\"'s object");
        }

        List<GameObject> objects = new List<GameObject> {r_back, r_front, r_left, r_right, r_up, floor};
        foreach (GameObject g in objects.ToArray())
        {
            mf = PMHelper.Exist<MeshFilter>(g);
            mr = PMHelper.Exist<MeshRenderer>(g);
            col = PMHelper.Exist<Collider>(g);
            if (mf == null)
            {
                Assert.Fail("All \"Bounds\" and \"Floor\" objects should have enabled \"MeshFilter\" component");
            }

            if (mr == null || !mr.enabled)
            {
                Assert.Fail("All \"Bounds\" and \"Floor\" objects should have enabled \"MeshRenderer\" component");
            }

            if (col == null || !col.enabled)
            {
                Assert.Fail("All \"Bounds\" and \"Floor\" objects should have enabled \"Collider\" component");
            }
        }

        yield return null;
    }
}