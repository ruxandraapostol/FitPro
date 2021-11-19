using FitPro.DataAcess;
using FitPro.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitPro.DataAccess
{
    public partial class FitProContext : DbContext
    {
        public FitProContext()
        {
        }

        public FitProContext(DbContextOptions<FitProContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aliment> Aliments { get; set; }
        public virtual DbSet<AlimentRegularUser> AlimentRegularUsers { get; set; }
        public virtual DbSet<CategoryR> CategoryRs { get; set; }
        public virtual DbSet<CategoryW> CategoryWs { get; set; }
        public virtual DbSet<FitProProgram> FitProPrograms { get; set; }
        public virtual DbSet<FitProProgramWorkout> FitProProgramWorkouts { get; set; }
        public virtual DbSet<FitProProgramCategory> FitProProgramCategories { get; set; }
        public virtual DbSet<FriendsList> FriendsLists { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<Recommandation> Recommandations { get; set; }
        public virtual DbSet<RegularUser> RegularUsers { get; set; }
        public virtual DbSet<RegularUserFitProProgram> RegularUserFitProPrograms { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Saved> Saveds { get; set; }
        public virtual DbSet<SpecialUser> SpecialUsers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Workout> Workouts { get; set; }
        public virtual DbSet<WorkoutCategory> WorkoutCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AlimentConfiguration());
            modelBuilder.ApplyConfiguration(new AlimentRegularUserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryRConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryWConfiguration());
            modelBuilder.ApplyConfiguration(new FitProProgramConfiguration());
            modelBuilder.ApplyConfiguration(new FitProProgramWorkoutConfiguration());
            modelBuilder.ApplyConfiguration(new FitProProgramCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new FriendsListConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeConfiguration());
            modelBuilder.ApplyConfiguration(new RecommandationConfiguration());
            modelBuilder.ApplyConfiguration(new RegularUserConfiguration());
            modelBuilder.ApplyConfiguration(new RegularUserFitProProgramConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new SavedConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialUserConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutCategoryConfiguration());
        }
    }
}
