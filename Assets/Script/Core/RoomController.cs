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
        // �ε� �ɼ� ����
        LoadSceneMode loadSceneMode = LoadSceneMode.Additive; // ���� �̱� ���� �ε��ϰų�, Additive ���� �ε��� �� �ֽ��ϴ�.

        // �񵿱�� ���� �ε��մϴ�.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName, loadSceneMode);

        // �ε尡 �Ϸ�� ������ ����մϴ�.
        while (!asyncOperation.isDone)
        {
            // �ε� ���¸� Ȯ���ϰų� �ʿ��� �ٸ� �۾��� ������ �� �ֽ��ϴ�.
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // �ε� ���� ���� (0.0 - 1.0)

            Debug.Log("Loading progress: " + (progress * 100) + "%");

            yield return null; // �� ������ ��ٸ��ϴ�.
        }

        // �ε尡 �Ϸ�Ǹ� �߰� �۾��� ������ �� �ֽ��ϴ�.
        Debug.Log("Scene loaded successfully");
    }

}

