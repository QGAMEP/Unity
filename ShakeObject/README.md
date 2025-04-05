
<h1 align="center">ENGLISH</a> 
<h3 align="center">Script for shaking an object with different settings</h3>
<h3 align="center">The script will be created automatically on the stage if it is called.</h3>

```csharp
using Utils; //Library for using shaking
```
```csharp
[SerializeField] private Transform objectTransform;             //Shaking object
[SerializeField] private ShakeType shakeType;                   //Type of shaking, rotation or position, or both position and rotation (ShakeType { Position, Rotation, PositionAndRotation })
[SerializeField] Vector3 shakeValues = new Vector3(5, 5, 5);    //Values for shaking
[SerializeField] float shakeDuration = 1f;                      //Shaking time
[SerializeField] int shakeCount = 10;                           //The number of fluctuations
[SerializeField] private bool localVector;                      //use local transform values or global ones
[SerializeField] bool smoothShake = true;                       //Smooth shaking or sharp
```
```csharp
//Usage example
  ShakeObject.Shake(Object, shakeType, shakeValues, shakeDuration, shakeCount, localVector, smoothShake);
```
```csharp
//You can only fart at the shaking object.
  ShakeObject.Shake(Object); 
```
```csharp
//The shaking values will be the default
  ShakeType shakeType = ShakeType.PositionAndRotation,
  Vector3? shakeValues = null,
  float duration = 1f,
  int shakes = 10,
  bool useLocalSpace = true,
  bool smooth = true
```

<h1 align="center">РУССКИЙ</a> 
<h3 align="center">Скрипт для тряски объекта с разными настройками</h3>
<h3 align="center">Скрипт автоматически создаться на сцене если его вызовут</h3>

```csharp
using Utils; //Библиотека для испольования тряски
```
```csharp
[SerializeField] private Transform objectTransform;            //Объект тряски
[SerializeField] private ShakeType shakeType;                  //Тип тряски, вращение или позиция или сразу позиция и вращение (ShakeType { Position, Rotation, PositionAndRotation })
[SerializeField] Vector3 shakeValues = new Vector3(5, 5, 5);   //Значения для тряски
[SerializeField] float shakeDuration = 1f;                     //Время тряски
[SerializeField] int shakeCount = 10;                          //Число колебаний
[SerializeField] private bool localVector;                     //использовать локальные значения transform или мировые
[SerializeField] bool smoothShake = true;                      //Плавная тряска или резкая
```
```csharp
//Пример использования
  ShakeObject.Shake(Object, shakeType, shakeValues, shakeDuration, shakeCount, localVector, smoothShake);
```
```csharp
//Можно пердать только объект тряски
  ShakeObject.Shake(Object); 
```
```csharp
//Значения тряски будут по умолчанию 
  ShakeType shakeType = ShakeType.PositionAndRotation,
  Vector3? shakeValues = null,
  float duration = 1f,
  int shakes = 10,
  bool useLocalSpace = true,
  bool smooth = true
```
