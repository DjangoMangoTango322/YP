using Microsoft.EntityFrameworkCore;

namespace RestAPI.Context
{
    public class DbConnection : DbContext
    {
        public static readonly string config = "Server=10.0.201.112;Database=base1_ISP_22_4_16;User Id=ISP_22_4_16;Password=6GdNkdTL69su_;TrustServerCertificate=True;";

    }
}
