using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Transform RoomPoint1;
    public Transform RoomPoint2;

    public string[] SceneStringArray;

    private void OnEnable()
    {
        StartCoroutine(LoadThisScene(SceneStringArray[0]));
    }
    IEnumerator LoadThisScene(string SceneName)
    {
        // 로드 옵션 설정
        LoadSceneMode loadSceneMode = LoadSceneMode.Additive; // 씬을 싱글 모드로 로드하거나, Additive 모드로 로드할 수 있습니다.

        // 비동기로 씬을 로드합니다.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName, loadSceneMode);

        // 로드가 완료될 때까지 대기합니다.
        while (!asyncOperation.isDone)
        {
            // 로드 상태를 확인하거나 필요한 다른 작업을 수행할 수 있습니다.
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 로드 진행 상태 (0.0 - 1.0)

            Debug.Log("Loading progress: " + (progress * 100) + "%");

            yield return null; // 한 프레임 기다립니다.
        }

        // 로드가 완료되면 추가 작업을 수행할 수 있습니다.
        Debug.Log("Scene loaded successfully");
    }

}

