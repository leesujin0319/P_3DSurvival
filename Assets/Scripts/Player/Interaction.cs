using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    // 카메라를 기준으로 Ray를 쏨 
    // 얼마나 자주 Ray를 쏴서 검출하게 만들지 

    public float checkRate = 0.5f;
    private float lastCheckTime;
    // 얼마나 멀리 있는 걸 체크할지
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
        // 현재 시간에서 마지막 체크타임을 뺀 것이 0.5초보다 크다면 
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            // 카메라 기준으로 Ray를 쏨 Vector3 >> 정중앙에서 쏘려고 
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            // 부딪힌 정보를 담아오는 변수 
            RaycastHit raycastHit;

            // ray 정보 담아주고 , 충돌이 된 물체가 있는지 확인, 길이 , layerMask 정해두기 
            if (Physics.Raycast(ray, out raycastHit, maxCheckDistance, layerMask))
            {
                if (raycastHit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = raycastHit.collider.gameObject;
                    // 상호 작용된 걸 
                    curInteractable = raycastHit.collider.GetComponent<IInteractable>();

                    SetPromptText();
                }

            }
            else
            {
                // 걸린게 없고 빈공간에 ray를 쏴줬을 때
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
        // 작동하고 현재 상호작용된 것이 null 이 아닐때
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // ItemObject 스크립트에 OnInteract
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
