RU
Материалы для своего проекта на игровом движке Unity

using Utils; //Библиотека для испольования тряски

[SerializeField] private Transform objectTransform; //Объект тряски
[SerializeField] private ShakeType shakeType; //Тип тряски, вращение или позиция или стразц позиция и вращение (ShakeType { Position, Rotation, PositionAndRotation })
[SerializeField] Vector3 shakeValues = new Vector3(5, 5, 5); //Значения для тряски
[SerializeField] float shakeDuration = 1f; //Время тряски
[SerializeField] int shakeCount = 10; //Число колебаний
[SerializeField] private bool localVector; //использовать локальные значения transform или мировые
[SerializeField] bool smoothShake = true; //Плавная тряска или резкая

//Пример использования
  ShakeObject.Shake(Object, shakeType, shakeValues, shakeDuration, shakeCount, localVector, smoothShake);

//Можно пердать только объект тряски
  ShakeObject.Shake(Object); 

//Значения тряски будут по умолчанию 
  ShakeType shakeType = ShakeType.PositionAndRotation,
  Vector3? shakeValues = null,
  float duration = 1f,
  int shakes = 10,
  bool useLocalSpace = true,
  bool smooth = true

EN
Materials for your project on the Unity game engine

using Utils; //Library for using shaking

[SerializeField] private Transform objectTransform; //Shaking object
[SerializeField] private ShakeType shakeType; //Type of shaking, rotation or position or rhinestone position and rotation (ShakeType { Position, Rotation, PositionAndRotation })
[SerializeField] Vector3 shakeValues = new Vector3(5, 5, 5); //Values for shaking
[SerializeField] float shakeDuration = 1f; //Shaking time
[SerializeField] int shakeCount = 10; //The number of fluctuations
[SerializeField] private bool localVector; //use local transform values or global ones
[SerializeField] bool smoothShake = true; //Smooth shaking or sharp

//Usage example
  ShakeObject.Shake(Object, shakeType, shakeValues, shakeDuration, shakeCount, localVector, smoothShake);

//You can only fart at the shaking object.
  ShakeObject.Shake(Object); 

//The shaking values will be the default
  ShakeType shakeType = ShakeType.PositionAndRotation,
  Vector3? shakeValues = null,
  float duration = 1f,
  int shakes = 10,
  bool useLocalSpace = true,
  bool smooth = true
