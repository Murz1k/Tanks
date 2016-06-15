using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class TerrainManager : MonoBehaviour
{
    #region Variables
    public Transform Sand;
    public Transform Tree;
    public Transform Stone;
    public Transform Pool;
    public Transform Bush;
    private Vector2 nextPosition;
    internal static int Height = 10, Widht = 16;
    private string[] data = null;
    private static Transform[,] transforms = null;
    private float bushDelay = 5f;
    private static string levelPath;
    #endregion
    void Start()
    {
        data = new string[Height * Widht];
        transforms = new Transform[Widht + 2, Height + 2];
        nextPosition = new Vector2(0.5f,0.5f);
        levelPath = Menu.loadedLevelPath;
        if (File.Exists(levelPath))
        {
            data = File.ReadAllLines(levelPath);
            if(data.Length!=0)
            {
                CreateLevel(data);
            }
            else { 
                print("Пустой файл.");
                CreateAll();
                RandomObjects();
            }
        }
        else
        {
            CreateAll();
            RandomObjects();
        }
        StartCoroutine(GenerateBush(bushDelay));
    }
    /// <summary>
    /// Создаем область без объектов.
    /// </summary>
    void CreateAll()
    {
        for (int i = 0; i < Height + 2; i++)
        {
            for (int j = 0; j < Widht + 2; j++)
            {
                Transform trans = (Transform)Instantiate(Sand);
                trans.name = "Sand";
                trans.localPosition = nextPosition;
                nextPosition.x += trans.localScale.x - 0.6f;
                transforms[j, i] = trans;
            }
            nextPosition.y += 1.6f - 0.6f;
            nextPosition.x = 0.5f;
        }
        nextPosition = new Vector2(0.5f, 0.5f);
    }
    /// <summary>
    /// Заполняем созданную область разными объектами.(опционально)
    /// </summary>
    void RandomObjects()
    {
        int count1 = 0;
        for (int r = 0; r < 50 + count1; r++)
        {
            int randomY = Random.Range(1, Height+1);
            int randomX = Random.Range(1, Widht+1);
            Transform trans = transforms[randomX, randomY];
            if (trans.name == "Sand")
            {
                int rand = Random.Range(1, 10);
                if (rand == 1)
                {
                    trans.GetComponent<SpriteRenderer>().sprite = Tree.GetComponent<SpriteRenderer>().sprite;
                    trans.name = "Tree";
                    trans.GetComponent<SpriteRenderer>().sortingLayerName = "Tree";
                }
                else if (rand == 2)
                {
                    trans.GetComponent<SpriteRenderer>().sprite = Pool.GetComponent<SpriteRenderer>().sprite;
                    trans.name = "Pool";
                }
                else if (rand >= 3 && rand <= 5)
                {
                    trans.GetComponent<SpriteRenderer>().sprite = Bush.GetComponent<SpriteRenderer>().sprite;
                    trans.name = "Bush";
                }
                else if (rand >= 6)
                {
                    trans.GetComponent<SpriteRenderer>().sprite = Stone.GetComponent<SpriteRenderer>().sprite;
                    trans.name = "Stone";
                }
            }
            else
            {
                count1++;
            }
        }
    }
    /// <summary>
    /// Сохраняем уровень в файл.
    /// </summary>
    /// <param name="name"></param>
    internal static void SaveAll(string name)
    {
        string[] array = new string[(Height + 2) * (Widht + 2)];
        int index = 0;
        int intervalX = 0;
        int intervalY = 0;
        for (int i = 0; i < Height + 2; i++)
        {
            for (int j = 0; j < Widht + 2; j++)
            {
                Transform trans = transforms[j, i];
                int x = (int)trans.position.x;
                int y = (int)trans.position.y;
                if(i==0 && j==0)
                {
                    if(x!=0)
                    {
                        intervalX = x;
                    }
                    if(y!=0)
                    {
                        intervalY = y;
                    }
                }
                x -= intervalX;
                y -= intervalY;
                array[index++] = trans.name + " " + x + 'x' + y;
            }
        }
        print("Уровень сохранен!");
        File.WriteAllLines(name, array);
    }
    /// <summary>
    /// Проверяем наличие кустов вокруг.
    /// </summary>
    /// <returns></returns>
    Transform GetLonelySand()
    {
        Dictionary<Transform, int> sands = new Dictionary<Transform, int>();
        for(int i = 2;i<Height+1;i++)
        {
            for(int j = 2;j<Widht+1;j++)
            {
                Transform trans = transforms[j, i];
                if(trans.name=="Sand")
                {
                    int count = 8;

                    if(transforms[j+1,i].name!="Bush")
                    {
                        count--;
                    }
                    if (transforms[j-1, i].name != "Bush")
                    {
                        count--;
                    }
                    if (transforms[j, i+1].name != "Bush")
                    {
                        count--;
                    }
                    if (transforms[j, i-1].name != "Bush")
                    {
                        count--;
                    }
                    if (transforms[j+1, i+1].name != "Bush")
                    {
                        count--;
                    }
                    if (transforms[j-1, i-1].name != "Bush")
                    {
                        count--;
                    }
                    if (transforms[j+1, i-1].name != "Bush")
                    {
                        count--;
                    }
                    if (transforms[j-1, i+1].name != "Bush")
                    {
                        count--;
                    }
                    sands.Add(trans, count);
                }
            }
        }
        //Sort
        var myList = sands.ToList();
        myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        print(myList[0].Key.position.x + " " + myList[0].Key.position.y);
        return myList[0].Key;
    }
    /// <summary>
    /// Создаем уровень по указанным данным.
    /// </summary>
    /// <param name="data">Загруженный массив строк.</param>
    void CreateLevel(string[] data)
    {
        for (int i = 0; i < (Height + 2) * (Widht + 2); i++)
        {
            string obj = data[i];
            string name = obj.Split(' ')[0];
            string position = obj.Split(' ')[1];
            int x = int.Parse(position.Split('x')[0]);
            int y = int.Parse(position.Split('x')[1]);
            Transform trans = Sand;
            switch (name)
            {
                case "Tree": trans = Tree; break;
                case "Bush": trans = Bush; break;
                case "Stone": trans = Stone; break;
                case "Pool": trans = Pool; break;
                case "Sand": trans = Sand; break;
            }
            Transform newTrans = (Transform)Instantiate(trans);
            newTrans.name = name;
            newTrans.transform.position = new Vector2(x + 0.5f, y + 0.5f);
            transforms[x, y] = newTrans;
        }
    }
    void Update()
    {
       MoveTerrain();
    }
    /// <summary>
    /// Создаем куст в случайном месте через указанное время.
    /// </summary>
    /// <param name="delay">Задержка до создания куста.</param>
    /// <returns></returns>
    private IEnumerator GenerateBush(float delay)
    {
        while (true)
        {
            int randomY = Random.Range(1, Height);
            int randomX = Random.Range(1, Widht);
            Transform trans = transforms[randomX, randomY];
            trans = GetLonelySand();
            if (trans.name == "Sand")
            {
                trans.GetComponent<SpriteRenderer>().sprite = Bush.GetComponent<SpriteRenderer>().sprite;
                trans.name = "Bush";
                print("Bush is created.");
                yield return new WaitForSeconds(delay);
            }
            else
            {
                continue;
            }
        }
    }
    /// <summary>
    /// Создаем определенное количество разных объектов на новых блоках.
    /// </summary>
    /// <param name="route">Направление движения.</param>
    void CreateRandom(string route)
    {
        //Узнаем, сколько новых объектов нужно создать
        int countObjects = 0;
        if(route=="Top")
        {
            for (int i = 1; i < Widht + 1; i++)
            {
                Transform o = transforms[i, 0];
                if (o.name != "Sand")
                {
                    countObjects++;
                }
            }
        }
        if (route == "Bot")
        {
            for (int i = 1; i < Widht + 1; i++)
            {
                Transform o = transforms[i, Height+1];
                if (o.name != "Sand")
                {
                    countObjects++;
                }
            }
        }
        else if (route=="Right")
        {
            for (int i = 1; i < Height + 1; i++)
            {
                Transform o = transforms[0, i];
                if (o.name != "Sand")
                {
                    countObjects++;
                }
            }
        }
        else if (route == "Left")
        {
            for (int i = 1; i < Height + 1; i++)
            {
                Transform o = transforms[Widht+1, i];
                if (o.name != "Sand")
                {
                    countObjects++;
                }
            }
        }

        int count = 0;
        for (int r = 0; r < countObjects + count; r++)//Создаем определенное число объектов.
        {
            int random = 0;
            Transform trans = Sand;
            if (route == "Top")
            {
                random = Random.Range(1, Widht);
                    trans = transforms[random, Height+1];
            }
            else if (route == "Bot")
            {
                random = Random.Range(1, Widht);
                trans = transforms[random, 0];
            }
            else if (route == "Right")
            {
                random = Random.Range(1, Height);
                trans = transforms[Widht + 1, random];
            }
            else if (route == "Left")
            {
                random = Random.Range(1, Height);
                trans = transforms[0, random];
            }
            if (trans.name == "Sand")
            {
                int rand = Random.Range(1, 10);
                if (rand == 1)
                {
                    trans.GetComponent<SpriteRenderer>().sprite = Tree.GetComponent<SpriteRenderer>().sprite;
                    trans.name = "Tree";
                    trans.GetComponent<SpriteRenderer>().sortingLayerName = "Tree";
                }
                else if (rand == 2)
                {
                    trans.GetComponent<SpriteRenderer>().sprite = Pool.GetComponent<SpriteRenderer>().sprite;
                    trans.name = "Pool";
                }
                else if (rand >= 3 && rand <= 5)
                {
                    trans.GetComponent<SpriteRenderer>().sprite = Bush.GetComponent<SpriteRenderer>().sprite;
                    trans.name = "Bush";
                }
                else if (rand >= 6)
                {
                    trans.GetComponent<SpriteRenderer>().sprite = Stone.GetComponent<SpriteRenderer>().sprite;
                    trans.name = "Stone";
                }
            }
            else
            {
                count++;
            }
        }
    }
    /// <summary>
    /// Двигаем площадку.
    /// </summary>
    void MoveTerrain()
    {
        if (transforms[9, 6].position.x < Tank.tankPositionX)//right
        {
            nextPosition = transforms[Widht + 1, 0].position;
            nextPosition.x++;
            for (int i = 0; i < Height + 2; i++)
            {
                Transform trans = transforms[0, i];
                trans.localPosition = nextPosition;
                nextPosition.y++;
                trans.GetComponent<SpriteRenderer>().sprite = Sand.GetComponent<SpriteRenderer>().sprite;
                trans.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                trans.name = "Sand";
                for (int j = 0; j < Widht + 1; j++)
                {
                    transforms[j, i] = transforms[j + 1, i];
                }
                transforms[Widht + 1, i] = trans;
            }
            CreateRandom("Right");
        }
        else if (transforms[8, 6].position.x > Tank.tankPositionX)//left
        {
            nextPosition = transforms[0, 0].position;
            nextPosition.x--;
            for (int i = 0; i < Height + 2; i++)
            {
                Transform trans = transforms[Widht + 1, i];
                trans.localPosition = nextPosition;
                nextPosition.y++;
                trans.GetComponent<SpriteRenderer>().sprite = Sand.GetComponent<SpriteRenderer>().sprite;
                trans.GetComponent<SpriteRenderer>().sortingLayerName = "Ground"; trans.name = "Sand";
                for (int j = Widht + 1; j > 0; j--)
                {
                    transforms[j, i] = transforms[j - 1, i];
                }
                transforms[0, i] = trans;
            }
            CreateRandom("Left");
        }
        else if (transforms[9,6].position.y < Tank.tankPositionY)//up
        {
            nextPosition = transforms[0, Height + 1].position;
            nextPosition.y++;
            for (int i = 0; i < Widht + 2; i++)
            {
                Transform trans = transforms[i, 0];
                trans.localPosition = nextPosition;
                nextPosition.x++;
                trans.GetComponent<SpriteRenderer>().sprite = Sand.GetComponent<SpriteRenderer>().sprite;
                trans.GetComponent<SpriteRenderer>().sortingLayerName = "Ground"; trans.name = "Sand";
                for (int j = 0; j < Height + 1; j++)
                {
                    transforms[i, j] = transforms[i, j + 1];
                }
                transforms[i, Height + 1] = trans;
            }
           CreateRandom("Top");
        }
        else if (transforms[9, 5].position.y > Tank.tankPositionY)//down
        {
            nextPosition = transforms[0, 0].position;
            nextPosition.y--;
            for (int i = 0; i < Widht + 2; i++)
            {
                Transform trans = transforms[i, Height + 1];
                trans.localPosition = nextPosition;
                nextPosition.x++ ;
                trans.GetComponent<SpriteRenderer>().sprite = Sand.GetComponent<SpriteRenderer>().sprite;
                trans.GetComponent<SpriteRenderer>().sortingLayerName = "Ground"; trans.name = "Sand";
                for (int j = Height + 1; j > 0; j--)
                {
                    transforms[i, j] = transforms[i, j - 1];
                }
                transforms[i, 0] = trans;
            }
            CreateRandom("Bot");
        }
    }
}
