using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "SO/PlayerSettings")]
    public class PlayerSettings_SO : ScriptableObject
    {
        [SerializeField]
        private string playerName;
        [SerializeField]
        private Color playerBodyColor;
        [SerializeField]
        private bool m_isProfessor;
        [SerializeField]
        private DrawOnBoard m_board = null;
    [SerializeField]
    private GameObject m_camera = null;

    public GameObject Camera
    {
        get { return m_camera; }
        set { m_camera = value; }
    }
        public bool IsProfessor
        {
            get { return m_isProfessor; }
            set { m_isProfessor = value; }
        }
        public DrawOnBoard Board
        {
            get { return m_board; }
            set { m_board = value; }
        }
    public Camera GetCamera()
    {
        if (m_camera != null)
            return m_camera.GetComponent<Camera>();
        return null;
    }
    }

//public class PlayerSettings_SO : ScriptableObject
//{
//    [SerializeField]
//    private string playerName;
//    [SerializeField]
//    private Color playerBodyColor;
//    [SerializeField]
//    private bool m_isProfessor;
//    [SerializeField]
//    private DrawOnBoard m_board = null;

//    public bool IsProfessor
//    {
//        get { return m_isProfessor; }
//        set { m_isProfessor = value;  }
//    }

//    public DrawOnBoard Board
//    {
//        get { return m_board; }
//        set { m_board = value; }
//    }
//}
