public interface ISaveable
{
    int SaveLoadOrder { get; set; }
    void SaveData(GameData data);
    void LoadData(GameData data);
}