namespace datagen
{
    public interface IGenerate
    {
        void FillDB();
        void FillTable();
        void FillColumn();
        Task AddRow(string tableName);
    }
}