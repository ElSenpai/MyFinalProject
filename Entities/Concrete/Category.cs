
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{ // Çıplak class kalmasın -- bi yere interface yada inherit alsın yoksa problem yaşarsın

  public  class Category :IEntity
    {
        public int CategoryId { get; set; }
        public string  CategoryName { get; set; }

    }
}
