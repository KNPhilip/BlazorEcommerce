﻿using Domain.Dtos;
using Domain.Models;

namespace WebUI.Server.Services.CategoryService;

public interface ICategoryService
{
    Task<ResponseDto<List<Category>>> GetCategoriesAsync();
    Task<ResponseDto<List<Category>>> GetAdminCategoriesAsync();
    Task<ResponseDto<List<Category>>> AddCategoryAsync(Category category);
    Task<ResponseDto<List<Category>>> UpdateCategoryAsync(Category category);
    Task<ResponseDto<List<Category>>> DeleteCategoryAsync(int categoryId);
}
