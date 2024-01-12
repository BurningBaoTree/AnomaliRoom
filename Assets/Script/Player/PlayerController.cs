#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ��� �Ⱥ����ֱ�
    bool ClassesShowCase = false;
    bool PlayerMoveOption = false;
    bool PlayerInteractiveOpint = false;

    // �ٸ� �׸���� ����
    bool PlayerMoveClass = false;
    bool PlayerInteractiveClass = false;

    PlayerMove moveClass;
    PlayerInteractive InteractiveClass;

    /// <summary>
    /// �ʱ� �غ� �Լ�
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
            // ��� ��ũ��Ʈ ����
            myTarget = (PlayerController)target;


            // �̸� ����ȭ�ϴ� ��ư
            if (GUILayout.Button("���� ����ȭ"))
            {
                InitializePlayer();
            }

            //�ڵ� Ȯ��
            myTarget.ClassesShowCase = EditorGUILayout.Foldout(myTarget.ClassesShowCase, "�÷��̾� ����");
            if (myTarget.ClassesShowCase)
            {
                EditorGUI.indentLevel++; // �鿩���� ���� ����

                // �߰� �׸�� ǥ��
                myTarget.PlayerMoveClass = EditorGUILayout.Toggle("PlayerMove", myTarget.PlayerMoveClass);
                if (!myTarget.PlayerMoveClass)
                {
                    EditorGUILayout.HelpBox("�÷��̾� �̵� �ڵ带 ã�� �� �����ϴ�.", MessageType.Warning);
                }
                else
                {
                    EditorGUI.indentLevel++;
                    myTarget.moveClass = (PlayerMove)EditorGUILayout.ObjectField("�÷��̾� �̵� �ڵ�", myTarget.moveClass, typeof(PlayerMove), true);
                    EditorGUI.indentLevel--;
                }
                myTarget.PlayerInteractiveClass = EditorGUILayout.Toggle("PlayerInteractive", myTarget.PlayerInteractiveClass);
                if (!myTarget.PlayerInteractiveClass)
                {
                    EditorGUILayout.HelpBox("�÷��̾� ��ȣ�ۿ� �ڵ带 ã�� �� �����ϴ�.", MessageType.Warning);
                }
                else
                {
                    EditorGUI.indentLevel++;
                    myTarget.moveClass = (PlayerMove)EditorGUILayout.ObjectField("��ȣ�ۿ� �ڵ�", myTarget.moveClass, typeof(PlayerMove), true);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--; // �鿩���� ���� ����
            }

            //�̵� ���� �ɼ�
            myTarget.PlayerMoveOption = EditorGUILayout.Foldout(myTarget.PlayerMoveOption, "�̵� ����");
            if (myTarget.PlayerMoveOption)
            {
                if (myTarget.moveClass != null)
                {
                    EditorGUI.indentLevel++;
                    myTarget.moveClass.cursorLock = EditorGUILayout.Toggle("Ŀ�� ��", myTarget.moveClass.cursorLock);
                    myTarget.moveClass.speed = EditorGUILayout.FloatField("�̵� �ӵ�", myTarget.moveClass.speed);
                    myTarget.moveClass.updonwAdd = EditorGUILayout.FloatField("�þ� ��鸲 �ӵ�", myTarget.moveClass.updonwAdd);
                    myTarget.moveClass.updonwImpulse = EditorGUILayout.FloatField("�þ� ��鸲 ������", myTarget.moveClass.updonwImpulse);
                    myTarget.moveClass.headrotationSpeed = EditorGUILayout.FloatField("�þ� ȸ�� �ӵ�", myTarget.moveClass.headrotationSpeed);
                    myTarget.moveClass.Cameratransform =
                        (Transform)EditorGUILayout.ObjectField("ī�޶� Ʈ������", myTarget.moveClass.Cameratransform, typeof(Transform), true);
                    myTarget.moveClass.CamUpdownTransform =
                        (Transform)EditorGUILayout.ObjectField("ķ ���ٿ� Ʈ������", myTarget.moveClass.CamUpdownTransform, typeof(Transform), true);
                    EditorGUI.indentLevel--;
                }
                else
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.HelpBox("�÷��̾� �̵� �ڵ尡 �������� �ʽ��ϴ�.", MessageType.Warning);
                    EditorGUI.indentLevel--;
                }
            }

            //��ȣ�ۿ� ���� �ɼ�
            myTarget.PlayerInteractiveOpint = EditorGUILayout.Foldout(myTarget.PlayerInteractiveOpint, "��ȣ�ۿ� ����");
            if (myTarget.PlayerInteractiveOpint)
            {
                if (myTarget.InteractiveClass != null)
                {

                }
                else
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.HelpBox("�÷��̾� ��ȣ�ۿ� �ڵ尡 �������� �ʽ��ϴ�.", MessageType.Warning);
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
