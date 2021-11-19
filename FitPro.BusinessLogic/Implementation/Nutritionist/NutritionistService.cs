using FitPro.Common;
using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class NutritionistService : BaseService
    {
        private AddAlimentValidation AddAlimentValidator;
        private EditAlimentValidation EditAlimentValidator;
        private DeleteAlimentValidation DeleteAlimentValidator;
        private AddRecipeValidation AddRecipeValidator;
        private EditRecipeValidation EditRecipeValidator;
        private DeleteRecipeValidation DeleteRecipeValidator;

        public NutritionistService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            AddAlimentValidator = new AddAlimentValidation(this);
            EditAlimentValidator = new EditAlimentValidation(this);
            DeleteAlimentValidator = new DeleteAlimentValidation(this);
            AddRecipeValidator = new AddRecipeValidation(this);
            EditRecipeValidator = new EditRecipeValidation(this);
            DeleteRecipeValidator = new DeleteRecipeValidation(this);
        }

        public bool AlimentNotAlreadyExist(string alimentName)
        {
            return UnitOfWork.Aliments.Get().SingleOrDefault(x => x.Name == alimentName) == null;
        }

        public List<DetailAlimentModel> GetAlimentsList(int currentPage, string searchString)
        {
            var AlimentIntermediateList = UnitOfWork.Aliments.Get()
                .OrderBy(x => x.Name)
                .Select(x => new DetailAlimentModel()
                {
                    Name = x.Name,
                    Calories = x.Calories,
                    Protein = x.Protein,
                    Fat = x.Fat,
                    Carbo = x.Carbo,
                    IdNutritionist = x.IdNutritionist
                });

            if (searchString != null)
            {
                AlimentIntermediateList = AlimentIntermediateList.Where(x => x.Name.Contains(searchString));
            }

            if (currentPage > 1)
            {
                return AlimentIntermediateList
                .Skip((currentPage - 1) * 15)
                .Take(15)
                .ToList();
            }
            else
            {
                return AlimentIntermediateList
                .Take(15)
                .ToList();
            }
        }

        public AlimentsListModel GetAlimentsListModel(int currentPage, string searchString)
        {
            var model = new AlimentsListModel()
            {
                CurrentPage = currentPage,
                SearchString = searchString,
            };

            model.AlimentList = GetAlimentsList(currentPage, searchString);

            return model;
        }

        public void AddAliment(AddAlimentModel model)
        {
            ExecuteInTransaction(uow =>
            {
                AddAlimentValidator.Validate(model).ThenThrow(model);

                var newAliment = Mapper.Map<AddAlimentModel, Aliment>(model);
                newAliment.IdAliment = Guid.NewGuid();

                uow.Aliments.Insert(newAliment);
                uow.SaveChanges();
            });
        }

        public EditAlimentModel GetAlimentByName(string alimentName)
        {
            var aliment = UnitOfWork.Aliments.Get()
                .SingleOrDefault(x => x.Name == alimentName);

            return Mapper.Map<Aliment, EditAlimentModel>(aliment);
        }

        public void EditAliment(EditAlimentModel model)
        {
            ExecuteInTransaction(uow =>
            {
                EditAlimentValidator.Validate(model).ThenThrow(model);

                var oldAliment = uow.Aliments.Get()
                    .SingleOrDefault(x => x.Name == model.Name);
                oldAliment.Calories = model.Calories;
                oldAliment.Protein = model.Protein;
                oldAliment.Fat = model.Fat;
                oldAliment.Carbo = model.Carbo;

                uow.Aliments.Update(oldAliment);
                uow.SaveChanges();
            });
        }

        public DeleteAlimentModel GetDeleteAlimentModelByName(string alimentName)
        {
            var aliment = UnitOfWork.Aliments.Get()
               .SingleOrDefault(x => x.Name == alimentName);

            return Mapper.Map<Aliment, DeleteAlimentModel>(aliment);
        }

        public void DeleteAliment(DeleteAlimentModel model)
        {
            ExecuteInTransaction(uow =>
            {
                DeleteAlimentValidator.Validate(model).ThenThrow(model);

                var aliment = UnitOfWork.Aliments.Get()
                    .SingleOrDefault(x => x.Name == model.Name);

                uow.Aliments.Delete(aliment);
                uow.SaveChanges();
            });
        }


        //--------------------------Recipe-----------------------------

        public List<RecipeModel> GetRecipesList(Guid currentUser, int currentPage, FilterRecipeModel filter)
        {
            var list = UnitOfWork.Recipes.Get();
            if(list.Count() == 0)
            {
                return null;
            }

            var newList = (filter.SortColumn != null && filter.SortColumnIndex != null) ? GetRecipesOrdered(filter.SortColumn, filter.SortColumnIndex, list) : list;
            newList = (filter.SearchString != null && filter.SearchString != "") ? GetRecipesFiteredBySearchString(filter.SearchString.ToLower(), newList) : newList;
            newList = (filter.LowerTimeLimit != null && filter.UpperTimeLimit != null) ? GetRecipesFiteredByTime(filter.LowerTimeLimit, filter.UpperTimeLimit, newList) : newList;
            newList = (filter.LowerCaloriesLimit != null && filter.UpperCaloriesLimit != null) ? GetRecipesFiteredByCalories(filter.LowerCaloriesLimit, filter.UpperCaloriesLimit, newList) : newList;
            newList = (filter.SelectedCategories != null && filter.SelectedCategories.Count() != 0) ? GetRecipesFiteredByCategory(filter.SelectedCategories, newList) : newList;
            newList = (filter.SelectedNutritionist != null && filter.SelectedNutritionist.Count() != 0) ? GetRecipesFiteredByNutritionis(filter.SelectedNutritionist, newList) : newList;

            if (currentPage > 1)
            {
                newList = newList.Skip((currentPage - 1) * 9);
            }

            newList = newList.Take(9);

            var recipeList = newList.Select(x => new RecipeModel() { 
                Name = x.Name,
                IdRecipe = x.IdRecipe,
                Time = x.Time
            }).ToList();

            recipeList.ForEach(x =>
            {
                x.IsSaved = UnitOfWork.Saved.Get()
                .SingleOrDefault(a => a.IdRecipe == x.IdRecipe 
                && a.IdRegularUser == currentUser) != null;
            });

            return recipeList;
        }

        public object GetDetailModelById(Guid idRecipe, bool fromSavedItems, bool fromShare)
        {

            var recipe = UnitOfWork.Recipes.Get()
                .Include(x => x.IdCategoryNavigation)
                .Include(x => x.IdNutritionistNavigation)
                .ThenInclude(x => x.IdSpecialUserNavigation)
                .SingleOrDefault(x => x.IdRecipe == idRecipe);

            var detailRecipe = Mapper.Map<Recipe, DetailRecipeModel>(recipe);

            using (var reader = new StringReader(recipe.AlimentsList))
            {
                detailRecipe.IngredientsList = new List<string>();
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    detailRecipe.IngredientsList.Add(line);
                }
            }

            using (var reader = new StringReader(recipe.Preparation))
            {
                detailRecipe.PreparationList = new List<string>();
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    detailRecipe.PreparationList.Add(line);
                }
            }

            detailRecipe.FromSaved = fromSavedItems;
            detailRecipe.FromShare = fromShare;
            detailRecipe.NutritionistName = recipe.IdNutritionistNavigation?.IdSpecialUserNavigation?.UserName;
            detailRecipe.CategoryName = recipe.IdCategoryNavigation?.Name;
            return detailRecipe;
        }

        public RecipeListModel GetRecipeModelList(Guid currentUser)
        {
            var model = new RecipeListModel();
            model.Filter.Categories = PopulateCategoryDropDown();
            model.Filter.Nutritionist = PopulateNutritionistsDropDown();
            model.RecipeList = GetRecipesList(currentUser, 1, model.Filter);

            return model;
        }


        public List<ListItemModel<string, Guid?>> PopulateCategoryDropDown()
        {
            var dropDownList = new List<ListItemModel<string, Guid?>>();
            dropDownList.Add(new ListItemModel<string, Guid?>()
            {
                Text = null,
                Value = null
            });

            var dataBaseDropDown = UnitOfWork.CategoriesR.Get()
               .Select(x => new ListItemModel<string, Guid?>()
               {
                   Text = x.Name,
                   Value = x.IdCategory
               })
               .OrderBy(x => x.Text)
               .ToList();


            return dropDownList.Concat(dataBaseDropDown).ToList();
        }

        public List<ListItemModel<string, Guid?>> PopulateNutritionistsDropDown()
        {
            return UnitOfWork.Users.Get()
                .Include(x => x.IdRoleNavigation)
                .Where(x => x.IdRoleNavigation.Name == "Nutritionist")
               .Select(x => new ListItemModel<string, Guid?>()
               {
                   Text = x.UserName,
                   Value = x.IdUser
               })
               .OrderBy(x => x.Text)
               .ToList();
        }

        public void AddRecipe(AddRecipeModel model)
        {
            model.Categories = PopulateCategoryDropDown();
            ExecuteInTransaction(uow => {
                AddRecipeValidator.Validate(model).ThenThrow(model);

                //var newRecipe = Mapper.Map<AddRecipeModel, Recipe>(model);
                var newRecipe = new Recipe()
                {
                    Name = model.Name,
                    Preparation = model.Preparation,
                    Time = model.Time,
                    Calories = model.Calories,
                    IdNutritionist = model.IdNutritionist,
                    AlimentsList = model.AlimentsList,
                    IdCategory = model.IdCategory
                };

                newRecipe.IdRecipe = Guid.NewGuid();

                if(model.Image != null)
                {
                    newRecipe.Image = ImageConvert(model.Image);
                }
                
                uow.Recipes.Insert(newRecipe);
                uow.SaveChanges();
            });
        }

        public EditRecipeModel GetEditRecipeModelById(Guid recipeId)
        {
            return Mapper.Map<Recipe,EditRecipeModel> (UnitOfWork.Recipes.Get()
                .SingleOrDefault(x => x.IdRecipe == recipeId));
        }

        public void EditRecipe(EditRecipeModel model)
        {
            model.Categories = PopulateCategoryDropDown();
            ExecuteInTransaction(uow => {
                EditRecipeValidator.Validate(model).ThenThrow(model);

                var newRecipe = uow.Recipes.Get()
                .SingleOrDefault(x => x.IdRecipe == model.IdRecipe);

                newRecipe.AlimentsList = model.AlimentsList;
                newRecipe.Preparation = model.Preparation;
                newRecipe.Time = model.Time;
                newRecipe.Calories = model.Calories;
                newRecipe.IdCategory = model.IdCategory;

                if (model.ImageFile != null)
                {
                    newRecipe.Image = ImageConvert(model.ImageFile);
                }

                uow.Recipes.Update(newRecipe);
                uow.SaveChanges();
            });
        }

        public DeleteRecipeModel GetDeleteRecipeModelById(Guid recipeId)
        {
            return Mapper.Map<Recipe, DeleteRecipeModel>(UnitOfWork.Recipes.Get()
                .SingleOrDefault(x => x.IdRecipe == recipeId));
        }

        public void DeleteRecipe(DeleteRecipeModel model)
        {
            ExecuteInTransaction(uow => {
                DeleteRecipeValidator.Validate(model).ThenThrow(model);

                var savedList = uow.Saved.Get()
                    .Where(x => x.IdRecipe == model.IdRecipe);

                foreach(var saved in savedList)
                {
                    uow.Saved.Delete(saved);
                }

                var sharedList = uow.Recommandations.Get()
                    .Where(x => x.IdRecipe == model.IdRecipe);

                foreach (var shared in sharedList)
                {
                    uow.Recommandations.Delete(shared);
                }

                var recipe = uow.Recipes.Get()
                    .SingleOrDefault(x => x.IdRecipe == model.IdRecipe);

                uow.Recipes.Delete(recipe);
                uow.SaveChanges();
            });
        }

        public byte[] GetRecipePicOrDefault(Guid recipeId)
        {
            var image = UnitOfWork.Recipes.Get()
                .SingleOrDefault(u => u.IdRecipe == recipeId)?
                .Image;

            if (image == null)
            {
                var path = "./wwwroot/image/noRecipePhoto.png";
                image = File.ReadAllBytes(path);
            }

            return image;
        }

        private IQueryable<Recipe> GetRecipesFiteredByNutritionis(List<Guid> selectedNutritionist, IQueryable<Recipe> newList)
        {
            return newList.Where(x => selectedNutritionist.Contains(x.IdNutritionist));
        }

        private IQueryable<Recipe> GetRecipesFiteredByCategory(List<Guid> selectedCategories, IQueryable<Recipe> newList)
        {
            return newList.Where(x => selectedCategories.Contains(x.IdCategory ?? Guid.Empty));
        }

        private IQueryable<Recipe> GetRecipesFiteredBySearchString(string lowerSearchString, IQueryable<Recipe> newList)
        {
            return newList.Where(x => x.Name.Contains(lowerSearchString));
        }

        private IQueryable<Recipe> GetRecipesFiteredByTime(int? lowerTime, int? upperTime, IQueryable<Recipe> newList)
        {
            return newList.Where(x => x.Time >= (lowerTime ?? 0) && x.Time <= (upperTime ?? 1000));

        }

        private IQueryable<Recipe> GetRecipesFiteredByCalories(int? lowerCalo, int? upperCalo, IQueryable<Recipe> newList)
        {
            return newList.Where(x => x.Calories >= (lowerCalo ?? 0) && x.Calories <= (upperCalo ?? 1000));
        }

        private IQueryable<Recipe> GetRecipesOrdered(string sortColumn, string sortColumnIndex, IQueryable<Recipe> list)
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

    }
}
