using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class WordGenerator : MonoBehaviour {

	public GameObject wordPrefab;
	public float spacing;
    public float readingWait;
    public GameObject marginMarker;
    public GameObject player;
    public GameController gameController;
    public GameObject bullet;
    public GameObject endMarker;
    public float marginHeight = 0.45f;
    private float nextRead;
    private List<GameObject> paragraph = new List<GameObject>();
    private Dictionary<String, Boolean> isWordAggresive = new Dictionary<string, bool>();
    private String text = "";
    private List<String> sentenceData = new List<string>();
    private int currentIndex = 0;
    private int wordsDestroyed = 0;
    private Vector2 position;

	// Use this for initialization
	void Start () {
        LoadData();
        LoadSentence();
        CreateWords();
	}
	
    //Initializes a Word Object given a string
    void initializeWord(String wordString)
    {
        //Creates the object
        GameObject wordObject = (GameObject)Instantiate(wordPrefab, position, Quaternion.identity);
        //Creates the text mesh based on current string
        wordObject.GetComponent<TextMesh>().text = wordString;
        //Creates the box collider
        BoxCollider2D collider = wordObject.AddComponent<BoxCollider2D>();
        //Adjusts the object's position
        float width = collider.size.x * wordObject.transform.localScale.x;
        if (position.x + width > endMarker.transform.position.x)
        {
            position = new Vector2(marginMarker.transform.position.x, position.y - marginHeight);
        }
        position = new Vector2(position.x + width / 2, position.y);
        wordObject.transform.position = position;
        //Updates the position marker for next iteration
        position = new Vector2(position.x + width / 2 + spacing, position.y);
        //Sets the bullet for the word
        WordController wordController = wordObject.GetComponent<WordController>();
        wordController.bullet = bullet;
        string rawWord = GetRawString(wordString);
        if (isWordAggresive.ContainsKey(rawWord))
        {
            if (isWordAggresive[rawWord])
            {
                wordController.adjective = false;
                wordController.verbOrNoun = true;
            } else
            {
                wordController.adjective = true;
                wordController.verbOrNoun = false;
            }
        }
        wordController.generator = this;
        //Adds word object to list
        paragraph.Add(wordObject);
    }

	// Update is called once per frame
	void Update () {
	    if (Time.time > nextRead && currentIndex < paragraph.Count && !gameController.isGameOver())
        {
            //Enables movement in the word
            WordController wordController = paragraph[currentIndex].GetComponent<WordController>();
            wordController.player = player;
            wordController.gameController = gameController;
            wordController.Invoke("WakeUp", 0);
            //Updates variables for next update conditions
            nextRead = Time.time + readingWait;
            currentIndex++;
        }
        if (wordsDestroyed == paragraph.Count)
        {
            wordsDestroyed = 0;
            currentIndex = 0;
            paragraph.Clear();
            CreateWords();
        }
	}

    //Updates words destroyed count
    public void UpdateDestroyed()
    {
        wordsDestroyed += 1;
    }

    //Loads data regarding whether a word is a noun, adjective, or verb
    void LoadData()
    {
        StreamReader reader = new StreamReader("Assets/Database/Data.txt");
        while (!reader.EndOfStream)
        {
            string[] line = reader.ReadLine().Split(';');
            if (line.Length == 2)
            {
                String word = GetRawString(line[0]);
                String type = GetRawString(line[1]);
                if (!isWordAggresive.ContainsKey(word))
                {
                    if (type == "adjective")
                    {
                        isWordAggresive.Add(word, false);
                    }
                    else if (type == "verb" || type == "noun")
                    {
                        isWordAggresive.Add(word, true);
                    }
                }
            }
        }
    }

    //Loads sentence data from text file
    void LoadSentence()
    {
        StreamReader reader = new StreamReader("Assets/Database/Sentence.txt");
        while (!reader.EndOfStream)
        {
            string sentence = reader.ReadLine();
            sentenceData.Add(sentence);
        }
    }

    //Creates the word objects from sentence data
    public void CreateWords()
    {
        //Gets the position from the margin marker
        position = marginMarker.transform.position;
        //Gets a random string from the sentence data
        int index = UnityEngine.Random.Range(0, sentenceData.Count);
        text = sentenceData[index];
        //Creates the word objects
        foreach (String wordString in text.Split(' '))
        {
            initializeWord(wordString);
        }
        //Initializes first "reading" instance
        nextRead = Time.time + readingWait;
    }

    //Gets the raw alphabetical characters in a string
    string GetRawString(String word)
    {
        String newWord = "";
        foreach (Char character in word.ToCharArray())
        {
            if (char.IsLetter(character))
            {
                newWord += character;
            }
        }
        return newWord.ToLower();
    }
}
