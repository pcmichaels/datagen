// See https://aka.ms/new-console-template for more information
using datagen.MySql;

Console.WriteLine("Hello, World!");

var generate = new Generate("Server=127.0.0.1;Port=3307;Database=musiclenderus;Uid=root;Pwd=mypass;AllowUserVariables=True");
generate.AddRow();