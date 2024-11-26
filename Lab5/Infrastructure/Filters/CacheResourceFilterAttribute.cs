using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace Lab4.Infrastructure.Filters
{
    // Простой фильтр ресурсов, кэширующий ViewResult
    public class CacheResourceFilterAttribute : Attribute, IResourceFilter
    {
        private static readonly Dictionary<string, object> _cache = new Dictionary<string, object>(); // Коллекция в виде словаря, в которую будем кэшировать данные
        private string _cacheKey; // ключ доступа

        // Выполняется до выполнения метода контроллера
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // считывание представление из коллекции
            _cacheKey = context.HttpContext.Request.Path.ToString();
            if (_cache.TryGetValue(_cacheKey, out object value))
            {
                if (value is ViewResult cachedValue)
                {
                    context.Result = cachedValue;
                }
            }
        }

        // Выполняется после выполнения метода контроллера
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // запись представления в коллекцию
            if (!String.IsNullOrEmpty(_cacheKey) && !_cache.ContainsKey(_cacheKey))
            {
                if (context.Result is ViewResult result)
                {
                    _cache.Add(_cacheKey, result);
                }
            }
        }
    }
}
