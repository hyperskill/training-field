using System;
using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Description("It never gets easier, you just get better."), Category("6")]
public class Stage6_Tests
{
    private GameObject score;

    [UnityTest, Order(0)]
    public IEnumerator RaycastCheck()
    {
        Time.timeScale = 5;
        SceneManager.LoadScene("Application");
        yield return new WaitForSeconds(2);
        GameObject player = GameObject.Find("Player");
        ShootScript ss = player.AddComponent<ShootScript>();
        Text score = GameObject.Find("Score").GetComponent<Text>();
        int was = 0;
        for (int i = 0; i < 20; i++)
        {
            yield return null;
            GameObject target = PMHelper.FindObjectWithLayer("Target");
            Bounds bounds = target.GetComponent<Collider>().bounds;

            float x=0, y=0, z=0, radius=0;
            if (bounds.size.x > bounds.size.y && bounds.size.z > bounds.size.y)
            {
                x = Random.Range(bounds.center.x - bounds.size.x / 2, bounds.center.x + bounds.size.x / 2);
                y = bounds.center.y;
                z = Random.Range(bounds.center.z - bounds.size.z / 2, bounds.center.z + bounds.size.z / 2);
                radius = bounds.size.x/2;
            }else if (bounds.size.y > bounds.size.x && bounds.size.z > bounds.size.x)
            {
                x = bounds.center.x;
                y = Random.Range(bounds.center.y - bounds.size.y / 2, bounds.center.y + bounds.size.y / 2);
                z = Random.Range(bounds.center.z - bounds.size.z / 2, bounds.center.z + bounds.size.z / 2);
                radius = bounds.size.z/2;
            }else if (bounds.size.y > bounds.size.z && bounds.size.x > bounds.size.z)
            {
                x = Random.Range(bounds.center.x - bounds.size.x / 2, bounds.center.x + bounds.size.x / 2);
                y = Random.Range(bounds.center.y - bounds.size.y / 2, bounds.center.y + bounds.size.y / 2);
                z = bounds.center.z;
                radius = bounds.size.x/2;
            }
            Vector3 where = new Vector3(x, y, z);
            yield return null;
            ss.shoot(where, true);
            yield return new WaitForSeconds(0.1f);
            RaycastHit[] hit = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit exact = Array.Find(hit, e => e.collider.gameObject.layer == LayerMask.NameToLayer("Target"));
            VInput.LeftButtonClick();
            yield return new WaitForSeconds(0.1f);
            float dist = Vector3.Distance(exact.point, bounds.center);

            int points;
            if (dist > radius)
            {
                points = 1;
            }else if(dist/radius>2f/3f)
            {
                points = 3;
            }else if(dist/radius>1f/6f)
            {
                points = 5;
            }else
            {
                points = 10;
            }
            int became;
            bool correct = int.TryParse(score.text, out became);
            if (!correct)
            {
                Assert.Fail("\"Score\"'s text should always be an integer value");
            }

            if (became-was!=points)
            {
                Assert.Fail("When the target is shot, points should increase by the value mentioned in task");
            }
            was = became;
        }
        yield return null;
    }
}