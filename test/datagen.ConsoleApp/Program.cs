// See https://aka.ms/new-console-template for more information
using datagen.Core;
using datagen.MySql;

var valueGenerator = new ValueGenerator(
    true,
    DateTime.Now, 
    DateTime.Now.AddDays(-100), 
    DateTime.Now.AddDays(10));

var dataTypeParser = new MySqlDataTypeParser();

var generate = new Generate(
    "Server=127.0.0.1;Port=3306;Database=datagentest;Uid=root;Pwd=password;AllowUserVariables=True",
    valueGenerator,
    dataTypeParser);
//await generate.AddRow("test_table", 50);
await generate.FillSchema(20, "datagentest");
