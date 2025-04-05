using UnityEngine;

public interface IWeapon
{
    string WeaponName { get; } //Название оружия
    Sprite WeaponSprite { get; } //Изоброжение оружия
    float FireRate { get; } //Интервал стрельбы
    float Distance { get; } //Дистанция
    int Damage { get; } //Урон
    AmmoType AmmoType { get; } //Тип патрон
    int CurrentAmmo { get; } //Текущее количество патрон в обойме
    int MaxAmmo { get; } //Текущее количество патрон в обойме
    Vector3 DefaultPosition { get; } //Позиция оружия
    void Shoot(); //Метод стрельбы
    void Reload(); //Метод перезарядки
}