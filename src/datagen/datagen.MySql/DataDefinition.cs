namespace datagen.MySql
{
    internal class DataDefinition
    {
        public string Column_Name { get; set; }
        public string Data_Type { get; set; }
        public long? Character_Maximum_Length { get; set; }
        public bool Is_Nullable { get; set; }    
        public string Column_Key { get; set; }
        public string Extra { get; set; }
    }
}