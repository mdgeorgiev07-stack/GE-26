using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class MenuNavigation : MonoBehaviour
    {
        public Selectable DefaultSelection;

        private InputAction m_SubmitAction;
        private InputAction m_NavigateAction;
        
        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(null);

            m_SubmitAction = InputSystem.actions.FindAction("UI/Submit");
            m_NavigateAction  = InputSystem.actions.FindAction("UI/Navigate");
        }

        void LateUpdate()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (m_SubmitAction.WasPressedThisFrame()
                    || m_NavigateAction.ReadValue<Vector2>().sqrMagnitude != 0 )
                {
                    EventSystem.current.SetSelectedGameObject(DefaultSelection.gameObject);
                }
            }
        }
    }
}