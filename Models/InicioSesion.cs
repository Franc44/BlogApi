namespace BlogApi.Models
{
    public class InicioSesion
    {
        public byte[] Usuario { get; set; }
        public byte[] Contra  { get; set; }
        public string Token { get; set; }
    }

}