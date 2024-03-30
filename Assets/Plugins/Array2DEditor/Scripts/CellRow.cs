using UnityEngine;
using UnityEngine.UIElements;

namespace Array2DEditor
{
    [System.Serializable]
    public class CellRow<T>
    {
        [SerializeField]
        private T[] row;

        public CellRow(int size)
        {
            row = new T[size];
        }

        public CellRow(T[] initialValues)
        {
            row = initialValues;
        }

        public T this[int i]
        {
            get => row[i];
            set => row[i] = value;
        }
    }
}
