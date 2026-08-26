using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Unity.FPS.Gameplay
{
    // Debug script, teleports the player across the map for faster testing
    public class TeleportPlayer : MonoBehaviour
    {
        PlayerCharacterController m_PlayerCharacterController;

        void Awake()
        {
            m_PlayerCharacterController = FindAnyObjectByType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, TeleportPlayer>(
                m_PlayerCharacterController, this);
        }

        void Update()
        {
            if (Keyboard.current.f12Key.wasPressedThisFrame)
            {
                m_PlayerCharacterController.transform.SetPositionAndRotation(transform.position, transform.rotation);
                Health playerHealth = m_PlayerCharacterController.GetComponent<Health>();
                if (playerHealth)
                {
                    playerHealth.Heal(999);
                }
            }
        }

    }
}