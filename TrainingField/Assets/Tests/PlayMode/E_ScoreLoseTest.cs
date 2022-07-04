using System;
using System.Collections;
using WindowsInput;
using WindowsInput.Native;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class E_ScoreLoseTest
{
    private InputSimulator IS = new InputSimulator();
    private GameObject crosshair, timer, score, maxScore;
    private GameObject canvas, panel;
    [UnityTest, Order(0)]
    public IEnumerator ObjectsCheck()
    {
        SceneManager.LoadScene("Application");
        yield return null;
        canvas = GameObject.Find("Canvas");
        crosshair = PMHelper.Exist("Crosshair");
        if (crosshair == null)
        {
            Assert.Fail("There is no crosshair object on scene, named \"Crosshair\"");
        }
        if (!PMHelper.Child(crosshair, canvas))
        {
            Assert.Fail("\"Crosshair\" object should be a child of \"Canvas\" object");
        }

        RectTransform trCH = PMHelper.Exist<RectTransform>(crosshair);
        if (trCH == null)
        {
            Assert.Fail("There should be a <RectTransform> component on \"Crosshair\" object");
        }

        if (trCH.anchorMin.x != 0.5 || trCH.anchorMin.y != 0.5 || trCH.anchorMax.x != 0.5 || trCH.anchorMax.y != 0.5)
        {
            Assert.Fail("\"Crosshair\" object should be placed at center of screen, so anchors should equal 0.5");
        }
        if (PMHelper.Exist<Image>(crosshair) == null)
        {
            Assert.Fail("There should be an <Image> component on \"Crosshair\" object");
        }

        timer = PMHelper.Exist("Timer");
        if (timer == null)
        {
            Assert.Fail("There is no timer object on scene, named \"Timer\"");
        }
        if (!PMHelper.Child(timer, canvas))
        {
            Assert.Fail("\"Timer\" object should be a child of \"Canvas\" object");
        }
        RectTransform trTimer = PMHelper.Exist<RectTransform>(timer);
        if (trTimer == null)
        {
            Assert.Fail("There should be a <RectTransform> component on \"Timer\" object");
        }
        if (!PMHelper.CheckRectTransform(trTimer))
        {
            Assert.Fail("Anchors of \"Timer\"'s <RectTransform> component are incorrect or it's offsets" +
                        "are not equal to zero, might be troubles with different resolutions");
        }
        if (PMHelper.Exist<Image>(timer) == null)
        {
            Assert.Fail("There should be an <Image> component on \"Timer\" object");
        }
        
        score = PMHelper.Exist("Score");
        if (score == null)
        {
            Assert.Fail("There is no score object on scene, named \"Score\"");
        }
        if (!PMHelper.Child(score, canvas))
        {
            Assert.Fail("\"Score\" object should be a child of \"Canvas\" object");
        }
        RectTransform trScore = PMHelper.Exist<RectTransform>(score);
        if (trScore == null)
        {
            Assert.Fail("There should be a <RectTransform> component on \"Score\" object");
        }
        if (!PMHelper.CheckRectTransform(trScore))
        {
            Assert.Fail("Anchors of \"Score\"'s <RectTransform> component are incorrect or it's offsets" +
                        "are not equal to zero, might be troubles with different resolutions");
        }
        if (PMHelper.Exist<Text>(score) == null)
        {
            Assert.Fail("There should be a <Text> component on \"Score\" object");
        }
        yield return null;
        IS.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
        yield return null;
        panel = GameObject.Find("Panel");
        
        maxScore = PMHelper.Exist("MaxScore");
        if (maxScore == null)
        {
            Assert.Fail("There is no max score object on scene, named \"MaxScore\"");
        }
        if (!PMHelper.Child(maxScore, panel))
        {
            Assert.Fail("\"MaxScore\" object should be a child of \"Panel\" object");
        }
        RectTransform trMax = PMHelper.Exist<RectTransform>(maxScore);
        if (trMax == null)
        {
            Assert.Fail("There should be a <RectTransform> component on \"MaxScore\" object");
        }
        if (!PMHelper.CheckRectTransform(trMax))
        {
            Assert.Fail("Anchors of \"MaxScore\"'s <RectTransform> component are incorrect or it's offsets" +
                        "are not equal to zero, might be troubles with different resolutions");
        }
        if (PMHelper.Exist<Text>(maxScore) == null)
        {
            Assert.Fail("There should be a <Text> component on \"MaxScore\" object");
        }
    }
    
    [UnityTest, Order(1)]
    public IEnumerator TimerCheck()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Application");
        yield return null;
        Scene loaded = SceneManager.GetActiveScene();
        String loadedName = SceneManager.GetActiveScene().name;
        yield return new WaitForSeconds(1.5f);
        if (SceneManager.GetActiveScene() != loaded)
        {
            Assert.Fail("The timer must be at least 2 seconds long");
        }

        yield return new WaitForSeconds(3);
        if (SceneManager.GetActiveScene() == loaded)
        {
            Assert.Fail("The timer must be maximum 3 seconds long");
        }

        if (!SceneManager.GetActiveScene().name.Equals(loadedName))
        {
            Assert.Fail("Scene should be reloaded when the timer is over");
        }
        SceneManager.LoadScene("Application");
        yield return null;
        timer = GameObject.Find("Timer");
        Image timerImg = timer.GetComponent<Image>();
        if (timerImg.sprite == null)
        {
            Assert.Fail("\"Timer\"'s sprite is missing");
        }
        if (timerImg.type != Image.Type.Filled)
        {
            Assert.Fail("\"Timer\"'s image type should be \"filled\" in order to set it's fill amount");
        }

        float amWas = timerImg.fillAmount;
        
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (!(timerImg.fillAmount < amWas))
            {
                Assert.Fail("\"Timer\"'s image fill amount should decrease with time");
            }
            amWas = timerImg.fillAmount;
        }
    }

    [UnityTest, Order(2)]
    public IEnumerator TimerUpdateCheck()
    {
        SceneManager.LoadScene("Application");
        yield return null;
        GameObject player = GameObject.Find("Player");
        ShootScript ss = player.AddComponent<ShootScript>();
        Scene loaded = SceneManager.GetActiveScene();
        Time.timeScale = 2;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f);
            float amWas = GameObject.Find("Timer").GetComponent<Image>().fillAmount;
            GameObject target = PMHelper.FindObjectWithLayer("Target");
            yield return null;
            ss.shoot(target.transform.position);
            yield return new WaitForSeconds(0.2f);//I am change this 0.1 -> 0.2
            if (GameObject.Find("Timer").GetComponent<Image>().fillAmount < amWas)
            {
                Assert.Fail("Timer's fill amount should increase to maximum when new target was spawned");
            }
        }

        if (SceneManager.GetActiveScene() != loaded)
        {
            Assert.Fail("Timer should be updated when a new target spawns.");
        }
        yield return null;
    }
    
    [UnityTest, Order(3)]
    public IEnumerator ScoreTest()
    {
        SceneManager.LoadScene("Application");
        yield return new WaitForSeconds(2);
        GameObject player = GameObject.Find("Player");
        ShootScript ss = player.AddComponent<ShootScript>();
        Text score = GameObject.Find("Score").GetComponent<Text>();
        int was = 0;
        if (!score.text.Equals("0"))
        {
            Assert.Fail("After scene was loaded, the \"Score\" text should be \"0\"");
        }
        for (int i = 0; i < 5; i++)
        {
            yield return null;
            GameObject target = PMHelper.FindObjectWithLayer("Target");
            yield return null;
            ss.shoot(target.transform.position);
            yield return new WaitForSeconds(0.1f);
            int became;
            bool correct = int.TryParse(score.text, out became);
            if (!correct)
            {
                Assert.Fail("\"Score\"'s text should always be an integer value");
            }

            if (was + 10 != became)
            {
                Assert.Fail("When the target is shot, points should increase by 10");
            }
            was = became;
        }
        yield return null;
    }

    [UnityTest, Order(4)]
    public IEnumerator MaxScoreTest()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Application");
        yield return null;
        GameObject player = GameObject.Find("Player");
        ShootScript ss = player.AddComponent<ShootScript>();
        IS.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
        yield return null;
        Text maxScoreText = GameObject.Find("MaxScore").GetComponent<Text>();
        if (!maxScoreText.text.Equals("0"))
        {
            Assert.Fail("\"MaxScore\"'s text by default should be \"0\"");
        }
        IS.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
        yield return null;
        for (int i = 0; i < 3; i++)
        {
            yield return null;
            GameObject target = PMHelper.FindObjectWithLayer("Target");
            yield return null;
            ss.shoot(target.transform.position);
            yield return new WaitForSeconds(0.1f);
        }

        Time.timeScale = 3;
        yield return new WaitForSeconds(8);
        Time.timeScale = 1;
        
        SceneManager.LoadScene("Application");
        yield return new WaitForSeconds(0.25f);
        player = GameObject.Find("Player");
        ss = player.AddComponent<ShootScript>();
        IS.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
        yield return null;
        maxScoreText = GameObject.Find("MaxScore").GetComponent<Text>();
        if (!maxScoreText.text.Equals("30"))
        {
            Assert.Fail("\"MaxScore\"'s text should increase when you've earned more points");
        }
        IS.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
        yield return null;
        for (int i = 0; i < 2; i++)
        {
            yield return null;
            GameObject target = PMHelper.FindObjectWithLayer("Target");
            yield return null;
            ss.shoot(target.transform.position);
            yield return new WaitForSeconds(0.1f);
        }

        Time.timeScale = 3;
        yield return new WaitForSeconds(8);
        Time.timeScale = 1;
        
        SceneManager.LoadScene("Application");
        yield return null;
        IS.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
        yield return null;
        maxScoreText = GameObject.Find("MaxScore").GetComponent<Text>();
        if (!maxScoreText.text.Equals("30"))
        {
            Assert.Fail("\"MaxScore\"'s text should not change when you've earned less points");
        }
        IS.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
        yield return null;
    }
}