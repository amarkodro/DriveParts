using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data.Models;


namespace RS1_2024_25.API.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<DashboardStats> Dashboards { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<PartEngine> PartEngines { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<ModelPart> ModelParts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MyPart> MyParts { get; set; }
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Part)
                .WithMany() // Add if Part has no collection of OrderItems
                .HasForeignKey(oi => oi.PartId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
            modelBuilder.Entity<DashboardStats>().HasNoKey();
            var dataSeeder = new DataSeeder();
            dataSeeder.DataSeed(modelBuilder);

            // opcija kod nasljeđivanja
            // modelBuilder.Entity<NekaBaznaKlasa>().UseTpcMappingStrategy();
        }
    }
}
