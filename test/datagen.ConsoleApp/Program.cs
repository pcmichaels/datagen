using datagen.Core;
using datagen.MySql;
using datagen.MySql.MySql;

var valueGenerator = new ValueGenerator(
    true,
    DateTime.Now, 
    DateTime.Now.AddDays(-100), 
    DateTime.Now.AddDays(10));

string connectionString = "Server=127.0.0.1;Port=3306;Database=datagentest;Uid=root;Pwd=password;AllowUserVariables=True";

var mySqlDefaults = new MySqlDefaults(connectionString);
var dataAccessorFactory = mySqlDefaults.DataAccessorFactory;

var generate = new Generate(    
    connectionString,
    valueGenerator,
    mySqlDefaults.DataTypeParser,
    mySqlDefaults.UniqueKeyGenerator, 
    dataAccessorFactory);

//await generate.AddRow("test_table", 50, "datagentest");
await generate.FillSchema(20, "datagentest");
