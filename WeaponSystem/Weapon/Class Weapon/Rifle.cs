using UnityEngine;

public class Rifle : Weapon, IWeapon
{
    private float lastFireTime = 0f;  // Время последнего выстрела
    private float reloadTime = 2f;    // Время перезарядки
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
        WeaponController.Instance.UpdateAmmoCurrentDisplay();

    }

    // Примерный метод для стрельбы
    public void Shoot()
    {
        if (isReloading) return;

        // Проверка времени между выстрелами
        if (Time.time - lastFireTime < fireRate) return;

        // Проверка наличия патронов
        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        // Уменьшаем количество патронов
        currentAmmo--;
        WeaponController.Instance.UpdateAmmoCurrentDisplay();
        Debug.Log("Shoot");

        // Отправляем луч из центра камеры
        Ray ray = WeaponController.Instance.cameraPlayer.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)); // Луч из центра экрана
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            // Обработка попадания в цель
            /*if (hit.collider.CompareTag("Enemy"))
            {
                // Наносим урон по врагу
                hit.collider.GetComponent<Enemy>().TakeDamage(damage);
            }*/
        }

        // Обновляем время последнего выстрела
        lastFireTime = Time.time;
    }

    // Перезарядка оружия
    public void Reload()
    {
        if (isReloading) return;

        if (currentAmmo >= maxAmmo) return;

        // Получаем количество патронов типа оружия в WeaponController
        int ammoCount = WeaponController.Instance.GetAmmoCount(ammoType);

        // Проверка, есть ли достаточно патронов для перезарядки
        if (ammoCount <= 0)
        {
            Debug.Log("Нет патронов для перезарядки!");
            return;
        }

        Debug.Log("Reload");
        // Запускаем перезарядку
        isReloading = true;

        // Задержка для перезарядки
        Invoke("FinishReload", reloadTime);
    }

    // Завершение перезарядки
    private void FinishReload()
    {
        int ammoNeeded = maxAmmo - currentAmmo;
        int availableAmmo = WeaponController.Instance.GetAmmoCount(ammoType);
        int ammoToReload = Mathf.Min(ammoNeeded, availableAmmo);

        if (ammoToReload <= 0)
        {
            Debug.Log("Нет патронов для пополнения!");
            isReloading = false;
            return;
        }

        currentAmmo += ammoToReload;

        WeaponController.Instance.DecreaseAmmo(ammoType, ammoToReload);

        Debug.Log("Reload Finish");
        isReloading = false;

        WeaponController.Instance.UpdateAmmoCurrentDisplay();
    }

}
