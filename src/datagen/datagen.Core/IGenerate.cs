﻿namespace datagen.Core
{
    public interface IGenerate
    {
        Task FillSchema(int rowsPerTable, string schema);
        Task AddRow(string tableName, int count, string schema, object primaryKey);
    }
}