namespace datagen
{
    public interface IGenerate
    {
        Task FillSchema(int rowsPerTable, string schema);
        Task AddRow(string tableName, int count, string schema);
    }
}