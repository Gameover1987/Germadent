﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.WebApi.Entities;
using Newtonsoft.Json;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations
    {
        public PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.AddPriceGroup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = (int)priceGroupDto.BranchType;
                    command.Parameters.Add(new SqlParameter("@priceGroupName", SqlDbType.NVarChar)).Value = priceGroupDto.Name;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    priceGroupDto.PriceGroupId = command.Parameters["@priceGroupId"].Value.ToInt();

                    return priceGroupDto;
                }
            }
        }

        public PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.UpdatePriceGroup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int)).Value = (int)priceGroupDto.PriceGroupId;
                    command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = priceGroupDto.BranchType;
                    command.Parameters.Add(new SqlParameter("@priceGroupName", SqlDbType.NVarChar)).Value = priceGroupDto.Name;

                    command.ExecuteNonQuery();

                    return priceGroupDto;
                }
            }
        }

        public DeleteResult DeletePriceGroup(int priceGroupId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.UnionPriceGroups", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@oldPriceGroupId", SqlDbType.NVarChar)).Value = priceGroupId;
                    command.Parameters.Add(new SqlParameter("@newPriceGroupId", SqlDbType.NVarChar)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@resultCount", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    var count = command.Parameters["@resultCount"].Value.ToInt();

                    return new DeleteResult()
                    {
                        Id = priceGroupId,
                        Count = count
                    };
                }
            }
        }

        public PricePositionDto AddPricePosition(PricePositionDto pricePositionDto)
        {
            var jsonStringProduct = pricePositionDto.Products.SerializeToJson(Formatting.Indented);
            var jsonStringPrices = pricePositionDto.Prices.SerializeToJson(Formatting.Indented);
            var jsonStringMaterial = pricePositionDto.MaterialId;

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.AddPricePosition", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@pricePositionCode", SqlDbType.NVarChar)).Value = pricePositionDto.UserCode;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int)).Value = pricePositionDto.PriceGroupId;
                    command.Parameters.Add(new SqlParameter("@pricePositionName", SqlDbType.NVarChar)).Value = pricePositionDto.Name;
                    command.Parameters.Add(new SqlParameter("@materialId", SqlDbType.Int)).Value = pricePositionDto.MaterialId.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@jsonStringProduct", SqlDbType.NVarChar)).Value = jsonStringProduct;
                    command.Parameters.Add(new SqlParameter("@jsonStringPrices", SqlDbType.NVarChar)).Value = jsonStringPrices;
                    command.Parameters.Add(new SqlParameter("@pricePositionId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    command.ExecuteNonQuery();

                    pricePositionDto.PricePositionId = command.Parameters["@pricePositionId"].Value.ToInt();

                    return pricePositionDto;
                }
            }
        }

        public PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            var jsonStringProduct = pricePositionDto.Products.SerializeToJson(Formatting.Indented);
            var jsonStringPrices = pricePositionDto.Prices.SerializeToJson(Formatting.Indented);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.UpdatePricePosition", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@pricePositionId", SqlDbType.Int)).Value = pricePositionDto.PricePositionId;
                    command.Parameters.Add(new SqlParameter("@pricePositionCode", SqlDbType.NVarChar)).Value = pricePositionDto.UserCode;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int)).Value = pricePositionDto.PriceGroupId;
                    command.Parameters.Add(new SqlParameter("@pricePositionName", SqlDbType.NVarChar)).Value = pricePositionDto.Name;
                    command.Parameters.Add(new SqlParameter("@materialId", SqlDbType.Int)).Value = pricePositionDto.MaterialId.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@jsonStringProduct", SqlDbType.NVarChar)).Value = jsonStringProduct;
                    command.Parameters.Add(new SqlParameter("@jsonStringPrices", SqlDbType.NVarChar)).Value = jsonStringPrices;

                    command.ExecuteNonQuery();

                    return pricePositionDto;
                }
            }
        }

        public DeleteResult DeletePricePosition(int pricePositionId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.DeletePricePosition", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@pricePositionId", SqlDbType.Int)).Value = pricePositionId;
                    command.Parameters.Add(new SqlParameter("@resultCount", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    var count = command.Parameters["@resultCount"].Value.ToInt();

                    return new DeleteResult()
                    {
                        Id = pricePositionId,
                        Count = count
                    };
                }
            }
        }

        public PriceGroupDto[] GetPriceGroups(BranchType branchType)
        {
            var cmdText = string.Format("select distinct PriceGroupID, PriceGroupName from dbo.GetPriceListForBranch({0})", (int)branchType);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var priceGroupCollection = new List<PriceGroupDto>();
                    while (reader.Read())
                    {
                        var priceGroupEntity = new PriceGroupEntity();

                        priceGroupEntity.PriceGroupId = reader[nameof(priceGroupEntity.PriceGroupId)].ToInt();
                        priceGroupEntity.PriceGroupName = reader[nameof(priceGroupEntity.PriceGroupName)].ToString();

                        var priceGroupDto = _converter.ConvertToPriceGroup(priceGroupEntity);
                        priceGroupDto.BranchType = branchType;
                        priceGroupCollection.Add(priceGroupDto);
                    }
                    reader.Close();

                    return priceGroupCollection.ToArray();
                }
            }
        }

        public PricePositionDto[] GetPricePositions(BranchType branchType)
        {
            var selectPricesCmd = "select * from dbo.Prices";
            var prostheticTypesByPricePositionsCmd = "select * from dbo.GetProductSetsForPricePositions()";
            var cmdText = string.Format("select distinct PricePositionID, PriceGroupID, PricePositionCode, PricePositionName, MaterialID from GetPriceListForBranch({0}) \r\n{1} \r\n{2}", (int)branchType, selectPricesCmd, prostheticTypesByPricePositionsCmd);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var pricePositionCollection = new List<PricePositionDto>();

                    // Get price positions
                    while (reader.Read())
                    {
                        var pricePositionEntity = new PricePositionEntity();

                        var pricePositionId = reader[nameof(pricePositionEntity.PricePositionId)];
                        if (pricePositionId == DBNull.Value)
                            continue;

                        pricePositionEntity.PricePositionId = pricePositionId.ToInt();
                        pricePositionEntity.PriceGroupId = reader[nameof(pricePositionEntity.PriceGroupId)].ToInt();
                        pricePositionEntity.PricePositionCode = reader[nameof(pricePositionEntity.PricePositionCode)].ToString();
                        pricePositionEntity.PricePositionName = reader[nameof(pricePositionEntity.PricePositionName)].ToString();

                        var materialId = reader[nameof(pricePositionEntity.MaterialId)];
                        if (materialId != DBNull.Value)
                            pricePositionEntity.MaterialId = materialId.ToInt();

                        var pricePositionDto = _converter.ConvertToPricePosition(pricePositionEntity);
                        pricePositionDto.BranchType = branchType;
                        pricePositionCollection.Add(pricePositionDto);
                    }

                    // Get prices
                    reader.NextResult();

                    var allPrices = new List<PriceDto>();
                    while (reader.Read())
                    {
                        var priceEntity = new PriceEntity();
                        priceEntity.PricePositionId = reader[nameof(PriceEntity.PricePositionId)].ToInt();
                        priceEntity.DateBeginning = reader[nameof(PriceEntity.DateBeginning)].ToDateTime();
                        priceEntity.PriceSTL = reader[nameof(PriceEntity.PriceSTL)].ToDecimal();
                        priceEntity.PriceModel = reader[nameof(PriceEntity.PriceModel)].ToDecimal();
                        allPrices.Add(_converter.ConvertToPrice(priceEntity));
                    }

                    // Get products
                    reader.NextResult();

                    var allProducts = new List<ProductDto>();
                    while (reader.Read())
                    {
                        var productEntity = new ProductEntity
                        {
                            PricePositionId = reader[nameof(ProductEntity.PricePositionId)].ToInt(),
                            ProductId = reader[nameof(ProductEntity.ProductId)].ToInt(),
                            //BranchType = (BranchType)reader[nameof(ProductEntity.BranchType)].ToInt(),
                        };
                        allProducts.Add(_converter.ConvertToProduct(productEntity));
                    }

                    reader.Close();

                    // Проставляем цены и наборы изделий для каждой ценовой позиции
                    foreach (var pricePositionDto in pricePositionCollection)
                    {
                        pricePositionDto.Prices = allPrices.Where(x => x.PricePositionId == pricePositionDto.PricePositionId).ToArray();
                        pricePositionDto.Products = allProducts.Where(x => x.PricePositionId == pricePositionDto.PricePositionId).ToArray();
                    }

                    return pricePositionCollection.ToArray();
                }
            }
        }

        public ProductDto[] GetProducts()
        {
            var cmdText = "select distinct BranchTypeID, PriceGroupID, PricePositionID, PricePositionCode, MaterialID, MaterialName, ProductID, ProductName, PriceSTL, PriceModel from dbo.GetPriceListForBranch(NULL)";
            using var connection = new SqlConnection(_configuration.ConnectionString);
            connection.Open();

            using var command = new SqlCommand(cmdText, connection);
            using var reader = command.ExecuteReader();
            var products = new List<ProductDto>();
            while (reader.Read())
            {
                var productEntity = new ProductEntity();

                var productId = reader[nameof(ProductEntity.ProductId)];
                if (productId == DBNull.Value)
                    continue;

                productEntity.ProductId = productId.ToInt();
                productEntity.BranchTypeId = (BranchType)reader[nameof(ProductEntity.BranchTypeId)].ToInt();
                productEntity.MaterialId = reader[nameof(ProductEntity.MaterialId)].ToIntOrNull();
                productEntity.MaterialName = reader[nameof(ProductEntity.MaterialName)].ToString();
                productEntity.PriceGroupId = reader[nameof(ProductEntity.PriceGroupId)].ToInt();
                productEntity.PricePositionCode = reader[nameof(ProductEntity.PricePositionCode)].ToString();
                productEntity.PriceStl = reader[nameof(ProductEntity.PriceStl)].ToDecimalOrNull();
                productEntity.PriceModel = reader[nameof(ProductEntity.PriceModel)].ToDecimal();
                productEntity.PricePositionId = reader[nameof(ProductEntity.PricePositionId)].ToInt();
                productEntity.ProductName = reader[nameof(ProductEntity.ProductName)].ToString();

                products.Add(_converter.ConvertToProduct(productEntity));
            }

            return products.ToArray();
        }
    }
}