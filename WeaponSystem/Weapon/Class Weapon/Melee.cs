using UnityEngine;

public class Melee : Weapon, IWeapon
{
    private float lastAttackTime = 0f;  // Время последней атаки

    // Примерный метод для атаки
    public void Shoot()
    {
        // Проверка времени между атаками
        if (Time.time - lastAttackTime < fireRate) return;
        Debug.Log("Attack");

        // Проверка попадания в цель с помощью коллайдеров
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distance);
        foreach (var hitCollider in hitColliders)
        {
            /*if (hitCollider.CompareTag("Enemy"))
            {
                // Обработка попадания по врагу
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("Нанесен урон врагу!");
                }
            }*/
        }

        // Обновляем время последней атаки
        lastAttackTime = Time.time;

        // Можно добавить анимацию атаки (например, с помощью Animator)
    }

    public void Reload() { return; }
}
