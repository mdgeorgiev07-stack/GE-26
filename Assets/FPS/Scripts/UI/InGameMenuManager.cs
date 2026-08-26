using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class InGameMenuManager : MonoBehaviour
    {
        [Tooltip("Root GameObject of the menu used to toggle its activation")]
        public GameObject MenuRoot;

        [Tooltip("Master volume when menu is open")] [Range(0.001f, 1f)]
        public float VolumeWhenMenuOpen = 0.5f;

        [Tooltip("Slider component for look sensitivity")]
        public Slider LookSensitivitySlider;

        [Tooltip("Toggle component for shadows")]
        public Toggle ShadowsToggle;

        [Tooltip("Toggle component for invincibility")]
        public Toggle InvincibilityToggle;

        [Tooltip("Toggle component for framerate display")]
        public Toggle FramerateToggle;

        [Tooltip("GameObject for the controls")]
        public GameObject ControlImage;

        PlayerInputHandler m_PlayerInputsHandler;
        Health m_PlayerHealth;
        FramerateCounter m_FramerateCounter;
        
        private InputAction m_SubmitAction;
        private InputAction m_CancelAction;
        private InputAction m_NavigateAction;
        private InputAction m_MenuAction;

        void Start()
        {
            m_PlayerInputsHandler = FindAnyObjectByType<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler,
                this);

            m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

            m_FramerateCounter = FindAnyObjectByType<FramerateCounter>();
            DebugUtility.HandleErrorIfNullFindObject<FramerateCounter, InGameMenuManager>(m_FramerateCounter, this);

            MenuRoot.SetActive(false);

            LookSensitivitySlider.value = m_PlayerInputsHandler.LookSensitivity;
            LookSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);

            ShadowsToggle.isOn = QualitySettings.shadows != ShadowQuality.Disable;
            ShadowsToggle.onValueChanged.AddListener(OnShadowsChanged);

            InvincibilityToggle.isOn = m_PlayerHealth.Invincible;
            InvincibilityToggle.onValueChanged.AddListener(OnInvincibilityChanged);

            FramerateToggle.isOn = m_FramerateCounter.UIText.gameObject.activeSelf;
            FramerateToggle.onValueChanged.AddListener(OnFramerateCounterChanged);

            m_SubmitAction = InputSystem.actions.FindAction("UI/Submit");
            m_CancelAction = InputSystem.actions.FindAction("UI/Cancel");
            m_NavigateAction = InputSystem.actions.FindAction("UI/Navigate");
            m_MenuAction = InputSystem.actions.FindAction("UI/Menu");
            
            m_SubmitAction.Enable();
            m_CancelAction.Enable();
            m_NavigateAction.Enable();
            m_MenuAction.Enable();
        }

        void Update()
        {
            // Lock cursor when clicking outside of menu
            if (!MenuRoot.activeSelf && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (m_MenuAction.WasPressedThisFrame()
                || (MenuRoot.activeSelf && m_CancelAction.WasPressedThisFrame()))
            {
                if (ControlImage.activeSelf)
                {
                    ControlImage.SetActive(false);
                    return;
                }

                SetPauseMenuActivation(!MenuRoot.activeSelf);

            }

            if (m_NavigateAction.ReadValue<Vector2>().y != 0)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    LookSensitivitySlider.Select();
                }
            }
        }

        public void ClosePauseMenu()
        {
            SetPauseMenuActivation(false);
        }

        void SetPauseMenuActivation(bool active)
        {
            MenuRoot.SetActive(active);

            if (MenuRoot.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);

                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                AudioUtility.SetMasterVolume(1);
            }

        }

        void OnMouseSensitivityChanged(float newValue)
        {
            m_PlayerInputsHandler.LookSensitivity = newValue;
        }

        void OnShadowsChanged(bool newValue)
        {
            QualitySettings.shadows = newValue ? ShadowQuality.All : ShadowQuality.Disable;
        }

        void OnInvincibilityChanged(bool newValue)
        {
            m_PlayerHealth.Invincible = newValue;
        }

        void OnFramerateCounterChanged(bool newValue)
        {
            m_FramerateCounter.UIText.gameObject.SetActive(newValue);
        }

        public void OnShowControlButtonClicked(bool show)
        {
            ControlImage.SetActive(show);
        }
    }
}