using System.Collections.Generic;

public interface IDataSaver<T> where T : ISavableData
{
    List<T> Load();
    void Save(T data);
}
