using datagen.Core;
using datagen.MySql;
using datagen.MySql.KeyGeneration;

var valueGenerator = new ValueGenerator(
    true,
    DateTime.Now, 
    DateTime.Now.AddDays(-100), 
    DateTime.Now.AddDays(10));

string connectionString = "Server=127.0.0.1;Port=3306;Database=datagentest;Uid=root;Pwd=password;AllowUserVariables=True";
var dataTypeParser = new MySqlDataTypeParser();
var uniqueKeyGenerator = new UniqueKeyGenerator(
    connectionString,
    dataTypeParser);

var generate = new Generate(    
    connectionString,
    valueGenerator,
    dataTypeParser,
    uniqueKeyGenerator);
//await generate.AddRow("test_table", 50, "datagentest");
await generate.FillSchema(20, "datagentest");
