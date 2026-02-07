using Microsoft.EntityFrameworkCore;

namespace RestAPI.Context
{
    public class DbConnection : DbContext
    {
        //public static readonly string config = "Server=DESKTOP-U29GOO8\\SQLEXPRESS;Trusted_Connection=True;Database=tinder;TrustServerCertificate=True;";
        public static readonly string config = "Server=10.0.201.112;TrustServerCertificate=True;Database=base2_ISP_22_4_16;User=ISP_22_4_16;PWD=6GdNkdTL69su_";
        // public static readonly string config =
        // "Server=SEMEN1337\\SQLEXPRESS;Database=base1_ISP_22_4_16;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
