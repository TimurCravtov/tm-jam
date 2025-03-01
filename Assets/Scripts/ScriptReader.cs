using System;
using System.Collections.Generic;

[Serializable]
public class DialogueCharacter
{
    public string id;          // Character ID
    public string name;        // Character display name
    public string emotion;     // Current emotion/expression
    public string position;    // Position on screen (left, center, right)
    public bool visible;       // Whether the character is visible
    public bool isSpeaking;    // Whether this character is the current speaker
}

[Serializable]
public class Dialogue
{
    public string text;             // The dialogue text
    public string animation;        // Optional animation to play
    public bool waitForInput;       // Whether to wait for user input before continuing
    public List<DialogueCharacter> characters;  // All characters in the scene for this dialogue
}

[Serializable]
public class SceneChoices
{
    public string text;
    public string nextScene;
}

[Serializable]
public class SceneNext
{
    public string scene;
    public List<SceneChoices> choices;
}

[Serializable]
public class Scene
{
    public string id;
    public string background;
    public bool transition;
    public List<Dialogue> dialogue;
    public SceneNext next;
}

[Serializable]
public class GameScript
{
    public List<Scene> scenes;  // Removed the characters list
}