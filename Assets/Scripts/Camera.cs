using UnityEngine;

public class Camera : MonoBehaviour {

    public GameObject tank;
	void Update () {
        Vector3 tankPos = tank.transform.position;//Получаем координаты танка
        tankPos.z = -1;
        gameObject.transform.position = tankPos;//Камера следует за танком
	}
}
