using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class FoodsApiLogEvents
    {
        public static readonly int BaseId = 2;

        public static readonly EventId GetAllFoodList
            = CreateEventId(LogEventType.Success, 0, nameof(GetAllFoodList));
        public static readonly EventId GetAllFoodCategories
            = CreateEventId(LogEventType.Success, 1, nameof(GetAllFoodCategories));
        public static readonly EventId GetAllFoodIngredients
            = CreateEventId(LogEventType.Success, 2, nameof(GetAllFoodIngredients));
        public static readonly EventId GetAllFoodIngredientCategories
            = CreateEventId(LogEventType.Success, 3, nameof(GetAllFoodIngredientCategories));

        public static readonly EventId GetFoodByID
            = CreateEventId(LogEventType.Success, 4, nameof(GetFoodByID));
        public static readonly EventId GetFoodCategoryByID
            = CreateEventId(LogEventType.Success, 5, nameof(GetFoodCategoryByID));
        public static readonly EventId GetFoodIngredientByID
            = CreateEventId(LogEventType.Success, 6, nameof(GetFoodIngredientByID));
        public static readonly EventId GetFoodIngredientCategoryByID
            = CreateEventId(LogEventType.Success, 7, nameof(GetFoodIngredientCategoryByID));

        public static readonly EventId AddFood
            = CreateEventId(LogEventType.Success, 8, nameof(AddFood));
        public static readonly EventId AddFoodCategory
            = CreateEventId(LogEventType.Success, 9, nameof(AddFoodCategory));
        public static readonly EventId AddFoodIngredient
            = CreateEventId(LogEventType.Success, 10, nameof(AddFoodIngredient));
        public static readonly EventId AddFoodIngredientCategory
            = CreateEventId(LogEventType.Success, 11, nameof(AddFoodIngredientCategory));

        public static readonly EventId EditFood
            = CreateEventId(LogEventType.Success, 12, nameof(EditFood));
        public static readonly EventId EditFoodCategory
            = CreateEventId(LogEventType.Success, 13, nameof(EditFoodCategory));
        public static readonly EventId EditFoodIngredient
            = CreateEventId(LogEventType.Success, 14, nameof(EditFoodIngredient));
        public static readonly EventId EditFoodIngredientCategory
            = CreateEventId(LogEventType.Success, 15, nameof(EditFoodIngredientCategory));

        public static readonly EventId DeleteFood
            = CreateEventId(LogEventType.Success, 16, nameof(DeleteFood));
        public static readonly EventId DeleteFoodCategory
            = CreateEventId(LogEventType.Success, 17, nameof(DeleteFoodCategory));
        public static readonly EventId DeleteFoodIngredient
            = CreateEventId(LogEventType.Success, 18, nameof(DeleteFoodIngredient));
        public static readonly EventId DeleteFoodIngredientCategory
            = CreateEventId(LogEventType.Success, 19, nameof(DeleteFoodIngredientCategory));


        public static readonly EventId FoodNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(FoodNotFound));
        public static readonly EventId FoodCategoryNotFound
            = CreateEventId(LogEventType.ClientError, 1, nameof(FoodCategoryNotFound));
        public static readonly EventId FoodIngredientNotFound
            = CreateEventId(LogEventType.ClientError, 2, nameof(FoodIngredientNotFound));
        public static readonly EventId FoodIngredientCategoryNotFound
            = CreateEventId(LogEventType.ClientError, 3, nameof(FoodIngredientCategoryNotFound));


        public static readonly EventId AddFoodError
            = CreateEventId(LogEventType.ServerError, 0, nameof(AddFoodError));
        public static readonly EventId AddFoodCategoryError
            = CreateEventId(LogEventType.ServerError, 1, nameof(AddFoodCategoryError));
        public static readonly EventId AddFoodIngredientError
            = CreateEventId(LogEventType.ServerError, 2, nameof(AddFoodIngredientError));
        public static readonly EventId AddFoodIngredientCategoryError
            = CreateEventId(LogEventType.ServerError, 3, nameof(AddFoodIngredientCategoryError));

        public static readonly EventId EditFoodError
            = CreateEventId(LogEventType.ServerError, 4, nameof(EditFoodError));
        public static readonly EventId EditFoodCategoryError
            = CreateEventId(LogEventType.ServerError, 5, nameof(EditFoodCategoryError));
        public static readonly EventId EditFoodIngredientError
            = CreateEventId(LogEventType.ServerError, 6, nameof(EditFoodIngredientError));
        public static readonly EventId EditFoodIngredientCategoryError
            = CreateEventId(LogEventType.ServerError, 7, nameof(EditFoodIngredientCategoryError));

        public static readonly EventId DeleteFoodError
            = CreateEventId(LogEventType.ServerError, 8, nameof(DeleteFoodError));
        public static readonly EventId DeleteFoodCategoryError
            = CreateEventId(LogEventType.ServerError, 9, nameof(DeleteFoodCategoryError));
        public static readonly EventId DeleteFoodIngredientError
            = CreateEventId(LogEventType.ServerError, 10, nameof(DeleteFoodIngredientError));
        public static readonly EventId DeleteFoodIngredientCategoryError
            = CreateEventId(LogEventType.ServerError, 11, nameof(DeleteFoodIngredientCategoryError));


        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
