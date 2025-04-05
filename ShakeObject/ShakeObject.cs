using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public enum ShakeType
    {
        Position,
        Rotation,
        PositionAndRotation
    }

    public class ShakeObject : MonoBehaviour
    {
        private static ShakeObject instance;
        public static ShakeObject Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("ShakeObject");
                    instance = go.AddComponent<ShakeObject>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private Dictionary<Transform, (Vector3 position, Quaternion rotation)> originalTransforms = new Dictionary<Transform, (Vector3, Quaternion)>();
        private Dictionary<Transform, int> activeShakes = new Dictionary<Transform, int>();

        public static void Shake(
            Transform targetTransform,
            ShakeType shakeType = ShakeType.PositionAndRotation,
            Vector3? shakeValues = null,
            float duration = 1f,
            int shakes = 10,
            bool useLocalSpace = true,
            bool smooth = true)
        {
            Vector3 finalShakeValues = shakeValues ?? new Vector3(5, 5, 5);
            Instance.StartCoroutine(Instance.ShakeCoroutine(targetTransform, shakeType, finalShakeValues, duration, shakes, useLocalSpace, smooth));
        }

        private IEnumerator ShakeCoroutine(Transform targetTransform, ShakeType shakeType, Vector3 shakeValues, float duration, int shakes, bool useLocalSpace, bool smooth)
        {
            Vector3 originalPosition = useLocalSpace ? targetTransform.localPosition : targetTransform.position;
            Quaternion originalRotation = useLocalSpace ? targetTransform.localRotation : targetTransform.rotation;

            if (!originalTransforms.ContainsKey(targetTransform))
            {
                originalTransforms[targetTransform] = (originalPosition, originalRotation);
            }

            if (!activeShakes.ContainsKey(targetTransform))
                activeShakes[targetTransform] = 0;
            activeShakes[targetTransform]++;

            float intervalBetweenShakes = duration / shakes;

            for (int i = 0; i < shakes; i++)
            {
                Vector3 shakeOffset = new Vector3(
                    Random.Range(-shakeValues.x, shakeValues.x),
                    Random.Range(-shakeValues.y, shakeValues.y),
                    Random.Range(-shakeValues.z, shakeValues.z)
                );

                Quaternion shakeRotation = Quaternion.Euler(
                    Random.Range(-shakeValues.x, shakeValues.x),
                    Random.Range(-shakeValues.y, shakeValues.y),
                    Random.Range(-shakeValues.z, shakeValues.z)
                );

                Vector3 targetPos = originalPosition + shakeOffset;
                targetPos.x = Mathf.Clamp(targetPos.x, originalPosition.x - shakeValues.x, originalPosition.x + shakeValues.x);
                targetPos.y = Mathf.Clamp(targetPos.y, originalPosition.y - shakeValues.y, originalPosition.y + shakeValues.y);
                targetPos.z = Mathf.Clamp(targetPos.z, originalPosition.z - shakeValues.z, originalPosition.z + shakeValues.z);

                Quaternion targetRot = originalRotation * shakeRotation;
                Vector3 targetRotEuler = targetRot.eulerAngles;
                targetRotEuler.x = Mathf.Clamp(targetRotEuler.x, originalRotation.eulerAngles.x - shakeValues.x, originalRotation.eulerAngles.x + shakeValues.x);
                targetRotEuler.y = Mathf.Clamp(targetRotEuler.y, originalRotation.eulerAngles.y - shakeValues.y, originalRotation.eulerAngles.y + shakeValues.y);
                targetRotEuler.z = Mathf.Clamp(targetRotEuler.z, originalRotation.eulerAngles.z - shakeValues.z, originalRotation.eulerAngles.z + shakeValues.z);
                targetRot = Quaternion.Euler(targetRotEuler);

                if (smooth)
                {
                    float elapsedTime = 0f;
                    Vector3 startPos = useLocalSpace ? targetTransform.localPosition : targetTransform.position;
                    Quaternion startRot = useLocalSpace ? targetTransform.localRotation : targetTransform.rotation;

                    while (elapsedTime < intervalBetweenShakes)
                    {
                        float t = elapsedTime / intervalBetweenShakes;
                        t = t * t * (3f - 2f * t); // Smoother step interpolation

                        if (shakeType == ShakeType.Position || shakeType == ShakeType.PositionAndRotation)
                        {
                            Vector3 lerpedPos = Vector3.Lerp(startPos, targetPos, t);
                            lerpedPos.x = Mathf.Clamp(lerpedPos.x, originalPosition.x - shakeValues.x, originalPosition.x + shakeValues.x);
                            lerpedPos.y = Mathf.Clamp(lerpedPos.y, originalPosition.y - shakeValues.y, originalPosition.y + shakeValues.y);
                            lerpedPos.z = Mathf.Clamp(lerpedPos.z, originalPosition.z - shakeValues.z, originalPosition.z + shakeValues.z);

                            if (useLocalSpace)
                                targetTransform.localPosition = lerpedPos;
                            else
                                targetTransform.position = lerpedPos;
                        }

                        if (shakeType == ShakeType.Rotation || shakeType == ShakeType.PositionAndRotation)
                        {
                            Quaternion lerpedRot = Quaternion.Slerp(startRot, targetRot, t);
                            Vector3 euler = lerpedRot.eulerAngles;
                            euler.x = Mathf.Clamp(euler.x, originalRotation.eulerAngles.x - shakeValues.x, originalRotation.eulerAngles.x + shakeValues.x);
                            euler.y = Mathf.Clamp(euler.y, originalRotation.eulerAngles.y - shakeValues.y, originalRotation.eulerAngles.y + shakeValues.y);
                            euler.z = Mathf.Clamp(euler.z, originalRotation.eulerAngles.z - shakeValues.z, originalRotation.eulerAngles.z + shakeValues.z);
                            lerpedRot = Quaternion.Euler(euler);

                            if (useLocalSpace)
                                targetTransform.localRotation = lerpedRot;
                            else
                                targetTransform.rotation = lerpedRot;
                        }

                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                }
                else
                {
                    if (shakeType == ShakeType.Position || shakeType == ShakeType.PositionAndRotation)
                    {
                        if (useLocalSpace)
                            targetTransform.localPosition = targetPos;
                        else
                            targetTransform.position = targetPos;
                    }

                    if (shakeType == ShakeType.Rotation || shakeType == ShakeType.PositionAndRotation)
                    {
                        if (useLocalSpace)
                            targetTransform.localRotation = targetRot;
                        else
                            targetTransform.rotation = targetRot;
                    }
                }

                yield return new WaitForSeconds(intervalBetweenShakes);
            }

            activeShakes[targetTransform]--;

            if (activeShakes[targetTransform] <= 0)
            {
                if (useLocalSpace)
                {
                    targetTransform.localPosition = originalTransforms[targetTransform].position;
                    targetTransform.localRotation = originalTransforms[targetTransform].rotation;
                }
                else
                {
                    targetTransform.position = originalTransforms[targetTransform].position;
                    targetTransform.rotation = originalTransforms[targetTransform].rotation;
                }

                activeShakes.Remove(targetTransform);
                originalTransforms.Remove(targetTransform);
            }
        }


    }
}
