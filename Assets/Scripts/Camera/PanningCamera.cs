using System;
using System.Collections.Generic;
using UnityEngine;

namespace ie.TUDublin.GE2.Camera {

    /// <summary>
    /// Camera Controller for panning camera
    /// </summary>
    public class PanningCamera : MonoBehaviour {

        public List<PanningSettings> panningSettings;

        private int _panningIndex;
        private float _yaw;

        private void OnEnable() {
            _panningIndex = 0;
        }

        private void FixedUpdate() {

            // get current camera settings
            var currentSettings = panningSettings[_panningIndex];

            // update camera position
            transform.position = currentSettings.position - currentSettings.offset;
            _yaw += currentSettings.speed * Time.deltaTime;
            transform.RotateAround(currentSettings.position, Vector3.up, _yaw);

            transform.LookAt(currentSettings.position);

            // change camera settings on user input
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                _panningIndex += 1;
                _panningIndex %= panningSettings.Count;
            }
        }
    }

    /// <summary>
    /// Panning settings for camera
    /// </summary>
    [Serializable]
    public struct PanningSettings {
        public string name;
        public Vector3 position;
        public Vector3 offset;
        public float speed;
    }

}