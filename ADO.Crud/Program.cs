// See https://aka.ms/new-console-template for more information

using System.Data.SqlClient;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

SQLiteConnection sqlConnection = null;


try
{
    sqlConnection = CreateConnection();
    CreateSchema();
    FillTables();
}
catch (Exception e)
{

    Console.WriteLine(e.Message);
}

void Menu() 
{
    // TODO: Debe haber un mecanismo dentro del Main que permita escoger al usuario que tipo de query quiera imprimir.
    // Son tres opciones: 1 todo de la tabla 1, todo de la tabla dos y los rap de nivel superior a 6.
}

//Método para generar la conexión
SQLiteConnection CreateConnection() 
{
    string databaseName = "MyDatabase.sqlite";
    // this creates a zero-byte file
    try
    {
        SQLiteConnection.CreateFile(databaseName);

        string cnn = $"Data Source={databaseName};Version=3;";
        SQLiteConnection connection = new(cnn);
        return connection;

    }
    catch (Exception e)
    {
        throw new Exception("Error al crear la conexión");
    }
    
   
}

void CreateSchema() 
{
    //La base de datos debe tener dos tablas, tabla uno: llamada "t_aprendices" y la tabla 2: "rap" (rap=resultado de aprendizaje)
    //La primera tabla debe tener campos: uno de id autoincremental, otro de "nombre", "apellido", "edad".
    //La segunda tabla debe tener: id, codigo_rap, nombre_rap, nivel_importancia (de 1 a 10); entero auto incremental, entero, texto, entero. 

    #region Definición de tablas
    string tablaAprendices = """
        CREATE TABLE IF NOT EXISTS "t_aprendices" (
        	id INTEGER PRIMARY KEY,
        	nombre TEXT NOT NULL,
        	apellidos TEXT NOT NULL,
            edad INTEGER
        );
        """;

    string tablaRap = """
        CREATE TABLE IF NOT EXISTS "rap"(
            id INTEGER PRIMARY KEY,
            codigo_rap INTEGER NOT NULL,
            nombre_rap TEXT,
            nivel_importancia   CHECK(nivel_importancia >= 1 or nivel_importancia <= 10)
        );
        """; 
    #endregion

    if (sqlConnection == null)
        throw new Exception("No se ha creado la conexión");

    try
    {
        SQLiteCommand command = null;
        Console.WriteLine("Creando Esquema");
        sqlConnection.Open();
        
        //Creación de la tabla de  aprendices
        command = new SQLiteCommand(tablaAprendices, sqlConnection);
        command.ExecuteNonQuery();

        //Creación de la tabla rap
        command = new SQLiteCommand(tablaRap, sqlConnection);
        command.ExecuteNonQuery();

        Console.WriteLine("Esquema creado satisfactoriamente");

    }
    catch (Exception)
    {
        throw new Exception("Error al crear el esquema de la base de datos");
    }
    finally 
    {
        sqlConnection.Close();
    }

}

void FillTables() 
{
    try
    {
        Random random = new Random();
        string aprendicesInserts = $"""
            INSERT INTO t_aprendices VALUES (null, "Juan David", "Castro Marín", 35);
            INSERT INTO t_aprendices VALUES (null, "Aprendiz 1", "Apellido 1", {random.Next(40)});
            INSERT INTO t_aprendices VALUES (null, "Aprendiz 2", "Apellido 2", {random.Next(40)});
            INSERT INTO t_aprendices VALUES (null, "Aprendiz 3", "Apellido 3", {random.Next(40)});
            INSERT INTO t_aprendices VALUES (null, "Aprendiz 4", "Apellido 4", {random.Next(40)});
            INSERT INTO t_aprendices VALUES (null, "Aprendiz 5", "Apellido 5", {random.Next(40)});
            INSERT INTO t_aprendices VALUES (null, "Aprendiz 6", "Apellido 6", {random.Next(40)});
            INSERT INTO t_aprendices VALUES (null, "Aprendiz 7", "Apellido 7", {random.Next(40)});
            """;

        string rapInserts = $"""
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1,10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1,10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1,10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1,10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1,10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1,10)});
            """;

        if (sqlConnection == null)
            throw new Exception("No se ha creado la conexión");


        SQLiteCommand command = null;
        sqlConnection.Open();
        Console.WriteLine("Insertando datos");
        command = new SQLiteCommand(aprendicesInserts, sqlConnection);
        command.ExecuteNonQuery(); 
        command = new SQLiteCommand(rapInserts, sqlConnection);
        command.ExecuteNonQuery();
        Console.WriteLine("Datos insertados correctamente");

    }
    catch (Exception)
    {

        throw;
    }
    finally { sqlConnection.Close(); }
}

