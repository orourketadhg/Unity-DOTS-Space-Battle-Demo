using UnityEngine;

namespace ie.TUDublin.GE2.Camera {

    public class FreeCamera : MonoBehaviour {

        public float moveSpeed;
        public float rotationSpeed;

        private const float MinY = -60f;
        private const float MaxY = 60f;

        private float _rotationY = 0f;
        
        private void FixedUpdate() {

            // camera rotation
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed;

            _rotationY += Input.GetAxis("Mouse Y") * rotationSpeed;
            _rotationY = Mathf.Clamp(_rotationY, MinY, MaxY);

            transform.localEulerAngles = new Vector3(-_rotationY, rotationX);
            
            // camera movement
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            
            transform.position = transform.position + (transform.forward * (y * moveSpeed))+ transform.position + (transform.right * (x * moveSpeed));
        }

    }

}