using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private Vector2 bulletVector; //Направление снаряда
    private float destroyDelay = 2f;//Задержка до уничтожения снаряда
    private float speed = 0.2f; //Скорость снаряда
    void Start()
    {
        bulletVector = Tank.BulletVector * speed; //Узнаем направление танка
        StartCoroutine(DestroyBullet(destroyDelay));//Запускаем событие уничтожения снаряда
    }
    void Update()
    {
      transform.Translate(bulletVector); //Двигаем снаряд в указанном направлении
    }
    /// <summary>
    /// Уничтожаем снаряд через указанное время
    /// </summary>
    /// <param name="delay">Время до уничтожения снаряда.</param>
    /// <returns></returns>
    private IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
             Destroy(gameObject);
    }
}
