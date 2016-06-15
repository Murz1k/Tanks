using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class Tank : MonoBehaviour
{
    #region Variables
    private GameObject tank;
    public float speed = 0.1f;
    public int HP = 100;
    public Transform Bullet;
    internal static Vector2 BulletVector;
    public Sprite TankUp;
    public Sprite TankRight;
    public Sprite BulletUp;
    public Sprite BulletRight;
    private SpriteRenderer TankSpriteRenderer;
    private string tankPosition = string.Empty;
    public static float tankPositionX = 9;
    public static float tankPositionY = 6;
    #endregion
    void Start()
    {
        tank = this.gameObject;
        TankSpriteRenderer = tank.GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        tankPositionX = transform.localPosition.x;
        tankPositionY = transform.localPosition.y;
        GameObject.Find("HP").GetComponent<TextMesh>().text = HP.ToString();
        if (Input.GetKey(KeyCode.UpArrow))//Едем наверх
        {
            TankSpriteRenderer.sprite = TankUp;//Меняем картинку танка на вертикальную
            TankSpriteRenderer.flipY = false;
            TankSpriteRenderer.flipX = true;
            tank.transform.Translate(Vector2.up * speed);//Передвигаем танк
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            TankSpriteRenderer.sprite = TankRight;
            TankSpriteRenderer.flipY = false;
            TankSpriteRenderer.flipX = true;
            tank.transform.Translate(Vector2.right * speed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            TankSpriteRenderer.sprite = TankRight;
            TankSpriteRenderer.flipY = false;
            TankSpriteRenderer.flipX = false;
            tank.transform.Translate(Vector2.left * speed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            TankSpriteRenderer.sprite = TankUp;
            TankSpriteRenderer.flipY = true;
            TankSpriteRenderer.flipX = true;
            tank.transform.Translate(Vector2.down * speed);
        }

        else if (Input.GetKeyDown(KeyCode.Escape))//Переходим в меню
        {
            TerrainManager.SaveAll("currentLevel.txt");
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.RightControl))//Выпускаем снаряд
        {
            Transform bullet = (Transform)Instantiate(Bullet);
            SpriteRenderer BulletSpriteRenderer = bullet.GetComponent<SpriteRenderer>();
            if (TankSpriteRenderer.sprite == TankRight && TankSpriteRenderer.flipX == true)//Определяем направление танка для направления снаряда
            {
                BulletVector = Vector2.right;
                BulletSpriteRenderer.flipX = false;
                BulletSpriteRenderer.sprite = BulletRight;
            }
            else if (TankSpriteRenderer.sprite == TankRight && TankSpriteRenderer.flipX == false)
            {
                BulletVector = Vector2.left;
                BulletSpriteRenderer.flipX = true;
                BulletSpriteRenderer.sprite = BulletRight;
            }
            else if (TankSpriteRenderer.sprite == TankUp && TankSpriteRenderer.flipY == false)
            {
                BulletVector = Vector2.up;
                BulletSpriteRenderer.flipY = false;
                BulletSpriteRenderer.sprite = BulletUp;
            }
            else if (TankSpriteRenderer.sprite == TankUp && TankSpriteRenderer.flipY == true)
            {
                BulletVector = Vector2.down;
                BulletSpriteRenderer.flipY = true;
                BulletSpriteRenderer.sprite = BulletUp;
            }
            bullet.transform.position = tank.transform.position;
        }
    }
}
