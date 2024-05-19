using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgressManager : MonoBehaviour
{
    [SerializeField] List<StoryObjective> storyObjectives = new List<StoryObjective>();

    void Initialize()
    {
        // Initialize all story progress variables
    }

    public class StoryObjective {
        public string objectiveName;
        public bool isCompleted;
    }

    #region Singleton
    private static StoryProgressManager instance;

    private StoryProgressManager() {
        Initialize();
    }

    public static StoryProgressManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StoryProgressManager();
            }
            return instance;
        }
    }    
    #endregion
}
