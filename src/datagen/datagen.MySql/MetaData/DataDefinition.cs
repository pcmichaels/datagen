namespace datagen.MySql.MetaData
{
    internal class DataDefinition
    {
        public string Column_Name { get; set; }
        public string Data_Type { get; set; }
        public long? Character_Maximum_Length { get; set; }
        public bool Is_Nullable { get; set; }    
        public string Column_Key { get; set; }
        public string Extra { get; set; }

        internal static DataDefinition ConvertFromDict(Dictionary<string, object> dataDefinitionDict)
        {
            ArgumentNullException.ThrowIfNull(dataDefinitionDict);

            return new DataDefinition()
            {
                Character_Maximum_Length = (long)(dataDefinitionDict["Character_Maximum_Length"] ?? 0L),
                Column_Key = dataDefinitionDict["Column_Key"].ToString() ?? string.Empty,
                Column_Name = dataDefinitionDict["Column_Name"].ToString() ?? string.Empty,
                Data_Type = dataDefinitionDict["Data_Type"].ToString() ?? string.Empty,
                Extra = dataDefinitionDict["Extra"].ToString() ?? string.Empty,
                Is_Nullable = (bool)(dataDefinitionDict["Is_Nullable"] ?? false),
            };
        }
    }
}