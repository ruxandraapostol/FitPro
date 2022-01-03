using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitPro.Common;
using FitPro.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitPro.BusinessLogic
{
    public class NutritionTrackService : BaseService
    {
        private readonly SaveAlimentTrackValidation SaveValidator;
        public NutritionTrackService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            this.SaveValidator = new SaveAlimentTrackValidation();
        }
        public List<ListItemModel<string, Guid?>> PopulateFoodList()
        {
            return UnitOfWork.Aliments.Get()
               .Select(x => new ListItemModel<string, Guid?>()
               {
                   Text = x.Name,
                   Value = x.IdAliment
               })
               .OrderBy(x => x.Text)
               .ToList();
        }

        public void ChangeActiveDay(Guid idRegularUser, DateTime date)
        {
            ExecuteInTransaction(uow =>
            {
                var activeDay = uow.UserActiveDays.Get()
                .FirstOrDefault(uad => uad.IdRegularUser == idRegularUser
                    && uad.Date == date);

                if(activeDay != null)
                {
                        uow.UserActiveDays.Delete(activeDay);
                } 
                else
                {
                    var newActiveDay = new UserActiveDays()
                    {
                        IdRegularUser = idRegularUser,
                        Date = date
                    };
                        uow.UserActiveDays.Insert(newActiveDay);
                }
                uow.SaveChanges();
            });
        }

        public DailyListModel GetDailyList(DateTime date, Guid idRegularUser)
        {
            DailyListModel model = new DailyListModel() { Date = date };

            model.AlimentTrackList = UnitOfWork.AlimentRegularUsers.Get()
                .Include(aru => aru.IdAlimentNavigation)
                .Where(aur => aur.IdRegularUser == idRegularUser
                    && aur.Date.Date == date.Date)
                .Select(aru => new AlimentTrackModel()
                {
                    IdAliment = aru.IdAliment,
                    Date = aru.Date,
                    Quantity = aru.Quantity,
                    AlimentName = aru.IdAlimentNavigation.Name,
                    TotalCalories = aru.IdAlimentNavigation.Calories / 100 * aru.Quantity,
                    TotalProt = aru.IdAlimentNavigation.Protein / 100 * aru.Quantity,
                    TotalFats = aru.IdAlimentNavigation.Fat / 100 * aru.Quantity,
                    TotalCarbs = aru.IdAlimentNavigation.Carbo / 100 * aru.Quantity
                }).ToList();

            model.ActiveDay = UnitOfWork.UserActiveDays.Get()
                .Any(uad => uad.IdRegularUser == idRegularUser
                    && uad.Date == date);

            model.TotalCalories = model.AlimentTrackList.Sum(x => x.TotalCalories);
            model.TotalFats = model.AlimentTrackList.Sum(x => x.TotalFats);
            model.TotalProt = model.AlimentTrackList.Sum(x => x.TotalProt);
            model.TotalCarbs = model.AlimentTrackList.Sum(x => x.TotalCarbs);

            var user = UnitOfWork.RegularUsers.Get()
                .FirstOrDefault(r => r.IdRegularUser == idRegularUser);

            if (user.BirthDate != null && user.Height != null
                && user.Weight != null)
            {
                var userAge = date.Year - user.BirthDate?.Year;
                var bmr = 10 * user.Weight + 6.25 * user.Height - 5 * userAge;

                model.RecommendedCalories = (user.Gender == (int)Gender.Woman) ?
                    (double)bmr - 161 : (double)bmr + 5;
            }
            else
            {
                model.RecommendedCalories = (user.Gender == (int)Gender.Woman) ? 2000 : 2500;
            }

            model.RecommendedCalories *= model.ActiveDay ? 1.75 : 1.2;
            model.RecommendedCalories = Math.Round(model.RecommendedCalories);
            model.RecommendedCarbs = Math.Round(0.45 * model.RecommendedCalories / 4);
            model.RecommendedFats = Math.Round(0.25 * model.RecommendedCalories / 9);
            model.RecommendedProt = Math.Round(0.3 * model.RecommendedCalories / 4);

            return model;
        }

        public void AddAlimentTrack(SaveAlimentTrackModel model)
        {
            ExecuteInTransaction(uow =>
            {
                SaveValidator.Validate(model).ThenThrow(model);

                
                var oldAliment = uow.AlimentRegularUsers.Get()
                    .SingleOrDefault(a => a.IdAliment == model.IdAliment
                        && a.IdRegularUser == model.IdRegularUser
                        && a.Date == model.Date);

                if (oldAliment != null)
                {
                    oldAliment.Quantity += model.Quantity;
                } 
                else
                {
                    var newAliment = Mapper.Map<SaveAlimentTrackModel, AlimentRegularUser>(model);

                    uow.AlimentRegularUsers.Insert(newAliment);

                }
                uow.SaveChanges();
            });
        }

        public SaveAlimentTrackModel GetEditAlimentTrackModel(Guid idAliment,
            Guid idRegularUser, DateTime date)
        {
            var alimentRegularUser = UnitOfWork.AlimentRegularUsers.Get();

            var alimentRegularUser2 = alimentRegularUser
                .Include(x => x.IdAlimentNavigation)
                .FirstOrDefault(a => a.IdAliment == idAliment
                    && a.IdRegularUser == idRegularUser
                    && a.Date == date);

            return Mapper.Map<AlimentRegularUser, SaveAlimentTrackModel>(alimentRegularUser2);
        }

        public void EditAlimentTrack(SaveAlimentTrackModel model)
        {
            ExecuteInTransaction(uow =>
            {
                SaveValidator.Validate(model).ThenThrow(model);

                var oldAliment = uow.AlimentRegularUsers.Get()
                    .SingleOrDefault(a => a.IdAliment == model.IdAliment
                        && a.IdRegularUser == model.IdRegularUser
                        && a.Date == model.Date);

                if (oldAliment != null)
                {
                    oldAliment.Quantity = model.Quantity;
                    uow.AlimentRegularUsers.Update(oldAliment);
                    uow.SaveChanges();
                }
            });
        }

        public void DeleteAlimentTrack(Guid idAliment,
            Guid idRegularUser, DateTime date)
        {
            ExecuteInTransaction(uow =>
            {
                var oldAliment = uow.AlimentRegularUsers.Get()
                    .SingleOrDefault(a => a.IdAliment == idAliment
                        && a.IdRegularUser == idRegularUser
                        && a.Date == date);

                if (oldAliment != null)
                {
                    uow.AlimentRegularUsers.Delete(oldAliment);
                    uow.SaveChanges();
                }
            });
        }

        public CalendarMonthModel GetViewMonthCalendar(Guid idRegularUser, int year, int month)
        {
            var dateTimeInfo = new DateTimeFormatInfo();

            var calendarMonth = new CalendarMonthModel()
            {
                Month = dateTimeInfo.GetAbbreviatedMonthName(month),
                Year = year,
                Days = new List<CalendarDayModel>()
            };

            var daysNumber = DateTime.DaysInMonth(year, month);
            var firstDay = new DateTime(year, month, 1);

            for(int i = 1; i < (int)firstDay.DayOfWeek; i ++)
            {
                calendarMonth.Days.Add(null);
            }

            for(int i = 1; i <= daysNumber; i++)
            {
                var currentDay = new DateTime(year, month, i);

                var calendarDay = new CalendarDayModel();

                calendarDay.DailyList = GetDailyList(currentDay, idRegularUser);

                if(currentDay > DateTime.Now)
                {
                    calendarDay.ColorCode = "#dee2e6";
                } 
                else
                {
                    if(calendarDay.DailyList.RecommendedCalories < calendarDay.DailyList.TotalCalories)
                    {
                        calendarDay.ColorCode = "#a68a64";
                    } else
                    {
                        calendarDay.ColorCode = "#a4ac86";
                    }
                }
                calendarMonth.Days.Add(calendarDay);
            }

            return calendarMonth;
        }

        public DateTime NavigateMonth(bool prev, string monthName, int year)
        {
            int month = DateTime.Parse("1," + monthName + " 2022").Month;
            var date = prev ? new DateTime(year, month, 1)
                : new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return prev ? date.AddDays(-1) : date.AddDays(1);
        }
    }

}
