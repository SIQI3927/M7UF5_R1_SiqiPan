using System;
using System.Collections.Generic;
using UnityEngine;

namespace XRMultiplayer
{
    public class LobbyTeleports : MonoBehaviour
    {
        private Dictionary<GameObject, Action> functions = new Dictionary<GameObject, Action>();
        private Quaternion lookRotation;
        public GameObject MeetingRoom;
        public GameObject Multiplayer;

        private void Awake()
        {
            //if (SceneManagment.Instance.hallway != null)
            //    functions.Add(SceneManagment.Instance.hallway, GoToHallway);
            //if (SceneManagment.Instance.lobby != null)
            //    functions.Add(SceneManagment.Instance.lobby, GoToLobby);
            //if (SceneManagment.Instance.multiplayer != null) 
            //    functions.Add(SceneManagment.Instance.multiplayer, GoToMultiplayer);
            //if (SceneManagment.Instance.meetingRoom != null) 
            //    functions.Add(SceneManagment.Instance.meetingRoom, GoToMeetingRoom);
        }

        public void ExecuteFunction(GameObject obj)
        {
            if (functions.TryGetValue(obj, out Action action))
            {
                action?.Invoke();
            }
            else
            {
                Debug.LogWarning($" No se encontró una función asociada al GameObject: {obj.name}");
            }
        }

        //public void GoToHallway()
        //{
        //    SceneManagment.Instance.GoToHallway();
        //    lookRotation = SceneManagment.Instance.hallway.transform.rotation;
        //    GameSettingsManager.Instance.TeleportToArea(SceneManagment.Instance.hallway.transform.position, lookRotation);
        //}

        //public void GoToLobby()
        //{
        //    SceneManagment.Instance.GoToLobby();
        //    lookRotation = SceneManagment.Instance.lobby.transform.rotation;
        //    GameSettingsManager.Instance.TeleportToArea(SceneManagment.Instance.lobby.transform.position, lookRotation);
        //}

        public void GoToMultiplayer()
        {
            GameSettingsManager.Instance.TeleportToArea(Multiplayer.transform.position, lookRotation);
        }

        public void GoToMeetingRoom()
        {
            GameSettingsManager.Instance.TeleportToArea(MeetingRoom.transform.position, lookRotation);
        }

        public void GoToPolitecnicoEstella()
        {
            SceneManagment.Instance.GoToPolitecnicoEstella();
        }

        public void GoToLinares()
        {
            SceneManagment.Instance.GoToLinares();
        }

        public void GoToLobby()
        {
            SceneManagment.Instance.GoToLobby();
        }
    }
}
