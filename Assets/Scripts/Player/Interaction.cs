using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    // ī�޶� �������� Ray�� �� 
    // �󸶳� ���� Ray�� ���� �����ϰ� ������ 

    public float checkRate = 0.5f;
    private float lastCheckTime;
    // �󸶳� �ָ� �ִ� �� üũ����
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        // ���� �ð����� ������ üũŸ���� �� ���� 0.5�ʺ��� ũ�ٸ� 
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            // ī�޶� �������� Ray�� �� Vector3 >> ���߾ӿ��� ����� 
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            // �ε��� ������ ��ƿ��� ���� 
            RaycastHit raycastHit;

            // ray ���� ����ְ� , �浹�� �� ��ü�� �ִ��� Ȯ��, ���� , layerMask ���صα� 
            if (Physics.Raycast(ray, out raycastHit, maxCheckDistance, layerMask))
            {
                if (raycastHit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = raycastHit.collider.gameObject;
                    // ��ȣ �ۿ�� �� 
                    curInteractable = raycastHit.collider.GetComponent<IInteractable>();

                    SetPromptText();
                }

            }
            else
            {
                // �ɸ��� ���� ������� ray�� ������ ��
                curInteractable = null;
                curInteractGameObject = null;
                promptText.gameObject.SetActive(false);

            }
        }

    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        // �۵��ϰ� ���� ��ȣ�ۿ�� ���� null �� �ƴҶ�
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // ItemObject ��ũ��Ʈ�� OnInteract
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
