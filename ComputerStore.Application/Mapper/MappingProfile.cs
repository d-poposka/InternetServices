using Application.DTOs;
using AutoMapper;
using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ComputerStore.Application.DTOs.CreateProductRequest;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
          //  CreateMap<CreateProductRequest, Product>();
            CreateMap<ProductDTO, Product>();
            CreateMap<ProductDTO, Product>()
           .ForMember(dest => dest.categories, opt => opt.MapFrom(src => src.categories.Select(c => new Category { Name = c })));
            CreateMap<Product, ProductResponse>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Product,ProductWithoutCategoryDTO>();
            

        }
    }
    }
