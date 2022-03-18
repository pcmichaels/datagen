namespace datagen.Core
{
    public interface IUniqueKeyGenerator
    {
        public object GenerateUniqueKey(
            string tableName, string columnName,
            long columnLength, string dataType);

    }
}