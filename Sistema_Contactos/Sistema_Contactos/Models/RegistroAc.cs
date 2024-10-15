using MongoDB.Bson.Serialization.Attributes;

namespace Proyecto_Bb_2.Models
{
    public class RegistroAc
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public DateTime Fecha_creacion { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        public bool Notificacion { get; set; } = false;

        public bool P_Noti = true;
        public bool S_Noti = true;
        public bool T_Noti = true;
        public bool C_Noti = true;
    }
}
