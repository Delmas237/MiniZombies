using System.Collections.Generic;

namespace Saves
{
    public interface IDataSaver<T> where T : ISavableData
    {
        List<T> Load();
        void Save(T data);
    }
}
