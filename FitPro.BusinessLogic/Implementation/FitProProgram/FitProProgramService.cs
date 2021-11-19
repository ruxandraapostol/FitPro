using FitPro.Common;
using FitPro.DataAccess;
using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitPro.BusinessLogic
{
    public class FitProProgramService : BaseService
    {
        CreateProgramValitaion createProgramValidator;
        public FitProProgramService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            createProgramValidator = new CreateProgramValitaion();
        }

        public void PopulateWorkoutCategoryDropDown(CreateProgramModel model)
        {
            model.Categories = UnitOfWork.CategoriesW.Get()
                .Select(x => new ListItemModel<string, Guid>()
                {
                    Text = x.Name,
                    Value = x.IdCategory
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<FitProProgramModel> GetUserCurrentPrograms(Guid userId)
        {
            return UnitOfWork.RegularUserFitProPrograms.Get()
                .Include(x => x.IdProgramNavigation)
                .ThenInclude(x => x.FitProProgramWorkouts)
                .Where(x => x.IdRegularUser == userId)
                .OrderByDescending(x => x.StartDate)
                .Take(9)
                .Select(x => new FitProProgramModel()
                {
                    ProgramId = x.IdProgram,
                    StartDay = x.StartDate,
                    LinkUrl = x.IdProgramNavigation.FitProProgramWorkouts
                                .FirstOrDefault(a => a.IdProgram == x.IdProgram && a.DayNumber == 1)
                                .IdWorkoutNavigation.LinkUrl,
                    Categories = x.IdProgramNavigation.FitProProgramCategories
                                .Select(x => x.IdCategoryNavigation.Name)
                                .ToList()
                }).ToList();
        }

        public FitProProgramModel GetUserCurrentProgram(Guid userId, Guid programId)
        {
            var program = UnitOfWork.RegularUserFitProPrograms.Get()
                .Include(x => x.IdProgramNavigation)
                .ThenInclude(x => x.FitProProgramWorkouts)
                .ThenInclude(x => x.IdWorkoutNavigation)
                .Where(x => x.IdRegularUser == userId && x.IdProgram == programId)
                .Select(x => new FitProProgramModel()
                {
                    ProgramId = x.IdProgram,
                    StartDay = x.StartDate,
                    CurrentDay = x.CurrentDay,
                    Categories = x.IdProgramNavigation.FitProProgramCategories
                                .Select(x => x.IdCategoryNavigation.Name)
                                .ToList()
                })
                .SingleOrDefault();

            program.WorkoutsList = GetProgramWorkouts(programId, 1);

            return program;
        }

        public List<ProgramWorkoutModel> GetProgramWorkouts(Guid programId, int currentPage)
        {
            var rawList = UnitOfWork.FitProProgramWorkouts.Get()
                    .Where(x => x.IdProgram == programId)
                    .OrderBy(x => x.DayNumber);

            if (currentPage > 1)
            {
                rawList = (IOrderedQueryable<FitProProgramWorkout>)rawList.Skip((currentPage - 1) * 5);
            }

            var finalList = rawList.Take(5)
                .Select(x => new ProgramWorkoutModel() {
                        Day = x.DayNumber,
                        IdWorkout = x.IdWorkoutNavigation.IdWorkout,
                        Name = x.IdWorkoutNavigation.Name,
                        LinkUrl = x.IdWorkoutNavigation.LinkUrl
                })
                .ToList();

            return finalList;
        }


        public bool CreateFitProProgram(Guid userId, CreateProgramModel model)
        {
            ExecuteInTransaction(uow =>
            {
                createProgramValidator.Validate(model).ThenThrow(model);
                bool check = false;
                var existingProgram = checkNotAlreadyExistWantedProgram(uow, model);

                if (existingProgram != Guid.Empty)
                {
                    InsertRegularUserProgram(uow, userId, existingProgram);
                }
                else
                {
                        // creez programul si il inserez
                        var newFitProProgram = new FitProProgram()
                    {
                        IdProgram = Guid.NewGuid(),
                        TimePeriod = model.TimePeriod
                    };
                    uow.FitProPrograms.Insert(newFitProProgram);

                        // adaug categoriile programului
                        InsertProgramCategory(uow, model.SelectedCategories, newFitProProgram.IdProgram);

                        //fac legatura dintre program si regularuser
                        InsertRegularUserProgram(uow, userId, newFitProProgram.IdProgram);

                        // imi aleg noile workout-uri
                        check = ChooseWorkouts(uow, model.SelectedCategories, model.TimePeriod * 7, newFitProProgram.IdProgram);
                }

                uow.SaveChanges();
                return check;
            });

            return false;
        }

        private void InsertProgramCategory(UnitOfWork uow, List<Guid> selectedCategories, Guid programId)
        {
            foreach (var category in selectedCategories)
            {
                uow.FitProProgramCategory.Insert(new FitProProgramCategory()
                {
                    IdProgram = programId,
                    IdCategory = category
                });
            }
        }

        private void InsertRegularUserProgram(UnitOfWork uow, Guid userId, Guid programId)
        {
            uow.RegularUserFitProPrograms.Insert(new RegularUserFitProProgram()
            {
                IdRegularUser = userId,
                IdProgram = programId
            });
        }

        private bool ChooseWorkouts(UnitOfWork uow, List<Guid> selectedCategories, int numberOfDays, Guid idProgram)
        {
            var possibleWorkoutsForProgram = uow.WorkoutCategories.Get()
                    .Include(x => x.IdWorkoutNavigation)
                    .Where(x => selectedCategories.Contains(x.IdCategory))
                    .Select(x => x.IdWorkout)
                    .ToList();

            if (possibleWorkoutsForProgram.Count() == 0)
            {
                return false;
            }

            for (int i = 1; i <= numberOfDays; i++)
            {
                if (i % 4 != 0)
                {
                    var currentChooseList = possibleWorkoutsForProgram
                        .OrderBy(arg => Guid.NewGuid()).Take(Math.Min(Math.Min(3, possibleWorkoutsForProgram.Count()), 1 + i / 7))
                        .Distinct()
                        .ToList();

                    currentChooseList.ForEach(workout =>
                        uow.FitProProgramWorkouts.Insert(new FitProProgramWorkout()
                        {
                            IdProgram = idProgram,
                            IdWorkout = workout,
                            DayNumber = i
                        }));
                }
            }

            return true;
        }

        private Guid checkNotAlreadyExistWantedProgram(UnitOfWork uow, CreateProgramModel model)
        {
            var existingWantedPrograms = uow.FitProPrograms.Get()
                .Include(x => x.FitProProgramCategories)
                .Where(x => x.TimePeriod == model.TimePeriod)
                .Select(x => new
                {
                    IdProgram = x.IdProgram,
                    Categories = x.FitProProgramCategories.Select(a => a.IdCategory).ToList()
                })
                .Where(x => model.SelectedCategories.Count() == x.Categories.Count());

            foreach (var possibleProgram in existingWantedPrograms)
            {
                if (!model.SelectedCategories.Except(possibleProgram.Categories).Any()
                    && !possibleProgram.Categories.Except(model.SelectedCategories).Any())
                {
                    return possibleProgram.IdProgram;
                }
            }

            return Guid.Empty;
        }
    }
}
