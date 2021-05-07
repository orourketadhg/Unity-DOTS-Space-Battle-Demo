using System;
using System.Collections.Generic;
using UnityEngine;

namespace ie.TUDublin.GE2.Camera {

    public class PanningCamera : MonoBehaviour {

        public List<PanningSettings> panningSettings;

        private int _panningIndex;
        private float _yaw;

        private void OnEnable() {
            _panningIndex = 0;
        }

        private void FixedUpdate() {

            var currentSettings = panningSettings[_panningIndex];

            transform.position = currentSettings.position - currentSettings.offset;
            _yaw += currentSettings.speed * Time.deltaTime;
            transform.RotateAround(currentSettings.position, Vector3.up, _yaw);

            transform.LookAt(currentSettings.position);

            if (Input.GetKey(KeyCode.Space)) {
                _panningIndex += 1;
                _panningIndex %= panningSettings.Count;
            }
        }
    }

    [Serializable]
    public struct PanningSettings {
        public string name;
        public Vector3 position;
        public Vector3 offset;
        public float speed;
    }

}