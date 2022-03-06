// See https://aka.ms/new-console-template for more information
using datagen.MySql;

Console.WriteLine("Hello, World!");

var generate = new Generate("Server=127.0.0.1;Port=3306;Database=datagentest;Uid=root;Pwd=password;AllowUserVariables=True");
generate.AddRow("test_table");
