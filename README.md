# Project Rascal
Action RPG prototype. 

## Demo Video
<p align="center">
  <a href="https://www.youtube.com/watch?v=-Ozvl6QzQxQ">
    <img src="https://img.youtube.com/vi/-Ozvl6QzQxQ/0.jpg" alt="IMAGE ALT TEXT HERE" />
  </a>
</p>

## Code samples

### 1. Wizards
```cs
public enum GvKey {
    none,
    letter1Read,
    villager1Talked,
    spirit1Talked,
    grondBeaten,
    hektorTalked,
    spirit2Talked
}

public class GameVariablesWizard : MonoBehaviour
{
    private List<GameVariable> gameVariables = new();

    public event Action<GvKey> GameVariableChanged;

    private void Start() {
        InitGameVariablesBasedOnKeys();
    }

    private void InitGameVariablesBasedOnKeys() {
        foreach (GvKey key in Enum.GetValues(typeof(GvKey))) {
            gameVariables.Add(new GameVariable { key = key, value = false });
        }
    }

    public void ActivateGameVariable(GvKey key) {
        var gameVariable = gameVariables.Find(x => x.key == key);
        if(gameVariable != null) {
            gameVariable.value = true;
            GameVariableChanged?.Invoke(key);
        }
    }

    public bool GetGameVariable(GvKey key) {
        if (key == GvKey.none) {
            return true;
        }

        var gameVariable = gameVariables.Find(x => x.key == key);
        if(gameVariable != null) {
            return gameVariable.value;
        }
        return false;
    }

    private void OnDestroy() {
        GameVariableChanged = null;
    }
}
 
[Serializable]
public class GameVariable
{
    public GvKey key;
    public bool value;
}
```

Wizards are manager-singletons to handle different stuff. In this example we have game variables to handle story/level progress. Objects change their state based on this singleton. For example when the player do something bad, then NPCs from the village can start to be agressive. Every object in the game can subscribe to the GameVariableChanged event to listen if there are some changes. 

### 2. Damage Dealers
```cs
    protected void OnTriggerEnter(Collider other) {
        TryToDealDamage(other);
    }

    private void TryToDealDamage(Collider other) {
        if(!injured.Contains(other.gameObject)) {
            var character = other.GetComponent<GameCharacter>();
            if(IsValidDamageTarget(character)) {
                injured.Add(other.gameObject);
                character.TakeDamage(finalDamage);

                var controller = other.GetComponent<IDamagaController>();
                if(controller != null) {
                    controller.VisualizeDamage(transform.position, isBloodSpillVisible);
                }
                
                OnDamageEnd();
            } else {
                OnInvalidDamageTarget();
            }
        }
    }
```
Every object intended for dealing damage inherits from the class DamageDealer which contains logic to explain how damage should be calculated, how long the damage should be, etc. Above method checks if damage should be dealt, but only if the trigger is already active. The trigger is activated in another method by outside controller, for example the player. Because the player decides, based on animation, when the sword should take damage.     

### 3. Player Controller
```cs
    private void HandleRotation() {
        var direction = mouseGroundPosition - transform.position;
        direction.y = 0f; 

        if(direction != lookDirection && CountDistanceToMouse() > minDistanceForRotating) {
            lookDirection = direction;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void HandleRunning() {
        if(InputWizard.instance.IsRightClickPressed() 
        && playerState != CharacterState.Casting 
        && CountDistanceToMouse() > minDistanceForRunning) { 
            var movement = lookDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
            playerState = CharacterState.Running;
            humanAnimator.AnimateRunning();
        } 
    }

    private void HandleIdle() {
        if((!InputWizard.instance.IsRightClickPressed() 
        && playerState != CharacterState.Casting)
        || (CountDistanceToMouse() <= minDistanceForRunning 
        && playerState != CharacterState.Casting)) { 
            playerState = CharacterState.Idle;
            humanAnimator.AnimateIdle();
        } 
    }
```
Player controller is very simple for now. Above methods are called in Update() method. When user presses right mouse button, the character moves in the current rotation direction. And the rotation changes based on current cursor position.

