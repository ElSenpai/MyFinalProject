using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;

            _categoryService = categoryService;


        }
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))] 
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                 CheckIfProductNameExists(product.ProductName), CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);


        }

        public IDataResult<List<Product>> GetAll()
        {
            //iş kodları
            //yetkisi varsa
            if (DateTime.Now.Hour == 2)
            {
                return new ErrorDataResult<List<Product>>(Messages.DefaultE);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.DefaultS);

        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(c => c.CategoryId == categoryId).Count;

            if (result >= 10)
            {
                return new ErrorResult(Messages.DefaultE);
            }
            return new SuccessResult(Messages.DefaultS);
        }
        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();

            if (result)
            {
                return new ErrorResult(Messages.DefaultE);

            }
            return new SuccessResult(Messages.DefaultS);
        }
        private IResult CheckIfCategoryLimitExceded()

        {
            var result = _categoryService.GetAll();

            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.DefaultE);
                
            }
            return new SuccessResult(Messages.DefaultS);
        }
    }
}
