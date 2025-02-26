using System;
using System.Collections.Generic;

[Serializable]
public class Character
{
    public string id;
    public string name;
    public string sprite_dir;
    public string position;
    public int[] initialRect;
    public bool visible;
    public Dictionary<string, string> animations;
}

[Serializable]
public class Dialogue
{
    public string character;
    public string text;
    public string emotion;
    public string position;
    public bool visible;
    public string animation;
    public bool waitForInput;
    public List<Character> otherCharacters;
}

[Serializable]
public class SceneTransition
{
    public string type;
    public float duration;
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
    public SceneTransition transitions;
    public List<Dialogue> dialogue;
    public SceneNext next;
}

[Serializable]
public class GameScript
{
    public List<Character> characters;
    public List<Scene> scenes;
}
