using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categorydal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categorydal = categoryDal;
        }
        public List<Category> GetAll()
        {
            return _categorydal.GetAll();
        }

        public Category GetById(int categoryId)
        {
            return _categorydal.Get(c=>c.CategoryId==categoryId);
        }
    }
}
