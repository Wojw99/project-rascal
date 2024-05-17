using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class StringsWizard
{
    readonly string textFilePath = "Assets\\_Rascal\\Other\\Text\\text.json";
    readonly string dialogFilePath = "Assets\\_Rascal\\Other\\Text\\dialogs.json";
    readonly string actorsFilePath = "Assets\\_Rascal\\Other\\Text\\actors.json";

    Dictionary<string, string> text;
    Dictionary<string, List<string>> dialogs;
    Dictionary<string, Dictionary<string, string>> actors;

    void Initialize() {
        text = LoadFromJson<Dictionary<string, string>>(textFilePath);
        dialogs = LoadFromJson<Dictionary<string, List<string>>>(dialogFilePath);
        actors = LoadFromJson<Dictionary<string, Dictionary<string, string>>>(actorsFilePath);
    }

    T LoadFromJson<T>(string jsonFilePath) {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), jsonFilePath);

        if (!File.Exists(filePath)) {
            throw new FileNotFoundException($"File {filePath} does not exist.");
        }

        var jsonData = File.ReadAllText(filePath);
        var dict = JsonConvert.DeserializeObject<T>(jsonData);

        return dict;
    }

    public string GetActorName(string actorKey) {
        return actors[actorKey]["name"];
    }

    public string GetText(string textKey) {
        return text[textKey];
    }

    public Dictionary<string, string> Text {
        get {
            return text;
        }
    }

    public Dictionary<string, List<string>> Dialogs {
        get {
            return dialogs;
        }
    }

    public Dictionary<string, Dictionary<string, string>> Actors {
        get {
            return actors;
        }
    }

    #region Singleton
    private static StringsWizard instance;

    private StringsWizard() {
        Initialize();
    }

    public static StringsWizard Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StringsWizard();
            }
            return instance;
        }
    }    
    #endregion
}
