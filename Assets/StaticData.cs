using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static RiggedPlayerController.WeaponEnum chosenWeapon;

    public class StoryPojo
    {
        public float storyTime;
        public string storyText;
        public AudioClip storyClip;
        public string targetScene;

        public StoryPojo(float storyTime, string storyText, AudioClip storyClip, string targetScene)
        {
            this.storyTime = storyTime;
            this.storyText = storyText;
            this.storyClip = storyClip;
            this.targetScene = targetScene;
        }
    }

    public static StoryPojo story;

    public static Dictionary<string, StoryPojo> storyDictionary = new Dictionary<string, StoryPojo>()
    {
        {"Intro",
            new StoryPojo(
                30f,
                "In the dry hush of the temple’s inner sanctum,the priest stood, his shadow stretching long and dark across the ochre floor. <br> <br> His name has long been forgotten, only his title remained: “Hieromancer”. Whispered in reverence among those who feared the gods’ displeasure more than death itself. ",
                null,
                "Temp Menu"
                )},

        {"Level 1_1 Part 1",
            new StoryPojo(
                30f,
                "In the dry hush of the temple’s inner sanctum,the priest stood, his shadow stretching long and dark across the ochre floor.<br><br>His name has long been forgotten, only his title remained: “Hieromancer”. Whispered in reverence among those who feared the gods’ displeasure more than death itself. ",
                null, 
                "Level 1_1"
                )},

        {"Level 1_1 Part 2", 
            new StoryPojo(
                30f,
                "His command had come not in words but in signs—omens read in the entrails of a sparrow, in the patterns of smoke that twisted toward the heavens. In dreams that twisted likewise. The gods demanded a sigil of flesh, a holy cipher formed not by the hand of man but through the binding of sinew and bone.", 
                null,
                "Level 1_1"
                )},

        {"Level 1_1 Part 3",
            new StoryPojo(
                30f,
                "The priest’s hands trembled as he took the blade. He was no stranger to sacrifice; his days were steeped in the blood of offerings, the cries of beasts echoing beneath the temple’s vaulted ceiling. He moved to the first pen, where a goat stood, its eyes wide with the eternal sorrow of prey.", 
                null, 
                "HUB"
                )},

        {"Level 1_2 Part 1", 
            new StoryPojo(
                30f,
                "The priest slew the goat with a single clean cut. The body slumped into the dust with a sound like the falling of fruit. Blood spilled in a widening pool, its edges reaching for his feet as though it sought to claim him too. There it laid, the body aligned  with the constellation that lived within his mind, a vision seared into him in feverish dreams.", 
                null,
                "Level 1_2"
                )},

        {"Level 1_2 Part 2", 
            new StoryPojo(
                30f,
                "And when the pen had cleared, he ventured forth into an unending sea of sand and stone where life was not sustained but endured. Into the desert where dunes rose like the backs of slumbering beasts, their spines carved by winds that had wandered these barrens since the birth of time.", 
                null, 
                "Level 1_2"
                )},

        {"Level 1_2 Part 3",
            new StoryPojo(
                30f,
                "Again and again he worked, the days folding into one another like the creases of a burial shroud. A vulture, its wings spread as if in flight even in death. A cat, its body still warm, fur slick beneath his hands. A serpent, its head severed yet still writhing. He assembled them with a mason’s precision, each corpse a fragment of the divine pattern.",
                null,
                "HUB"
                )},

        {"Level 1_3 Part 1",
            new StoryPojo(
                30f,
                "It was dawn and the work was done. He stepped back, the air thick with the stench of blood and the weight of his labors. Before him, the pattern stretched wide—a glyph unmistakable in its meaning. A warning. A promise. The language of gods, etched not in stone but in death.",
                null,
                "Level 1_3"
                )},

        {"Level 1_3 Part 2",
            new StoryPojo(
                30f,
                "The priest sank to his knees. He stared at the offering, its grotesque beauty undeniable, and felt the crushing insignificance of his own flesh, his own blood. For the pattern was incomplete.",
                null,
                "Level 1_3"
                )},

        {"Level 1_3 Part 3",
            new StoryPojo(
                30f,
                "The gods did not speak, yet he felt their presence in the sudden stillness, in the way the shadows deepened around the glyph. He knew then that he would not leave this place. The sigil demanded more than the unknowing blood of beasts; it hungered still.",
                null,
                "HUB"
                )},

        {"Finale",
            new StoryPojo(
                30f,
                "When the blade turned upon his own flesh, the sun had already risen high, Ra himself, casting its indifferent light over the mound of cadavers. The hieroglyph gleamed, wet and terrible, as the priest’s body slumped into the pattern. A perfect fit. And as the final piece fell into place, the air shifted. The gods had answered…",
                null,
                "HUB"
                )},

    };


}
