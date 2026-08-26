using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Unity.FPS.UI
{
    public class ToggleGameObjectButton : MonoBehaviour
    {
        public GameObject ObjectToToggle;
        public bool ResetSelectionAfterClick;
        
        private InputAction m_CancelAction;

        void Start()
        {
            m_CancelAction = InputSystem.actions.FindAction("UI/Cancel");
            m_CancelAction.Enable();
        }

        void Update()
        {
            if (ObjectToToggle.activeSelf && m_CancelAction.WasPressedThisFrame())
            {
                SetGameObjectActive(false);
            }
        }

        public void SetGameObjectActive(bool active)
        {
            ObjectToToggle.SetActive(active);

            if (ResetSelectionAfterClick)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
}