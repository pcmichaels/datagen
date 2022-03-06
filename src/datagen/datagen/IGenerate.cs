namespace datagen
{
    public interface IGenerate
    {
        void FillDB();
        void FillTable();
        void FillColumn();
        void AddRow(string tableName);
    }
}