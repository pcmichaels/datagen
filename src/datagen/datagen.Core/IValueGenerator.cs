namespace datagen.Core
{
    public interface IValueGenerator
    {
        public int? IntGeneric(bool allowNulls);
        public int? Int(string fieldName, bool allowNulls);
        object GenerateValue(string columnName, string dataType, bool isNullable);
    }
}