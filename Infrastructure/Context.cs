using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<UserTalent> UserTalents { get; set; }
        public DbSet<ThreeStarWeapon> ThreeStarWeapons { get; set; }
        public DbSet<FourStarWeapon> FourStarWeapons { get; set; }
        public DbSet<FiveStarWeapon> FiveStarWeapons { get; set; }
        public DbSet<FourStarCharacter> FourStarCharacters { get; set; }
        public DbSet<FiveStarCharacter> FiveStarCharacters { get; set; }

        public Context()
        {
            Database.EnsureCreated();
        }
        private static Context database;
        public static Context Initialization()
        {
            if (database == null)
            {
                database = new Context();
            }
            return database;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=lorutskiibotdb;Trusted_Connection=True;");
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string DSId { get; set; }
        public string TwitchId { get; set; }
        public string Nickname { get; set; }
        public int Coin { get; set; }
        public int Experience { get; set; }
        public DateTime LastCommand { get; set; }
        public DateTime LastTimely { get; set; } //last timely
        public int RollCount { get; set; }
        public int CardId { get; set; }
    }
    public class UserTalent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Pyro { get; set; }
        public int Cryo { get; set; }
        public int Hydro { get; set; }
        public int Electro { get; set; }
        public int Anemo { get; set; }
        public int Geo { get; set; }
        public int Dendro { get; set; }
    }
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class ThreeStarWeapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class FourStarWeapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class FiveStarWeapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class FourStarCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class FiveStarCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
