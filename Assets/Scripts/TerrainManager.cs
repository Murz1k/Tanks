using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class TerrainManager : MonoBehaviour
{
    public Transform Sand;
    public Transform Tree;
    public Transform Stone;
    public Transform Pool;
    public Transform Bush;
    private Vector2 nextPosition;
    Tank tank;
    int Height = 10;
    int Widht = 16;
    private static Transform[,] transforms = null;
    private float bushDelay = 5f;
    void Start()
    {
        tank = FindObjectOfType<Tank>();
        transforms = new Transform[Widht + 2, Height + 2];
        nextPosition = new Vector2(0.5f, 0.5f);
        CreateAll();
        RandomObjects();
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
                trans.parent = gameObject.transform;
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
        int count = 0;
        for (int r = 0; r < 50 + count; r++)
        {
            int randomY = Random.Range(1, Height + 1);
            int randomX = Random.Range(1, Widht + 1);
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
                count++;
            }
        }
    }
    /// <summary>
    /// Проверяем наличие кустов вокруг.
    /// </summary>
    /// <returns></returns>
    Transform GetLonelySand()
    {
        Dictionary<Transform, decimal> lonelySandsDictionary = new Dictionary<Transform, decimal>();
        for (int i = 2; i < Height + 1; i++)
        {
            for (int j = 2; j < Widht + 1; j++)
            {
                Transform cellTransform = transforms[j, i];
                if (cellTransform.name == "Sand")
                {
                    int bushCount = 8;

                    if (transforms[j + 1, i].name != "Sand")
                    {
                        bushCount--;
                    }
                    if (transforms[j - 1, i].name != "Sand")
                    {
                        bushCount--;
                    }
                    if (transforms[j, i + 1].name != "Sand")
                    {
                        bushCount--;
                    }
                    if (transforms[j, i - 1].name != "Sand")
                    {
                        bushCount--;
                    }
                    if (transforms[j + 1, i + 1].name != "Sand")
                    {
                        bushCount--;
                    }
                    if (transforms[j - 1, i - 1].name != "Sand")
                    {
                        bushCount--;
                    }
                    if (transforms[j + 1, i - 1].name != "Sand")
                    {
                        bushCount--;
                    }
                    if (transforms[j - 1, i + 1].name != "Sand")
                    {
                        bushCount--;
                    }
                    lonelySandsDictionary.Add(cellTransform, bushCount);
                }
            }
        }
        var veryLonelySand = lonelySandsDictionary.Aggregate((l, r) => l.Value >= r.Value ? l : r).Key;
        return veryLonelySand;
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
        yield return new WaitForSeconds(delay);
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
                break;
            }
        }
        StartCoroutine(GenerateBush(bushDelay));
    }
    /// <summary>
    /// Создаем определенное количество разных объектов на новых блоках.
    /// </summary>
    /// <param name="route">Направление движения.</param>
    void CreateRandom(string route)
    {
        //Узнаем, сколько новых объектов нужно создать
        int countObjects = 0;
        if (route == "Top")
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
                Transform o = transforms[i, Height + 1];
                if (o.name != "Sand")
                {
                    countObjects++;
                }
            }
        }
        else if (route == "Right")
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
                Transform o = transforms[Widht + 1, i];
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
                trans = transforms[random, Height + 1];
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
        Vector2 directionMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        foreach(var cell in transforms)
        {
            cell.Translate(-directionMove*0.2f);
        }
        if (transforms[9, 6].position.x < tank.transform.position.x)//right
        {
            nextPosition = transforms[Widht + 1, 0].localPosition;
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
        else if (transforms[8, 6].position.x > tank.transform.position.x)//left
        {
            nextPosition = transforms[0, 0].localPosition;
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
        else if (transforms[9, 6].position.y < tank.transform.position.y)//up
        {
            nextPosition = transforms[0, Height + 1].localPosition;
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
        else if (transforms[9, 5].position.y > tank.transform.position.y)//down
        {
            nextPosition = transforms[0, 0].localPosition;
            nextPosition.y--;
            for (int i = 0; i < Widht + 2; i++)
            {
                Transform trans = transforms[i, Height + 1];
                trans.localPosition = nextPosition;
                nextPosition.x++;
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
