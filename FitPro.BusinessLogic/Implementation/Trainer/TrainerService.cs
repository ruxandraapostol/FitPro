using FitPro.Common;
using FitPro.DataAccess;
using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitPro.BusinessLogic
{
    public class TrainerService : BaseService
    {
        private readonly AddWorkoutValidation WorkoutValidator;
        private readonly EditWorkoutValidation EditWorkoutValidator;
        private readonly DeleteWorkoutValidation DeleteWorkoutValidator;
        public TrainerService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            WorkoutValidator = new AddWorkoutValidation(this);
            EditWorkoutValidator = new EditWorkoutValidation(this);
            DeleteWorkoutValidator = new DeleteWorkoutValidation(this);
        }


        public List<ListItemModel<String, Guid>> PopulateWorkoutCategoryDropDown()
        {
            return UnitOfWork.CategoriesW.Get()
                .Select(x => new ListItemModel<string, Guid>()
                {
                    Text = x.Name,
                    Value = x.IdCategory
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<ListItemModel<String, Guid>> PopulateTrainersDropDown()
        {
            return UnitOfWork.Users.Get()
                .Include(x => x.IdRoleNavigation)
                .Where(x => x.IdRoleNavigation.Name == "Trainer")
                .Select(x => new ListItemModel<string, Guid>()
                {
                    Text = x.UserName,
                    Value = x.IdUser
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public void PopulateWorkoutCategoryDropDown(EditWorkoutModel model)
        {
            var id = UnitOfWork.Workouts.Get().SingleOrDefault(w => w.LinkUrl == model.LinkUrl)?.IdWorkout;
            model.Categories = UnitOfWork.CategoriesW.Get()
                .Select(x => new ListItemModel<string, Guid>()
                {
                    Text = x.Name,
                    Value = x.IdCategory,
                    Selected = UnitOfWork.WorkoutCategories.Get()
                    .Any(a => a.IdWorkout == id && a.IdCategory == x.IdCategory)
                })
                .OrderBy(x => x.Text)
                .ToList();
        }


        public List<WorkoutModel> GetWorkoutsList(Guid currentUser, int currentPage, FiltersModel filter) 
        {
            var lowerSearchString = filter.SearchString.ToLower();
            var list = UnitOfWork.Workouts.Get();

            var newList = GetWorkoutsOrdered(filter.SortColumn, filter.SortColumnIndex, list);
            
            newList = GetWorkoutsFiteredBySearchString(lowerSearchString, newList);
            newList = GetWorkoutsFiteredByTime(filter.LowerTimeLimit, filter.UpperTimeLimit, newList);
            newList = GetWorkoutsFiteredByCalories(filter.LowerCaloriesLimit, filter.UpperCaloriesLimit, newList);
            newList = GetWorkoutsFiteredByCategories(filter.SelectedCategories, newList);
            newList = GetWorkoutsFiteredByTrainers(filter.SelectedTrainers, newList);

            if (currentPage > 1)
            {
                newList = newList.Skip((currentPage - 1) * 9);
            }

            newList = newList.Take(9);

            var workoutsList = newList.Select(x => Mapper.Map<Workout, WorkoutModel>(x)).ToList();
        
            workoutsList.ForEach(x =>
            {
                x.IsSaved = UnitOfWork.Saved.Get()
                .SingleOrDefault(a => a.IdWorkout == x.IdWorkout
                && a.IdRegularUser == currentUser) != null;
            });
            return workoutsList;
        }

        public WorkoutsListModel GetWorkoutsListModel(Guid currentUser)
        {
            var model = new WorkoutsListModel();
            model.FilterMore.Categories = PopulateWorkoutCategoryDropDown();
            model.FilterMore.Trainers = PopulateTrainersDropDown();

            var list = UnitOfWork.Workouts.Get()
                .Include(x => x.IdTrainerNavigation)
                .ThenInclude(x => x.IdSpecialUserNavigation)
                .Select(x => Mapper.Map<Workout, WorkoutModel>(x));

            model.WorkoutsList = list
                .Take(9)
                .ToList();

            model.WorkoutsList.ForEach(x =>
            {
                x.IsSaved = (UnitOfWork.Saved.Get()
                    .SingleOrDefault(a => a.IdRegularUser == currentUser &&
                    a.IdWorkoutNavigation.LinkUrl == x.LinkUrl)) == null ? false : true;
            });

            return model;
        }

        public void AddWorkout(AddWorkoutModel model)
        {
            ExecuteInTransaction(uow =>
            {
                WorkoutValidator.Validate(model).ThenThrow(model);

                var workout = Mapper.Map<AddWorkoutModel, Workout>(model);
                workout.IdWorkout = Guid.NewGuid();
                workout.IdTrainerNavigation = UnitOfWork.SpecialUsers.Get().SingleOrDefault(s => s.IdSpecialUser == model.IdTrainer);
                workout.LastModified = workout.LastModified;
                workout.IdLastModifiedNavigation = workout.IdTrainerNavigation;
                uow.Workouts.Insert(workout);

                foreach(var category in model.SelectedCategories)
                {
                    var newConnection = AddWorkoutCategoryConnection(category, workout.IdWorkout);
                    uow.WorkoutCategories.Insert(newConnection);
                }

                uow.SaveChanges();
            });
        }

        public DetailWorkoutModel GetDetailWorkoutModel(string workoutLink, Guid programId, Guid userId, bool fromSaved, bool fromShare)
        {
            var workout = UnitOfWork.Workouts.Get().SingleOrDefault(w => workoutLink == w.LinkUrl);
            var model = Mapper.Map<Workout, DetailWorkoutModel>(workout);

            model.Categories = UnitOfWork.WorkoutCategories.Get()
                .Where(w => w.IdWorkout == workout.IdWorkout)
                .Select(w => w.IdCategoryNavigation.Name)
                .ToList();

            model.AuthorTrainer = UnitOfWork.Users.Get().SingleOrDefault(s => s.IdUser == workout.IdTrainer)?.UserName;
            model.LastModifiedTrainer = UnitOfWork.Users.Get().SingleOrDefault(s => s.IdUser == workout.LastModified)?.UserName;
            model.ProgramId = programId;
            model.UserId = userId;
            model.FromSaved = fromSaved;
            model.FromShare = fromShare;
            return model;
        }

        public EditWorkoutModel GetEditWorkoutModel(string workoutLink)
        {
            var workout = UnitOfWork.Workouts.Get().SingleOrDefault(w => workoutLink == w.LinkUrl);
            var model = Mapper.Map<Workout, EditWorkoutModel>(workout);

            PopulateWorkoutCategoryDropDown(model);

            model.Author = UnitOfWork.Users.Get().SingleOrDefault(u => u.IdUser == workout.IdTrainer)?.UserName;
            model.SelectedCategories = model.Categories
                .Where(x => x.Selected)
                .Select(x => x.Value)
                .ToList();
            
            return model;
        }

        public void EditWorkout(EditWorkoutModel model)
        {
            ExecuteInTransaction(uow =>
            {
                EditWorkoutValidator.Validate(model).ThenThrow(model);

                var workout = UnitOfWork.Workouts.Get().SingleOrDefault(w => model.LinkUrl == w.LinkUrl);

                workout.Description = model.Description;
                workout.Time = model.Time;
                workout.Calories = model.Calories;
                workout.LastModified = model.LastModifiedBy;
                workout.IdLastModifiedNavigation = UnitOfWork.SpecialUsers.Get()
                    .SingleOrDefault(w => model.LastModifiedBy == w.IdSpecialUser);
                
                uow.Workouts.Update(workout);

                foreach (var category in model.Categories)
                {
                    if (model.SelectedCategories.Contains(category.Value) && !category.Selected) 
                    {
                        var newConnection = AddWorkoutCategoryConnection(category.Value, workout.IdWorkout);
                        uow.WorkoutCategories.Insert(newConnection);

                    }
                    else if (!model.SelectedCategories.Contains(category.Value) && category.Selected)
                    {
                        var oldWorkoutCategory = uow.WorkoutCategories.Get()
                            .SingleOrDefault(x => x.IdWorkout == workout.IdWorkout
                                && x.IdCategory == category.Value);

                        category.Selected = false;
                        uow.WorkoutCategories.Delete(oldWorkoutCategory);
                    }
                }

                uow.SaveChanges();
            });
        }

        public DeleteWorkoutModel GetDeleteWorkoutModel(string workoutLink, Guid currentId)
        {
            var workout = UnitOfWork.Workouts.Get()
                .SingleOrDefault(x => x.LinkUrl == workoutLink);

            var model = Mapper.Map<Workout, DeleteWorkoutModel>(workout);

            if(workout.IdTrainer == null)
            {
                model.IdTrainer = currentId;
            } else
            {
                model.Author = UnitOfWork.Users.Get().SingleOrDefault(x => x.IdUser == model.IdTrainer).UserName;
            }
            return model;
        }

        public void DeleteWorkout(DeleteWorkoutModel model)
        {
            ExecuteInTransaction(uow =>
            {
                DeleteWorkoutValidator.Validate(model).ThenThrow(model);

                var workout = uow.Workouts.Get().SingleOrDefault(w => model.LinkUrl == w.LinkUrl);
                
                foreach(var elem in uow.WorkoutCategories.Get()
                                        .Where(x => x.IdWorkout == workout.IdWorkout))
                {
                    uow.WorkoutCategories.Delete(elem);
                }

                foreach (var saved in uow.Saved.Get()
                            .Where(x => x.IdWorkout == workout.IdWorkout))
                {
                    uow.Saved.Delete(saved);
                }

                foreach (var shared in uow.Recommandations.Get()
                            .Where(x => x.IdWorkout == workout.IdWorkout))
                {
                    uow.Recommandations.Delete(shared);
                }


                var programWorkouts = uow.FitProProgramWorkouts.Get()
                    .Include(x => x.IdProgramNavigation)
                    .ThenInclude(x => x.FitProProgramCategories)
                    .Where(x => x.IdWorkout == workout.IdWorkout)
                    .ToList();

                foreach (var elem in programWorkouts)
                {
                    ReplaceWorkoutInProgram(uow, elem.IdProgramNavigation, 
                        elem.DayNumber, workout.IdWorkout);
                    uow.FitProProgramWorkouts.Delete(elem);
                }

                uow.Workouts.Delete(workout);
                uow.SaveChanges();
            });
        }

        private bool ReplaceWorkoutInProgram(UnitOfWork uow, FitProProgram fitProProgram, int dayNumber, Guid idWorkout)
        {
            var SelectedCategories = fitProProgram.FitProProgramCategories
                .Select(x => x.IdCategory)
                .ToList();

            var possibleWorkoutsForProgram = uow.WorkoutCategories.Get()
                    .Include(x => x.IdWorkoutNavigation)
                    .Where(x => SelectedCategories.Contains(x.IdCategory) && x.IdWorkout != idWorkout)
                    .Select(x => x.IdWorkout)
                    .ToList();

            var dayWorkouts = uow.FitProProgramWorkouts.Get()
                .Where(x => x.IdProgram == fitProProgram.IdProgram
                && x.DayNumber == dayNumber && x.IdWorkout != idWorkout)
                .Select(x => x.IdWorkout)
                .ToList();

            var possibleReplaceWorkout = possibleWorkoutsForProgram.Except(dayWorkouts).ToList();

            if (possibleReplaceWorkout.Count() == 0 && dayWorkouts.Count() > 1)
            {
                return false;
            }

            if (possibleReplaceWorkout.Count() > 0)
            {
                var random = new Random();
                int index = random.Next(possibleReplaceWorkout.Count);
                uow.FitProProgramWorkouts.Insert(new FitProProgramWorkout()
                {
                    IdProgram = fitProProgram.IdProgram,
                    IdWorkout = possibleReplaceWorkout[index],
                    DayNumber = dayNumber
                });
            }

            return true;
        }

        private WorkoutCategory AddWorkoutCategoryConnection(Guid categoryId, Guid workoutId)
        {
            var workoutCategory = new WorkoutCategory()
            {
                IdCategory = categoryId,
                IdWorkout = workoutId,
            };

            return workoutCategory;
        }

        private IQueryable<Workout> GetWorkoutsOrdered(string sortColumn, string sortColumnIndex, IQueryable<Workout> list)
        {
            switch (sortColumn)
            {
                case "Name":
                    return (sortColumnIndex == "Descending") ? list.OrderByDescending(x => x.Name) : list.OrderBy(x => x.Name);
                case "Time":
                    return (sortColumnIndex == "Descending") ? list.OrderByDescending(x => x.Time) : list.OrderBy(x => x.Time);
                case "Calo":
                    return (sortColumnIndex == "Descending") ? list.OrderByDescending(x => x.Calories) : list.OrderBy(x => x.Calories);
                default:
                    return list;
            }
        }

        private IQueryable<Workout> GetWorkoutsFiteredBySearchString(string searchString, IQueryable<Workout> list)
        {
            if (searchString != null)
            {
                var lowerSearchString = searchString.ToLower();
                return list = list.Where(x => x.Name.ToLower().Contains(lowerSearchString));
            }
            return list;
        }

        private IQueryable<Workout> GetWorkoutsFiteredByTime(int? lowerTime, int? upperTime, IQueryable<Workout> list)
        {
            return list.Where(x => x.Time >= (lowerTime ?? 0) && x.Time <= (upperTime ?? 1000));
        }

        private IQueryable<Workout> GetWorkoutsFiteredByCalories(int? lowerCalo, int? upperCalo, IQueryable<Workout> list)
        {
            return list.Where(x => x.Calories >= (lowerCalo ?? 0) && x.Calories <= (upperCalo ?? 1000));
        }
    
        private IQueryable<Workout> GetWorkoutsFiteredByCategories(List<Guid> categories, IQueryable<Workout> list)
        {
            if (categories.Count() > 0) {
                list = list.Where(x => x.WorkoutCategories.Any(w => categories.Contains(w.IdCategory)));
            }
            return list;
        }

        private IQueryable<Workout> GetWorkoutsFiteredByTrainers(List<Guid> trainers, IQueryable<Workout> list)
        {
            if (trainers.Count() > 0)
            {
                return list.Where(x => trainers.Contains(x.IdTrainer ?? Guid.Empty));
            }
            return list;
        }


    }
}
