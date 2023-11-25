/*
 * 	
Evidencia de desempeño grupal #3. CRUD C# DE CONSOLA
Realizar un crud en c# con las siguientes especificaciones.

Crear una base de datos en SQLite y adminsitrarla con dbbrowser o similar.

La base de datos debe tener dos tablas, tabla uno: llamada "t_aprendices" y la tabla 2: "rap" (rap=resultado de aprendizaje)

La primera tabla debe tener campos: uno de id autoincremental, otro de "nombre", "apellido", "edad".

La segunda tabla debe tener: id, codigo_rap,nombre_rap, nivel_importancia (de 1 a 10); entero auto incremental, entero, texto, entero. 

Crear los metodos necesario para conectar, insertar datos en ambas tablas pero la tabla de aprendices debe estar organizada o impresa por la edad de menor a mayor, mostrar la información completa de ambas tablas y un metodo extraque me imprima los nombres del rap cuyo nivel de importancia se mayor a 6.

Debe haber un mecanismo dentro del Main que permita escoger al usuario que tipo de query quiera imprimir. Son tres opciones: 1 todo de la tabla 1, todo de la tabla dos y los rap de nivel superior a 6.

Se debe entregar  todo comprimido. Fecha ficnal de recepción: 30 de noviembre.

Se entrega comprimido un solo miembro del equipo.
 */


using System.Data.SQLite;

SQLiteConnection sqlConnection = null;

//Ejecución principal del programa
try
{
    Console.WriteLine("Cargando base de datos...");
    sqlConnection = CreateConnection();
    CreateSchema();
    FillTables();
    sqlConnection.Close();
    
    Console.WriteLine("Carga completa.");
    
    //Se ejecuta el menú
    Menu();

}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}



void Menu()
{
    // Debe haber un mecanismo dentro del Main que permita escoger al usuario que tipo de query quiera imprimir.
    // Son tres opciones: 1 todo de la tabla 1, todo de la tabla dos y los rap de nivel superior a 6.

    int opcion = 0;
    while (opcion == 0)
    {
        Console.WriteLine("");
        Console.WriteLine("*****************************************************");
        Console.WriteLine("                         MENU                        ");
        Console.WriteLine("*****************************************************");
        Console.WriteLine("");

        Console.WriteLine("Digite la opción deseada");
        Console.WriteLine("1. Imprmir todos los registros de la tabla aprendcies");
        Console.WriteLine("2. Imprmir todos los registros de la tabla rap");
        Console.WriteLine("3. Imprmir todos los registros de la tabla rap con nivel superior a 6");
        Console.WriteLine("");

        //Se valida la entrada del usuario
        if (!int.TryParse(Console.ReadLine(), out opcion))
        {
            opcion = 0;
            Console.WriteLine("Opción invalida");
        }


        switch (opcion)
        {
            // Mostrar toda la tabla aprendices
            case 1:
                string queryTablaAprendices = """
                    SELECT * FROM t_aprendices;
                    """;

                ExecuteQuery(queryTablaAprendices, "t_aprendices");
     
                break;
            // Mostrar tabla  rap
            case 2:
                string queryTablaRap= """
                    SELECT * FROM rap;
                    """;

                ExecuteQuery(queryTablaRap, "rap");
                break;
            //Mostrar rap mayores a 6
            case 3:
                string queryRapFilter = """
                    SELECT * FROM rap WHERE nivel_importancia > 6 ;
                    """;

                ExecuteQuery(queryRapFilter, "rap");
                break;
            default:
                Console.WriteLine("Opción invalida");
                opcion = 0;
                break;
        }
    }
}


#region Métodos de SQLite

///Genera la conexión con sqlite
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

///Crea el esquema de la base de datos (tablas)
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

/// SE llena la tabla con datos aleatorios
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
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1, 10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1, 10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1, 10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1, 10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1, 10)});
            INSERT INTO rap VALUES (NULL, {random.Next(1000, 9999)}, "Nombre del rap {random.Next(99)}", {random.Next(1, 10)});
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

void ExecuteQuery(string query, string tableName)
{
    if (string.IsNullOrEmpty(query))
        throw new Exception("El query no ha sido establecido");

    if (sqlConnection == null)
        throw new Exception("La conexión no ha sido creada");

    try
    {
        using (sqlConnection)
        {
            SQLiteDataReader reader = null;
            sqlConnection.Open();
            SQLiteCommand commnad = new(query, sqlConnection);
            reader = commnad.ExecuteReader();
            PrintDataReader(reader, tableName);            
        }
    }
    catch (Exception e)
    {
        throw new Exception($"Error al ejecutar query:  {e.Message}");
    }

} 
#endregion


void PrintDataReader(SQLiteDataReader reader, string tableName) 
{
    Console.Clear();
    if (tableName.Equals("t_aprendices")) 
    {
        //Titulo de la tabla 
        Console.WriteLine("*****************************************************");
        Console.WriteLine("                  TABLA APRENDICES                   ");
        Console.WriteLine("*****************************************************");
        Console.WriteLine("");

        //Encabezado
        Console.WriteLine("ID\t\t|NOMBRE\t\t\t|APELLIDOS\t\t|EDAD");
        Console.WriteLine("---------------------------------------------------------------------------------------");

        //Información de la tabla
        while (reader.Read())
        {        
            Console.WriteLine($"{reader["id"]}\t\t|{reader["nombre"]}\t\t|{reader["apellidos"]}\t\t|{reader["edad"]}");
            Console.WriteLine("---------------------------------------------------------------------------------------");
        }
    }
    else
    {
        //Titulo de la tabla 
        Console.WriteLine("*****************************************************");
        Console.WriteLine("                     TABLA RAP                       ");
        Console.WriteLine("*****************************************************");
        Console.WriteLine("");
        
        //Encabezado
        Console.WriteLine("ID\t\t|Codigo\t\t|Nombre\t\t\t\t|Nivel de importancia");
        Console.WriteLine("---------------------------------------------------------------------------------------");

        //Información de la tabla.
        while (reader.Read())
        {
            Console.WriteLine($"{reader["id"]}\t\t|{reader["codigo_rap"]}\t\t|{reader["nombre_rap"]}\t\t|{reader["nivel_importancia"]}");
            Console.WriteLine("---------------------------------------------------------------------------------------");
        }
    }

    //Se retorna al menú
    Console.WriteLine("Precione cualquier telca para volver al menú");
    Console.ReadKey();
    
    //Se cierra la conexión
    sqlConnection.Close();
    Menu();
}