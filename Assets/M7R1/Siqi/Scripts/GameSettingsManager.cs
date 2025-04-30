using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

    /// <summary>
    /// This class is used to manage the game settings
    /// </summary>
    public class GameSettingsManager : MonoBehaviour
    {
        public static GameSettingsManager Instance;

    public XROrigin player;
        public PlayerSettings_SO playerSettings;
        public TeleportationProvider teleportationProvider;
        // Start is called before the first frame update
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            teleportationProvider = FindFirstObjectByType<TeleportationProvider>();
        }
        public void SetProfessor()
        {
            playerSettings.IsProfessor = true;
        }

        public void SetStudent()
        {
            playerSettings.IsProfessor = false;
        }

        public bool IsProfessor()
        {
            return playerSettings.IsProfessor;
        }
        public void TeleportToArea(Vector3 position, Quaternion rotation)
        {
            TeleportRequest teleportRequest = new TeleportRequest
            {
                destinationPosition = position,
                destinationRotation = rotation,
                matchOrientation = MatchOrientation.TargetUpAndForward
            };

            teleportationProvider.QueueTeleportRequest(teleportRequest);
        }

    public void Update()
    {
        if (playerSettings.Camera == null && player != null)
            playerSettings.Camera = player.gameObject;
    }
}

