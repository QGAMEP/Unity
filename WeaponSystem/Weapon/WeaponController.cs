using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private TMP_Text ammoCurrentText; // Текущее количество патронов в обойме
    [SerializeField] private TMP_Text ammoTypeText; // Текущее количество патронов типа (например, пистолет, винтовка)
    [SerializeField] private Image weaponImage; // Изображение оружия в UI
    [SerializeField] private Transform handTransform; // Позиция для размещения оружия в руках
    [SerializeField] public Camera cameraPlayer; // Позиция для размещения оружия в руках

    [Header("Ammo")]
    [SerializeField] private int rifleAmmo; // Количество патронов для винтовки
    [SerializeField] private int smgAmmo; // Количество патронов для SMG (пистолет-пулемет)
    [SerializeField] private int pistolAmmo; // Количество патронов для пистолета
    [SerializeField] private int shotgunAmmo; // Количество патронов для дробовика
    [SerializeField] private int rpgAmmo; // Количество патронов для РПГ
    [SerializeField] private int snipeAmmo; // Количество патронов для снайперской винтовки

    [Header("Weapon Prefabs")]
    [SerializeField] private List<GameObject> weaponPrefabs; // Список префабов оружия

    #region Private Fields
    private List<GameObject> spawnedWeapons = new List<GameObject>(); // Список созданных оружий
    private List<IWeapon> weaponScripts = new List<IWeapon>(); // Список скриптов оружия (для взаимодействия с ними)

    private GameObject currentSpawnedWeapon; // Текущее спавненное оружие
    private IWeapon currentWeaponScript; // Текущий скрипт оружия для взаимодействия
    private int currentWeaponIndex = 0; // Индекс текущего оружия в списке
    #endregion

    #region MonoBehaviour Methods
    void Awake()
    {
        // Устанавливаем синглтон
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        InitializeWeapons();              // Создаем все оружия один раз
        ActivateWeapon(currentWeaponIndex); // Активируем первое
    }


#if UNITY_STANDALONE_WIN
    void Update()
    {
        // Стрельба
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            Shoot();
        }

        // Перезарядка
        if (Input.GetKeyDown(KeyCode.R)) // Клавиша R
        {
            Reload();
        }

        // Смена оружия
        if (Input.GetKeyDown(KeyCode.Q)) // Клавиша Q
        {
            SwitchWeapon(currentWeaponIndex + 1); // Переключаем на следующее оружие
        }
    }
#endif
    #endregion

    #region Weapon Actions
    void Shoot()
    {
        currentWeaponScript?.Shoot(); // Стрельба, если скрипт оружия доступен
    }

    void Reload()
    {
        currentWeaponScript?.Reload(); // Перезарядка, если скрипт оружия доступен
    }
    #endregion

    #region UI Updates
    // Обновление UI для отображения текущего количества патронов в обойме
    public void UpdateAmmoCurrentDisplay()
    {
        if (ammoCurrentText != null && currentWeaponScript != null)
        {
            // Проверяем, является ли оружие ближнего боя
            if (currentWeaponScript.AmmoType == AmmoType.Melee)
            {
                ammoCurrentText.gameObject.SetActive(false); // Скрываем текст с количеством патронов
            }
            else
            {
                ammoCurrentText.gameObject.SetActive(true); // Показываем текст с количеством патронов
                ammoCurrentText.text = currentWeaponScript.CurrentAmmo.ToString(); // Обновляем количество патронов
            }
        }
    }

    // Обновление UI для отображения общего количества патронов для текущего типа оружия
    public void UpdateAmmoTypeDisplay()
    {
        if (ammoTypeText != null && currentWeaponScript != null)
        {
            // Проверяем, является ли оружие ближнего боя
            if (currentWeaponScript.AmmoType == AmmoType.Melee)
            {
                ammoTypeText.gameObject.SetActive(false); // Скрываем текст с количеством патронов типа
            }
            else
            {
                ammoTypeText.gameObject.SetActive(true); // Показываем текст с количеством патронов типа
                int ammoCountType = GetAmmoCount(currentWeaponScript.AmmoType); // Получаем количество патронов для текущего типа оружия
                ammoTypeText.text = ammoCountType.ToString(); // Обновляем количество патронов
            }
        }
    }
    #endregion

    #region Ammo Management
    // Существующий метод для получения патронов
    public int GetAmmoCount(AmmoType type)
    {
        return type switch
        {
            AmmoType.Rifle => rifleAmmo,
            AmmoType.SMG => smgAmmo,
            AmmoType.Pistol => pistolAmmo,
            AmmoType.Shotgun => shotgunAmmo,
            AmmoType.RPG => rpgAmmo,
            AmmoType.Snipe => snipeAmmo,
            _ => 0 // Если тип не найден, возвращаем 0
        };
    }

    // Новый метод для уменьшения патронов
    public void DecreaseAmmo(AmmoType type, int amount)
    {
        switch (type)
        {
            case AmmoType.Rifle:
                rifleAmmo -= amount;
                break;
            case AmmoType.SMG:
                smgAmmo -= amount;
                break;
            case AmmoType.Pistol:
                pistolAmmo -= amount;
                break;
            case AmmoType.Shotgun:
                shotgunAmmo -= amount;
                break;
            case AmmoType.RPG:
                rpgAmmo -= amount;
                break;
            case AmmoType.Snipe:
                snipeAmmo -= amount;
                break;
        }

        // Обновление UI
        UpdateAmmoTypeDisplay();
    }
    #endregion

    #region Weapon Management
    // Метод для смены оружия
    public void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Count)
        {
            weaponIndex = 0;
        }

        ActivateWeapon(weaponIndex);
    }


    private void InitializeWeapons()
    {
        for (int i = 0; i < weaponPrefabs.Count; i++)
        {
            GameObject weaponInstance = Instantiate(weaponPrefabs[i], handTransform);
            weaponInstance.SetActive(false); // Выключаем до активации
            spawnedWeapons.Add(weaponInstance);

            IWeapon weaponScript = weaponInstance.GetComponent<IWeapon>();
            weaponScripts.Add(weaponScript);
        }
    }

    private void ActivateWeapon(int index)
    {
        if (index < 0 || index >= spawnedWeapons.Count)
        {
            Debug.LogError("Weapon index out of range.");
            return;
        }

        // Отключаем все оружия
        foreach (GameObject weapon in spawnedWeapons)
        {
            weapon.SetActive(false);
        }

        currentSpawnedWeapon = spawnedWeapons[index];
        currentWeaponScript = weaponScripts[index];

        currentSpawnedWeapon.SetActive(true);

        if (currentWeaponScript != null)
        {
            weaponImage.sprite = currentWeaponScript.WeaponSprite;
            handTransform.localPosition = currentWeaponScript.DefaultPosition;
            UpdateAmmoCurrentDisplay();
            UpdateAmmoTypeDisplay();
        }

        currentWeaponIndex = index;
    }

    #endregion
}
