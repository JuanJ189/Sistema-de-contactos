namespace Proyecto_Bb_2.Data
{
    public class Db_actividades : IDb_actividades
    {
        public string Db_connection { get; set; } = null!;
        public string Db_name { get; set; } = null!;
        public string C_Actividades { get; set; } = "Actividades";
    }
}
