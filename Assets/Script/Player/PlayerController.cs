#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 접어서 안보여주기
    bool ClassesShowCase = false;
    bool PlayerMoveOption = false;
    bool PlayerInteractiveOpint = false;

    // 다른 항목들을 선언
    bool PlayerMoveClass = false;
    bool PlayerInteractiveClass = false;

    PlayerMove moveClass;
    PlayerInteractive InteractiveClass;

    /// <summary>
    /// 초기 준비 함수
    /// </summary>
    void InitializePlayer()
    {
        moveClass = GetComponent<PlayerMove>();
        InteractiveClass = GetComponent<PlayerInteractive>();

        if (moveClass != null)
        {
            PlayerMoveClass = true;
            moveClass.controller = this;
        }
        if (InteractiveClass != null)
        {
            PlayerInteractiveClass = true;
            InteractiveClass.controller = this;
        }
    }

    private void Awake()
    {
        InitializePlayer();
    }
    private void OnEnable()
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerController))]
    public class FoldoutExampleEditor : Editor
    {
        PlayerController myTarget;
        public override void OnInspectorGUI()
        {
            // 대상 스크립트 참조
            myTarget = (PlayerController)target;


            // 미리 동기화하는 버튼
            if (GUILayout.Button("시험 동기화"))
            {
                InitializePlayer();
            }

            //코드 확인
            myTarget.ClassesShowCase = EditorGUILayout.Foldout(myTarget.ClassesShowCase, "플레이어 제어");
            if (myTarget.ClassesShowCase)
            {
                EditorGUI.indentLevel++; // 들여쓰기 레벨 증가

                // 추가 항목들 표시
                myTarget.PlayerMoveClass = EditorGUILayout.Toggle("PlayerMove", myTarget.PlayerMoveClass);
                if (!myTarget.PlayerMoveClass)
                {
                    EditorGUILayout.HelpBox("플레이어 이동 코드를 찾을 수 없습니다.", MessageType.Warning);
                }
                else
                {
                    EditorGUI.indentLevel++;
                    myTarget.moveClass = (PlayerMove)EditorGUILayout.ObjectField("플레이어 이동 코드", myTarget.moveClass, typeof(PlayerMove), true);
                    EditorGUI.indentLevel--;
                }
                myTarget.PlayerInteractiveClass = EditorGUILayout.Toggle("PlayerInteractive", myTarget.PlayerInteractiveClass);
                if (!myTarget.PlayerInteractiveClass)
                {
                    EditorGUILayout.HelpBox("플레이어 상호작용 코드를 찾을 수 없습니다.", MessageType.Warning);
                }
                else
                {
                    EditorGUI.indentLevel++;
                    myTarget.moveClass = (PlayerMove)EditorGUILayout.ObjectField("상호작용 코드", myTarget.moveClass, typeof(PlayerMove), true);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--; // 들여쓰기 레벨 감소
            }

            //이동 제어 옵션
            myTarget.PlayerMoveOption = EditorGUILayout.Foldout(myTarget.PlayerMoveOption, "이동 제어");
            if (myTarget.PlayerMoveOption)
            {
                if (myTarget.moveClass != null)
                {
                    EditorGUI.indentLevel++;
                    myTarget.moveClass.cursorLock = EditorGUILayout.Toggle("커서 락", myTarget.moveClass.cursorLock);
                    myTarget.moveClass.speed = EditorGUILayout.FloatField("이동 속도", myTarget.moveClass.speed);
                    myTarget.moveClass.updonwAdd = EditorGUILayout.FloatField("시야 흔들림 속도", myTarget.moveClass.updonwAdd);
                    myTarget.moveClass.updonwImpulse = EditorGUILayout.FloatField("시야 흔들림 높낮이", myTarget.moveClass.updonwImpulse);
                    myTarget.moveClass.headrotationSpeed = EditorGUILayout.FloatField("시야 회전 속도", myTarget.moveClass.headrotationSpeed);
                    myTarget.moveClass.Cameratransform =
                        (Transform)EditorGUILayout.ObjectField("카메라 트랜스폼", myTarget.moveClass.Cameratransform, typeof(Transform), true);
                    myTarget.moveClass.CamUpdownTransform =
                        (Transform)EditorGUILayout.ObjectField("캠 업다운 트랜스폼", myTarget.moveClass.CamUpdownTransform, typeof(Transform), true);
                    EditorGUI.indentLevel--;
                }
                else
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.HelpBox("플레이어 이동 코드가 존재하지 않습니다.", MessageType.Warning);
                    EditorGUI.indentLevel--;
                }
            }

            //상호작용 제어 옵션
            myTarget.PlayerInteractiveOpint = EditorGUILayout.Foldout(myTarget.PlayerInteractiveOpint, "상호작용 제어");
            if (myTarget.PlayerInteractiveOpint)
            {
                if (myTarget.InteractiveClass != null)
                {

                }
                else
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.HelpBox("플레이어 상호작용 코드가 존재하지 않습니다.", MessageType.Warning);
                    EditorGUI.indentLevel--;
                }
            }
        }
        void InitializePlayer()
        {
            myTarget.InitializePlayer();
        }
    }
#endif
}
