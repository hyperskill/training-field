using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Description(""), Category("3")]
public class Stage3_Tests
{
    private GameObject player, camera;
    private GameObject canvas, panel, sensSlider, speedSlider, eventSystem;

    [UnityTest, Order(0)]
    public IEnumerator ObjectsCheck()
    {
        SceneManager.LoadScene("Application");
        yield return null;
        Time.timeScale = 1;
        
        if (Time.timeScale != 1)
        {
            Debug.Log(Time.timeScale);
            Assert.Fail("Time scale should be equal to 1 when scene is loaded");
        }
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Assert.Fail("Cursor should be in \"Locked\" CursorLockMode when scene is loaded");
        }
        
        canvas = GameObject.Find("Canvas");
        if(canvas == null) Assert.Fail("There should be canvas, named \"Canvas\" on scene");
        if(PMHelper.Exist<Canvas>(canvas) == null) Assert.Fail("There is no basic component <Canvas> on \"Canvas\" object");
        if(PMHelper.Exist<CanvasScaler>(canvas) == null) Assert.Fail("There is no basic component <CanvasScaler> on \"Canvas\" object");
        if(PMHelper.Exist<GraphicRaycaster>(canvas) == null) Assert.Fail("There is no basic component <GraphicRaycaster> on \"Canvas\" object");
        
        eventSystem = GameObject.Find("EventSystem");
        if(eventSystem == null) Assert.Fail("There should be event system, named \"EventSystem\" on scene in order to detect events");
        if(PMHelper.Exist<EventSystem>(eventSystem) == null) Assert.Fail("There is no basic component <EventSystem> on \"EventSystem\" object");
        if(PMHelper.Exist<StandaloneInputModule>(eventSystem) == null) Assert.Fail("There is no basic component <StandaloneInputModule> on \"Canvas\" object");

        panel = GameObject.Find("Panel");

        if (panel != null)
        {
            Assert.Fail("There should be no panel object when a scene has been loaded, " +
                        "because it should only become active in pause for settings");
        }

        VInput.KeyPress(KeyCode.Escape);
        yield return null;
        
        if (Time.timeScale != 0)
        {
            Assert.Fail("Time scale should be equal to 0 when app is paused for settings");
        }

        Time.timeScale = 1;
        if (Cursor.lockState != CursorLockMode.None)
        {
            Assert.Fail("Cursor should be in \"None\" CursorLockMode when app is paused for settings");
        }
        
        panel = GameObject.Find("Panel");

        if(panel == null) Assert.Fail("There should be panel object, after Escape-key was pressed," +
                                      " named \"Panel\", where settings would be displayed");
        if(PMHelper.Exist<RectTransform>(panel)==null) Assert.Fail("There is no basic component <RectTransform> on \"Panel\" object");
        if(PMHelper.Exist<CanvasRenderer>(panel)==null) Assert.Fail("There is no basic component <CanvasRenderer> on \"Panel\" object");
        if(PMHelper.Exist<Image>(panel)==null) Assert.Fail("There is no basic component <Image> on \"Panel\" object");
        if (!PMHelper.CheckRectTransform(panel.GetComponent<RectTransform>()))
        {
            Assert.Fail("Anchors of \"Panel\"'s <RectTransform> component are incorrect or it's offsets" +
                        "are not equal to zero, might be troubles with different resolutions");
        }

        sensSlider = GameObject.Find("SensSlider");
        if(sensSlider == null) Assert.Fail("There should be slider object, named \"SensSlider\", that will be changing mouse sensitivity");
        if(PMHelper.Exist<RectTransform>(sensSlider) == null) Assert.Fail("There is no basic component <RectTransform> on \"SensSlider\" object");
        Slider s = PMHelper.Exist<Slider>(sensSlider); 
        if(s == null) Assert.Fail("There is no basic component <Slider> on \"SensSlider\" object");
        if (!s.interactable)
        {
            Assert.Fail("<Slider>'s \"Interactable\" field should be checked on \"SensSlider\" object");
        }
        if (!(s.minValue > 0.2 && s.minValue < 0.5))
        {
            Assert.Fail("Set <Slider>'s \"Min Value\" as any value between 0.2 and 0.5 in order to set mouse " +
                        "sensitivity in range of [minValue*default, default]");
        }
        if (!(s.maxValue == 1))
        {
            Assert.Fail("Set <Slider>'s \"Max Value\" as 1, so that mouse sensitivity will be " +
                        "in range of [minValue*default,default]");
        }
        if (!(s.value == 1))
        {
            Assert.Fail("Set <Slider>'s \"Value\" as 1 by default, so that user would be able to decrease sensitivity");
        }
        if (s.onValueChanged.GetPersistentEventCount() != 1)
        {
            Assert.Fail("There should be added one listener to <Slider>'s \"On Value Changed\" on \"SensSlider\" object");
        }

        speedSlider = GameObject.Find("SpeedSlider");
        if(speedSlider == null) Assert.Fail("There should be slider object, named \"SpeedSlider\", that will be changing player's speed");
        if(PMHelper.Exist<RectTransform>(speedSlider) == null) Assert.Fail("There is no basic component <RectTransform> on \"SpeedSlider\" object");
        s = PMHelper.Exist<Slider>(speedSlider); 
        if(s == null) Assert.Fail("There is no basic component <Slider> on \"SpeedSlider\" object");
        if (!s.interactable)
        {
            Assert.Fail("<Slider>'s \"Interactable\" field should be checked on \"SpeedSlider\" object");
        }
        if (!(s.minValue > 0.2 && s.minValue < 0.5))
        {
            Assert.Fail("Set <Slider>'s \"Min Value\" as any value between 0.2 and 0.5 in order to set player's " +
                        "speed in range of [minValue*default, default]");
        }
        if (!(s.maxValue == 1))
        {
            Assert.Fail("Set <Slider>'s \"Max Value\" as 1, so that player's speed will be " +
                        "in range of [minValue*default,default]");
        }
        if (!(s.value == 1))
        {
            Assert.Fail("Set <Slider>'s \"Value\" as 1 by default, so that user would be able to decrease player's speed");
        }
        if (s.onValueChanged.GetPersistentEventCount() != 1)
        {
            Assert.Fail("There should be added one listener to <Slider>'s \"On Value Changed\" on \"SpeedSlider\" object");
        }


        if (!PMHelper.Child(panel, canvas))
        {
            Assert.Fail("\"Panel\" object should be a child of \"Canvas\" object as a UI element");
        }
        if (!PMHelper.Child(sensSlider, panel))
        {
            Assert.Fail("\"SensSlider\" object should be a child of \"Panel\" object");
        }
        if (!PMHelper.Child(speedSlider, panel))
        {
            Assert.Fail("\"SensSlider\" object should be a child of \"Panel\" object");
        }

        VInput.KeyPress(KeyCode.Escape);
        yield return null;
        
        panel = GameObject.Find("Panel");
        if (panel != null)
        {
            Assert.Fail("There should be no active panel object when app is not in pause for settings");
        }
        if (Time.timeScale != 1)
        {
            Assert.Fail("Time scale should be equal to 1 when app is not paused for settings");
        }
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Assert.Fail("Cursor should be in \"Locked\" CursorLockMode when app is not paused for settings");
        }
    }

    [UnityTest, Order(1)]
    public IEnumerator CheckAction()
    {
        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
        yield return null;
        
        //Check settings changing

        Vector3 start = player.transform.position;
        VInput.KeyDown(KeyCode.A);
        yield return new WaitForSeconds(0.5f);
        Vector3 mid = player.transform.position;
        VInput.KeyUp(KeyCode.A);
        yield return null;
        float dist = Vector3.Distance(start,mid);
        
        speedSlider.GetComponent<Slider>().value = speedSlider.GetComponent<Slider>().minValue;
        VInput.KeyDown(KeyCode.A);
        yield return new WaitForSeconds(0.5f);
        Vector3 end2 = player.transform.position;
        VInput.KeyUp(KeyCode.A);
        yield return null;
        float dist2 = Vector3.Distance(mid,end2);
        if (dist <= dist2)
        {
            Assert.Fail("Decreasing speed slider value should decrease player's speed");
        }
        
        float rotStart = camera.transform.rotation.eulerAngles.y;
        VInput.MoveMouseBy(1000, 0);
        yield return null;
        
        float rotMid = camera.transform.rotation.eulerAngles.y;
        
        sensSlider.GetComponent<Slider>().value = sensSlider.GetComponent<Slider>().minValue;
        VInput.MoveMouseBy(1000, 0);
        yield return null;
        float rotEnd = camera.transform.rotation.eulerAngles.y;
        
        if (Mathf.Abs(rotMid-rotStart) <= Mathf.Abs(rotEnd-rotMid)*1.5)
        {
            Assert.Fail("Decreasing sensitivity slider value should decrease in-game mouse sensitivity");
        }
    }
}
