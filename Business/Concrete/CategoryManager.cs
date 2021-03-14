using Business.Abstract;
using Core.Utilities.Results;
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

        public IResult Add(Category category)
        {
            _categorydal.Add(category);
            return new SuccessResult();
        }

        public IDataResult< List<Category>> GetAll()
        {
            return new SuccessDataResult<List<Category>>( _categorydal.GetAll());
        }

        public IDataResult<Category> GetById(int categoryId)
        {
            return new SuccessDataResult<Category>( _categorydal.Get(c=>c.CategoryId==categoryId));
        }
    }
}
