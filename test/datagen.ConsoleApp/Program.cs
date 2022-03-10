// See https://aka.ms/new-console-template for more information
using datagen.Core;
using datagen.MySql;

var valueGenerator = new ValueGenerator(false, 
    DateTime.Now, DateTime.Now.AddDays(-100), DateTime.Now.AddDays(10));

var generate = new Generate(
    "Server=127.0.0.1;Port=3306;Database=datagentest;Uid=root;Pwd=password;AllowUserVariables=True",
    valueGenerator);
await generate.AddRow("test_table");
