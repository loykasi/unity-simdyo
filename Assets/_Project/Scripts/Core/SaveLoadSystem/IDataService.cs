public interface IDataService
{
    void Save(GameData data);
    GameData Load(string name);
}