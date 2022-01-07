using FitPro.Common;
using FitPro.Entities;

namespace FitPro.DataAccess
{
    public class UnitOfWork
    {
        private readonly FitProContext Context;
        public UnitOfWork(FitProContext context)
        {
            this.Context = context;
        }

        private IRepository<Aliment> aliments;
        public IRepository<Aliment> Aliments => aliments ?? (aliments = new BaseRepository<Aliment>(Context));

        private IRepository<AlimentRegularUser> alimentRegularUsers;
        public IRepository<AlimentRegularUser> AlimentRegularUsers => alimentRegularUsers ?? (alimentRegularUsers = new BaseRepository<AlimentRegularUser>(Context));

        private IRepository<CategoryR> categoriesR;
        public IRepository<CategoryR> CategoriesR => categoriesR ?? (categoriesR = new BaseRepository<CategoryR>(Context));

        private IRepository<CategoryW> categoriesW;
        public IRepository<CategoryW> CategoriesW => categoriesW ?? (categoriesW = new BaseRepository<CategoryW>(Context));

        private IRepository<FitProProgram> fitProPrograms;
        public IRepository<FitProProgram> FitProPrograms => fitProPrograms ?? (fitProPrograms = new BaseRepository<FitProProgram>(Context));

        private IRepository<FitProProgramCategory> fitProProgramCategory;
        public IRepository<FitProProgramCategory> FitProProgramCategory => fitProProgramCategory ?? (fitProProgramCategory = new BaseRepository<FitProProgramCategory>(Context));


        private IRepository<FitProProgramWorkout> fitProProgramWorkouts;
        public IRepository<FitProProgramWorkout> FitProProgramWorkouts => fitProProgramWorkouts ?? (fitProProgramWorkouts = new BaseRepository<FitProProgramWorkout>(Context));

        private IRepository<FriendsList> friendsLists;
        public IRepository<FriendsList> FriendsLists => friendsLists ?? (friendsLists = new BaseRepository<FriendsList>(Context));

        private IRepository<Recipe> recipes;
        public IRepository<Recipe> Recipes => recipes ?? (recipes = new BaseRepository<Recipe>(Context));

        private IRepository<Recommandation> recommandations;
        public IRepository<Recommandation> Recommandations => recommandations ?? (recommandations = new BaseRepository<Recommandation>(Context));

        private IRepository<RegularUser> regularUsers;
        public IRepository<RegularUser> RegularUsers => regularUsers ?? (regularUsers = new BaseRepository<RegularUser>(Context));

        private IRepository<RegularUserFitProProgram> regularUserFitProPrograms;
        public IRepository<RegularUserFitProProgram> RegularUserFitProPrograms => regularUserFitProPrograms ?? (regularUserFitProPrograms = new BaseRepository<RegularUserFitProProgram>(Context));

        private IRepository<Request> requests;
        public IRepository<Request> Requests => requests ?? (requests = new BaseRepository<Request>(Context));

        private IRepository<Role> roles;
        public IRepository<Role> Roles => roles ?? (roles = new BaseRepository<Role>(Context));

        private IRepository<SpecialUser> specialUsers;
        public IRepository<SpecialUser> SpecialUsers => specialUsers ?? (specialUsers = new BaseRepository<SpecialUser>(Context));

        private IRepository<Saved> saved;
        public IRepository<Saved> Saved => saved ?? (saved = new BaseRepository<Saved>(Context));

        private IRepository<Workout> workouts;
        public IRepository<Workout> Workouts => workouts ?? (workouts = new BaseRepository<Workout>(Context));

        private IRepository<WorkoutCategory> workoutCategories;
        public IRepository<WorkoutCategory> WorkoutCategories => workoutCategories ?? (workoutCategories = new BaseRepository<WorkoutCategory>(Context));

        private IRepository<User> users;
        public IRepository<User> Users => users ?? (users = new BaseRepository<User>(Context));

        private IRepository<UserActiveDays> userActiveDays;
        public IRepository<UserActiveDays> UserActiveDays => userActiveDays ?? (userActiveDays = new BaseRepository<UserActiveDays>(Context));


        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
