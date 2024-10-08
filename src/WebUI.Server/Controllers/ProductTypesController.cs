﻿using Domain.Models;
using WebUI.Server.Services.ProductTypeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Server.Controllers;

[Authorize(Roles = "Admin")]
public sealed class ProductTypesController(
    IProductTypeService productTypeService) : ControllerTemplate
{
    private readonly IProductTypeService _productTypeService = productTypeService;

    [HttpGet]
    public async Task<ActionResult<List<ProductType>>> GetProductTypes() =>
        HandleResult(await _productTypeService.GetProductTypesAsync());

    [HttpPost]
    public async Task<ActionResult<List<ProductType>>> AddProductType(ProductType productType) =>
        HandleResult(await _productTypeService.AddProductType(productType));

    [HttpPut]
    public async Task<ActionResult<List<ProductType>>> UpdateProductType(ProductType productType) =>
        HandleResult(await _productTypeService.UpdateProductType(productType));

    [HttpDelete("{productTypeId}")]
    public async Task<ActionResult<List<ProductType>>> DeleteProductType(int productTypeId) =>
        HandleResult(await _productTypeService.DeleteProductType(productTypeId)); 
}
