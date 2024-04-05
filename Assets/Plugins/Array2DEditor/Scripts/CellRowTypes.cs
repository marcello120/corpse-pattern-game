using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    public class CellRowBool : CellRow<bool>
    {
        public CellRowBool(int size) : base(size) { }

        public CellRowBool(bool[] initialValues) : base(initialValues) { }
    }

    [System.Serializable]
    public class CellRowFloat : CellRow<float>
    {
        public CellRowFloat(int size) : base(size) { }

        public CellRowFloat(float[] initialValues) : base(initialValues) { }
    }

    [System.Serializable]
    public class CellRowInt : CellRow<int>
    {
        public CellRowInt(int size) : base(size) { }

        public CellRowInt(int[] initialValues) : base(initialValues) { }
    }

    [System.Serializable]
    public class CellRowString : CellRow<string>
    {
        public CellRowString(int size) : base(size) { }

        public CellRowString(string[] initialValues) : base(initialValues) { }
    }

    [System.Serializable]
    public class CellRowSprite : CellRow<Sprite>
    {
        public CellRowSprite(int size) : base(size) { }

        public CellRowSprite(Sprite[] initialValues) : base(initialValues) { }
    }

    [System.Serializable]
    public class CellRowAudioClip : CellRow<AudioClip>
    {
        public CellRowAudioClip(int size) : base(size) { }

        public CellRowAudioClip(AudioClip[] initialValues) : base(initialValues) { }
    }
}
