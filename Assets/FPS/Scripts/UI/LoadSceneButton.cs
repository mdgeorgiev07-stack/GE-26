using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Unity.FPS.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        public string SceneName = "";

        private InputAction m_SubmitAction;
        
        void Start()
        {
            m_SubmitAction = InputSystem.actions.FindAction("UI/Submit");
            m_SubmitAction.Enable();
        }
        
        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject
                && m_SubmitAction.WasPressedThisFrame())
            {
                LoadTargetScene();
            }
        }

        public void LoadTargetScene()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}