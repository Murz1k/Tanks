using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    float destroyDelay = 2f;//Задержка до уничтожения снаряда
    float speed = 0.2f; //Скорость снаряда
    void Start()
    {
        StartCoroutine(DestroyBullet(destroyDelay));//Запускаем событие уничтожения снаряда
    }
    void Update()
    {
        transform.Translate(Vector2.up * speed); //Двигаем снаряд в указанном направлении
    }
    /// <summary>
    /// Уничтожаем снаряд через указанное время
    /// </summary>
    /// <param name="delay">Время до уничтожения снаряда.</param>
    /// <returns></returns>
    IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
