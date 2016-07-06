using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class Tank : MonoBehaviour
{
    public float speed = 0.15f;
    public int HP = 100;
    public GameObject Bullet;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))//Переходим в меню
            SceneManager.LoadScene(0);

        if (Input.GetButtonDown("Fire1"))//Выпускаем снаряд=
            Instantiate(Bullet, transform.position, transform.rotation);
    }
}
