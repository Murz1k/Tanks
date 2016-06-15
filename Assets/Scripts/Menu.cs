using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public bool Play = false;
    public bool Continue = false;
    public bool Exit = false;
    private TextMesh text;
    private static string levelPath = "currentLevel.txt";//Сохраненный уровень
    private static string startPath = "startLevel.txt";//Стартовая локация
    internal static string loadedLevelPath; //Загружаемый уровень
	void Start () {
        text = GetComponent<TextMesh>();
        if (File.Exists(levelPath) && Continue)
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
	}
    void OnMouseEnter()
    {
        text.color = Color.red;
    }
    void OnMouseExit()
    {
        text.color = Color.white;
    }
    void OnMouseUp()
    {
        if(Play)//Если кнопка "Плей"
        {
            File.Delete(levelPath);
            loadedLevelPath = startPath;
            SceneManager.LoadScene(1);
        }
        if (Continue)//Если кнопка "Продолжить"
        {
            loadedLevelPath = levelPath;
            SceneManager.LoadScene(1);
        }
        if (Exit)//Если кнопка "Выход"
        {
            Application.Quit();
        }
    }
}
