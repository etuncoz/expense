using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ExpenseApp.Data.Models;
using ExpenseApp.Engine.Domain;

namespace ExpenseApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //DbModel to Dto
            Mapper.CreateMap<ExpenseItem, ExpenseItemDto>();
            Mapper.CreateMap<Expense, ExpenseDto>();

            //Dto to DbModel
            Mapper.CreateMap<ExpenseItemDto, ExpenseItem>();
            Mapper.CreateMap<ExpenseDto, Expense>();
        }
    }
}