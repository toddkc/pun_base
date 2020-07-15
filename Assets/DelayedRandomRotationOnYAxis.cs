namespace RotationExamples
{
    using System.Collections;
    using UnityEngine;

    public class DelayedRandomRotationOnYAxis : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float rotationSpeed = 10f;
        [Header("Delay Range")]
        [SerializeField] private float minDelayTime = 4f;
        [SerializeField] private float maxDelayTime = 10f;
        [Header("Rotation Range")]
        [SerializeField] private float minRotation = 15f;
        [SerializeField] private float maxRotation = 90f;

        private Transform thisTransform;
        private Quaternion targetRotationQuaternion;
        private WaitForSeconds delayTime;

        private void Awake()
        {
            thisTransform = transform;
        }

        private void OnEnable()
        {
            targetRotationQuaternion = thisTransform.rotation;
            StartCoroutine(RotateAfterDelay());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void FixedUpdate()
        {
            ApplyRotation();
        }

        // rotate towards target rotation
        private void ApplyRotation()
        {
            thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, 
                                                    targetRotationQuaternion, 
                                                    Time.deltaTime * rotationSpeed);
        }

        // get a random float for a delay timer
        private WaitForSeconds GetRandomDelayTime()
        {
            float _delayTimer = Random.Range(minDelayTime, maxDelayTime);
            WaitForSeconds _newDelay = new WaitForSeconds(_delayTimer);
            return _newDelay;
        }

        // get a random rotation to rotate to
        private Quaternion GetRandomRotationQuaternion()
        {
            Quaternion _currentRotation = thisTransform.rotation;
            float _rotation = Random.Range(minRotation, maxRotation) * GetDirection();
            Quaternion _targetRotation = Quaternion.AngleAxis(_rotation, thisTransform.up);
            return (_currentRotation * _targetRotation);
        }

        // get a positive or negative direction
        private float GetDirection()
        {
            return Random.Range(0, 2) * 2 - 1;
        }

        // wait random amount of time then set random rotation
        private IEnumerator RotateAfterDelay()
        {
            while (true)
            {
                delayTime = GetRandomDelayTime();
                yield return delayTime;
                targetRotationQuaternion = GetRandomRotationQuaternion();
            }
        }
    }
}
