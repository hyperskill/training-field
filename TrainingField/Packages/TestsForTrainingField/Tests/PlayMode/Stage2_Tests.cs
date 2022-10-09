using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[Description("I may win and I may lose, but I will never be defeated"), Category("2")]
public class Stage2_Tests
{
    private GameObject player, camera;

    [UnityTest, Order(0)]
    public IEnumerator CheckLooking()
    {
        SceneManager.LoadScene("Application");
        yield return null;
        player = GameObject.Find("Player");
        CharacterController cc = PMHelper.Exist<CharacterController>(player);
        if (cc == null || !cc.enabled)
        {
            Assert.Fail("There should be enabled \"CharacterController\" component on \"Player\"'s object");
        }
        camera = GameObject.Find("Main Camera");
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Assert.Fail("Cursor's lock mode should be set as \"locked\" to make cursor disappear from screen");
        }

        float xRotWas = camera.transform.rotation.eulerAngles.x;
        float xRotCur;
        while(xRotWas<90)
        {
            VInput.MoveMouseBy(0, 100);
            yield return null;
            xRotCur = camera.transform.rotation.eulerAngles.x;
            if (!(xRotCur > xRotWas))
            {
                Assert.Fail("Mouse down-movement should increase \"Camera\"'s x-axis rotation");
            }
            xRotWas = xRotCur;
        }
        VInput.MoveMouseBy(0, 100);
        yield return null;
        if (xRotWas > 90)
        {
            Assert.Fail("\"Camera\"'s x-axis rotation should be clamped between -90 and 90 degrees");
        }
        
        
        
        SceneManager.LoadScene("Application");
        yield return null;
        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
        VInput.MoveMouseBy(0, -100);
        yield return null;
        xRotWas = camera.transform.rotation.eulerAngles.x;

        while(xRotWas>270)
        {
            VInput.MoveMouseBy(0, -100);
            yield return null;
            xRotCur = camera.transform.rotation.eulerAngles.x;

            if (!(xRotCur < xRotWas))
            {
                Debug.Log(xRotCur);
                Assert.Fail("Mouse up-movement should decrease \"Camera\"'s x-axis rotation");
            }
            xRotWas = xRotCur;
        }
        VInput.MoveMouseBy(0, -100);
        yield return null;
        if (xRotWas < 270)
        {
            Assert.Fail("\"Camera\"'s x-axis rotation should be clamped between -90 and 90 degrees");
        }
        
        SceneManager.LoadScene("Application");
        yield return null;
        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
        xRotWas = player.transform.rotation.y;
        for (int i = 0; i < 5; i++)
        {
            VInput.MoveMouseBy(100, 0);
            yield return null;
            xRotCur = player.transform.rotation.y;
            if (xRotCur < xRotWas)
            {
                Assert.Fail("Mouse right-movement should decrease \"Player\"'s y-axis rotation");
            }

            xRotWas = xRotCur;
        }
        for (int i = 0; i < 5; i++)
        {
            VInput.MoveMouseBy(-100, 0);
            yield return null;
            xRotCur = player.transform.rotation.y;
            if (xRotCur > xRotWas)
            {
                Assert.Fail("Mouse left-movement should increase \"Player\"'s y-axis rotation");
            }
            xRotWas = xRotCur;
        }
    }

    [UnityTest, Order(1)]
    public IEnumerator CheckMovement()
    {
        SceneManager.LoadScene("Application");
        Time.timeScale = 20;
        yield return new WaitForSeconds(2);
        player = GameObject.Find("Player");
        
        Vector3 startPos = player.transform.position;

        VInput.KeyDown(KeyCode.D);
        yield return new WaitForSeconds(1);
        VInput.KeyUp(KeyCode.D);
        yield return null;
        Vector3 curPos = player.transform.position;
        if (!(startPos.z == curPos.z && startPos.x < curPos.x))
        {
            Assert.Fail("Right-movement not working properly");
        }

        startPos = player.transform.position;
        VInput.KeyDown(KeyCode.A);
        yield return new WaitForSeconds(1);
        VInput.KeyUp(KeyCode.A);
        yield return null;
        curPos = player.transform.position;
        if (!(startPos.z == curPos.z && startPos.x > curPos.x))
        {
            Assert.Fail("Left-movement not working properly");
        }
        
        SceneManager.LoadScene("Application");
        yield return new WaitForSeconds(2);
        player = GameObject.Find("Player");
        
        startPos = player.transform.position;
        VInput.KeyDown(KeyCode.W);
        yield return new WaitForSeconds(1);
        VInput.KeyUp(KeyCode.W);
        yield return null;
        curPos = player.transform.position;
        if (!(startPos.z < curPos.z && startPos.x == curPos.x))
        {
            Debug.Log(startPos.x);
            Debug.Log(curPos.x);
            Assert.Fail("Forward-movement not working properly");
        }
        
        startPos = player.transform.position;
        VInput.KeyDown(KeyCode.S);
        yield return new WaitForSeconds(1);
        VInput.KeyUp(KeyCode.S);
        yield return null;
        curPos = player.transform.position;
        if (!(startPos.z > curPos.z && startPos.x == curPos.x))
        {
            Assert.Fail("Backward-movement not working properly");
        }
        yield return null;
    }
    
    [UnityTest, Order(2)]
    public IEnumerator CheckJump()
    {
        SceneManager.LoadScene("Application");
        yield return new WaitForSeconds(2);
        player = GameObject.Find("Player");
        
        Vector3 startPos = player.transform.position;

        VInput.KeyPress(KeyCode.Space);
        yield return new WaitForSeconds(0.2f);
        
        Vector3 curPos = player.transform.position;
        
        if (curPos.Equals(startPos))
        {
            Assert.Fail("\"Player\" should be able to jump and it's jump should last more than 0.5f seconds");
        }
        
        yield return new WaitForSeconds(1.5f);
        
        curPos = player.transform.position;

        if (curPos!=startPos)
        {
            Assert.Fail("\"Player\" should be staying still after landing and it's jump should last no longer than 2 seconds");
        }
        
        RaycastHit[] hit = Physics.RaycastAll(player.transform.position, Vector3.down);
        foreach (var h in hit)
        {
            GameObject.Destroy(h.collider.gameObject);
        }
        curPos = player.transform.position;
        yield return new WaitForSeconds(2);
        float firstDelta = player.transform.position.y - curPos.y;
        curPos = player.transform.position;
        yield return new WaitForSeconds(2);
        float secondDelta = player.transform.position.y - curPos.y;
        if (Mathf.Abs(secondDelta) <= Mathf.Abs(firstDelta))
        {
            Assert.Fail("Gravity should be applied to player's y-axis velocity");
        }
    }
}