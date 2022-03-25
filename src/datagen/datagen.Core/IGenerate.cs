namespace datagen.Core
{
    public interface IGenerate
    {
        /// <summary>
        /// Override a foreign key mapping
        /// Where no mapping exists in the DB itself, this will force datagen to assume one        
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        void CreateForeignKeyMapping(string from, string to);

        /// <summary>
        /// Calls AddRow for every table in the schema
        /// </summary>
        /// <param name="rowsPerTable"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        Task FillSchema(int rowsPerTable, string schema);

        /// <summary>
        /// Populates a table with data
        /// </summary>
        /// <param name="tableName">
        /// The table name to populate
        /// </param>
        /// <param name="count">
        /// Number of rows to create
        /// </param>
        /// <param name="schema">
        /// The schema to populate
        /// </param>       
        /// <param name="primaryKey">
        /// Allows a forced override of the primary key
        /// Can only be set if count is 1
        /// </param>
        /// <returns></returns>
        Task AddRow(string tableName, int count, string schema, object primaryKey);
    }
}