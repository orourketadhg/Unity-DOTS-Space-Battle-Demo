
using UnityEngine;

namespace ie.TUDublin.GE2.Camera {
    public class CameraController : MonoBehaviour {

        private FollowingCamera _followingCamera;
        private FreeCamera _freeCamera;
        private PanningCamera _panningCamera;

        private int _cameraIndex;

        private void Awake() {
            _followingCamera = GetComponent<FollowingCamera>();
            _freeCamera = GetComponent<FreeCamera>();
            _panningCamera = GetComponent<PanningCamera>();

            _cameraIndex = 0;
            CycleToNextCamera();
        }

        private void Update() {

            if (Input.GetKey(KeyCode.Mouse0)) {
                CycleToNextCamera();
            }
            
        }

        private void CycleToNextCamera() {
            
            switch(_cameraIndex) {
                case 0:
                    SetCameraToPanning();
                    break;
                case 1:
                    SetCameraToFollowing();
                    break;
                case 2:
                    SetCameraToFree();
                    break;
            }

            _cameraIndex += 1;
            _cameraIndex %= 3;
            
        }

        private void SetCameraToPanning() {
            _panningCamera.enabled = true;
            
            _freeCamera.enabled = false;
            _followingCamera.enabled = false;
        }
        
        private void SetCameraToFree() {
            _freeCamera.enabled = true;
            
            _panningCamera.enabled = false;
            _followingCamera.enabled = false;
        }
        
        private void SetCameraToFollowing() {
            _followingCamera.enabled = true;
            
            _panningCamera.enabled = false;
            _freeCamera.enabled = false;
            
        }
        
    }
}