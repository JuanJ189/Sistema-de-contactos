using MongoDB.Bson.Serialization.Attributes;

namespace Proyecto_Bb_2.Models
{
    public class Registro
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Edad { get; set; }
        public int Celular { get; set; }
        public string Correo { get; set; } = null!;
        public string Foto { get; set; } = null!;
    }
}
