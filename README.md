# datagen

A light-weight library for generating test data

# Supported Database Engines

Currently only supports MySql

# Usage

```
var valueGenerator = new ValueGenerator(
    true,
    DateTime.Now, 
    DateTime.Now.AddDays(-100), 
    DateTime.Now.AddDays(10));

string connectionString = "Server=127.0.0.1;Port=3306;Database=datagentest;Uid=root;Pwd=password;AllowUserVariables=True";

var mySqlDefaults = new MySqlDefaults(connectionString);

var generate = new Generate(    
    connectionString,
    valueGenerator,
    mySqlDefaults.DataTypeParser,
    mySqlDefaults.UniqueKeyGenerator);
await generate.FillSchema(20, "datagentest");
```

## ValueGenerator

Provides an engine to generate psuedo data.  This will use the field name to attempt to emulate actual data.


## Generate

Passed a valid connection string, this will populate a single table, or an entire DB schema.


# Limitations

Currently does now cater for foreign key relationships; that is, any foreign keys will be omitted from the data, rather than cross populated.


# Contibutions

Contributions are welcome, but please check first.



