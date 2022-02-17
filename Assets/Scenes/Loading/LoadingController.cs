using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    private bool finish = false;
    private AsyncOperation asyncLoad;

    private string PressAnyKeyString = "按任意键继续！";

    private string[] messages = new string[11] {
        "时间，转瞬即逝，不复再来。",
        "夫天地者，万物之逆旅也。",
        "时间，转瞬即逝，不复再来。",
        "少年易老学难成，一寸光阴不可轻。",
        "草木也知愁，韶华竟白头。",
        "少年辛苦终身事，莫向光阴惰寸功。",
        "读书不觉已春深，一寸光阴一寸金。",
        "仰天大笑出门去，我辈岂是蓬蒿人。",
        "长风破浪会有时，直挂云帆济沧海。",
        "书山有路勤为径，学海无涯苦作舟。",
        "千磨万击还坚劲，任尔东西南北风。"
    };

    public Text PressAnyKey;

    // Start is called before the first frame update
    void Start()
    {
        if (this.PressAnyKey == null)
        {
            Debug.LogError("Please select the text box.");
            return;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        this.finish = false;

        this.PressAnyKey.text = string.Empty;
        StartCoroutine(slideshow());

        var nextSceneName = LoadingHelper.Instance.GetNextSceneName();
        StartCoroutine(loadScene(nextSceneName));
    }

    void Update()
    {
        pressAnyKey();
    }

    private IEnumerator loadScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Please input the next scene name.");
            yield return null;
        }
        else
        {
            this.asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            this.asyncLoad.allowSceneActivation = false;

            while (this.asyncLoad.progress < 0.9f)
            {
                yield return new WaitForSeconds(15);//fake
                //yield return new WaitForEndOfFrame();
            }

            this.finish = true;
            this.PressAnyKey.text = PressAnyKeyString;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator slideshow()
    {
        while (!this.finish)
        {
            int n = (int)(Random.value * 10);
            var message = messages[n];
            this.PressAnyKey.text = message;

            yield return new WaitForSeconds(5);
        }
    }

    private void pressAnyKey()
    {
        if (this.finish
            && (
            (Gamepad.current != null && Gamepad.current.aButton.isPressed)
            || (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
            )
        )
        {
            this.asyncLoad.allowSceneActivation = true;
        }
    }
}
