namespace datagen.Core
{
    public interface IValueGenerator
    {
        public int IntGeneric(string fieldName, bool allowNulls);
        object GenerateValue(string columnName, string dataType, bool isNullable);
    }
}