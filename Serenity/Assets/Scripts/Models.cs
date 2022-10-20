using System;
using System.Collections.Generic;
using UnityEngine;

public static class Models
{
    #region - Player - 

    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float viewXSensitivity;
        public float viewYSensitivity;

        public bool viewXInverted;
        public bool viewYInverted;

        [Header("Movement")]
        public float walkingForwardSpeed;
        public float walkingBackwardSpeed;
        public float walkingStrafeSpeed;

        [Header("Jumping")]
        public float jumpingHeight;
        public float jumpingFalloff;
    }
    #endregion
}
