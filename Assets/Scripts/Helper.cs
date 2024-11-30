using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public static class Helper
{
    public enum SpecialTileState { EMPTY, SOLID, ETHEREAL, REFLECTIVE }

    public enum PlayerAction { ABSORB, RELEASE }

    public enum KeyType { NORMAL }


    public const int ID_DOC1 = 0100;
    public const int ID_DOC2 = 0101;
    public const int ID_PLAYER_UNKNOWN = 0200;
    public const int ID_PLAYER_KNOWN = 0201;
    public const int ID_NARRATOR = 0300;

    public const string NAME_DOC1 = "Scientist 1";
    public const string NAME_DOC2 = "Scientist 2";
    public const string NAME_PLAYER_UNKNOWN = "???";
    public const string NAME_PLAYER_KNOWN = "Subject 616";
    public const string NAME_NARRATOR = "NARRATOR";

    public static Dictionary<int, string> dictIDToName = new Dictionary<int, string>()
    {
        { ID_DOC1, NAME_DOC1 },
        { ID_DOC2, NAME_DOC2 },
        { ID_PLAYER_UNKNOWN, NAME_PLAYER_UNKNOWN },
        { ID_PLAYER_KNOWN, NAME_PLAYER_KNOWN },
        { ID_NARRATOR, NAME_NARRATOR },
    };

    public struct DialogueSettings
    {
        public int idCharacter;
        public string text;

        public DialogueSettings(int idCharacter, string text)
        {
            this.idCharacter = idCharacter;
            this.text = text;
        }
    }

    // MAX SIZE 170

    public static DialogueSettings tutorial1Dialogue1 = new DialogueSettings(ID_DOC1, "The experiment was a success.");
    public static DialogueSettings tutorial1Dialogue2 = new DialogueSettings(ID_DOC2, "We need to test its intelligence... Bring it to the trial ground.");

    public static List<DialogueSettings> room1Dialogues = new List<DialogueSettings>()
    {
        tutorial1Dialogue1, 
        tutorial1Dialogue2,
    };


    public static DialogueSettings level1Dialogue1 = new DialogueSettings(ID_DOC1, "It exceeded our expectations.");
    public static DialogueSettings level1Dialogue2 = new DialogueSettings(ID_DOC2, "We can use it...");
    public static DialogueSettings level1Dialogue3 = new DialogueSettings(ID_DOC2, "Prepare the lab for the next phase.");

    public static DialogueSettings level1Dialogue4 = new DialogueSettings(ID_NARRATOR, "That night...");

    public static DialogueSettings level1Dialogue5 = new DialogueSettings(ID_PLAYER_UNKNOWN, ".....................................");
    public static DialogueSettings level1Dialogue6 = new DialogueSettings(ID_PLAYER_UNKNOWN, "I feel...");
    public static DialogueSettings level1Dialogue7 = new DialogueSettings(ID_PLAYER_UNKNOWN, "Cold...");
    public static DialogueSettings level1Dialogue8 = new DialogueSettings(ID_PLAYER_UNKNOWN, "What's going on?");
    public static DialogueSettings level1Dialogue9 = new DialogueSettings(ID_PLAYER_UNKNOWN, "I need to get out.");

    public static List<DialogueSettings> level1Dialogues = new List<DialogueSettings>()
    {
        level1Dialogue1,
        level1Dialogue2,
        level1Dialogue3,

        level1Dialogue4,

        level1Dialogue5,
        level1Dialogue6,
        level1Dialogue7,
        level1Dialogue8,
        level1Dialogue9,
    };


    public static DialogueSettings level5Dialogue1 = new DialogueSettings(ID_PLAYER_UNKNOWN, "I'm getting tired.");
    public static DialogueSettings level5Dialogue2 = new DialogueSettings(ID_PLAYER_UNKNOWN, "I know they are watching me, why is nobody stopping me ?");

    public static List<DialogueSettings> level5Dialogues = new List<DialogueSettings>()
    {
        level5Dialogue1,
        level5Dialogue2,
    };


    public static DialogueSettings level8Dialogue1 = new DialogueSettings(ID_DOC1, "Phase 3 is now over!");
    public static DialogueSettings level8Dialogue2 = new DialogueSettings(ID_DOC2, "Congratulations everyone! Subject 626 is by far our biggest success.");
    public static DialogueSettings level8Dialogue3 = new DialogueSettings(ID_DOC2, "This is the first step towards the next technological era.");

    public static DialogueSettings level8Dialogue4 = new DialogueSettings(ID_PLAYER_UNKNOWN, ".............");
    public static DialogueSettings level8Dialogue5 = new DialogueSettings(ID_PLAYER_UNKNOWN, "So... That's who I am... WHAT I am...");

    public static DialogueSettings level8Dialogue6 = new DialogueSettings(ID_PLAYER_KNOWN, "Just an experiment.");
    public static DialogueSettings level8Dialogue7 = new DialogueSettings(ID_PLAYER_KNOWN, "Will my life always be like this? Is this even life?");
    public static DialogueSettings level8Dialogue8 = new DialogueSettings(ID_PLAYER_KNOWN, "I just want to rest.");

    public static List<DialogueSettings> level8Dialogues = new List<DialogueSettings>()
    {
        level8Dialogue1,
        level8Dialogue2,
        level8Dialogue3,

        level8Dialogue4,
        level8Dialogue5,

        level8Dialogue6,
        level8Dialogue7,
        level8Dialogue8,
    };


    public static Dictionary<int, List<DialogueSettings>> dictIDToDialogueSettings = new Dictionary<int, List<DialogueSettings>>()
    {
        { 0, room1Dialogues },
        { 1, level1Dialogues },
        { 2, level5Dialogues },
        { 3, level8Dialogues },
    };



    #region PLAYER PREFS KEYS


    //<size=100%>statu<size=138%>S<size=100%>cape

    /*
     * TODO
     * */

    public const string KEY_LEVEL_TO_PLAY = "KEY_LEVEL_TO_PLAY";

    public const string KEY_MASTER_VOLUME = "KEY_MASTER_VOLUME";
    public const string KEY_MUSIC_VOLUME = "KEY_MUSIC_VOLUME";
    public const string KEY_EFFECTS_VOLUME = "KEY_EFFECTS_VOLUME";


    public const string KEY_FULLSCREEN = "KEY_FULLSCREEN";




    public static Dictionary<int, string> dictIDToLevelName = new Dictionary<int, string>()
    {
        { 0, "Tutorial1" },
        { 1, "Tutorial2" },
        { 2, "Tutorial3" },

        { 3, "Level1" },
        { 4, "Level2" },
        { 5, "Level3" },
        { 6, "Level4" },
              
        { 7, "Level5" },
        { 8, "Level6" },
        { 9, "Level7" },
        { 10, "Level8" },
    };


    #endregion

}
