namespace Proyecto_Bb_2.Servicios
{
    public class EmailConfig
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string UserName { get; set; } = null!;
        public string PassWord { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
    }
}
