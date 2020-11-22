using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.WebApi.Entities;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations : IRmaDbOperations
    {
        public PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddPriceGroup", connection))
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
                using (var command = new SqlCommand("UpdatePriceGroup", connection))
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
                using (var command = new SqlCommand("UnionPriceGroups", connection))
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
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddPricePosition", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@pricePositionCode", SqlDbType.NVarChar)).Value = pricePositionDto.UserCode;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int)).Value = pricePositionDto.PriceGroupId;
                    command.Parameters.Add(new SqlParameter("@pricePositionName", SqlDbType.NVarChar)).Value = pricePositionDto.Name;
                    command.Parameters.Add(new SqlParameter("@materialId", SqlDbType.Int)).Value = pricePositionDto.MaterialId;
                    command.Parameters.Add(new SqlParameter("@pricePositionId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    pricePositionDto.PricePositionId = command.Parameters["@pricePositionId"].Value.ToInt();

                    var price = new PriceDto
                    {
                        PricePositionId = pricePositionDto.PricePositionId,
                        //PriceModel = pricePositionDto.PriceModel,
                        //PriceSTL = pricePositionDto.PriceStl
                    };

                    AddPrice(price, connection);

                    return pricePositionDto;
                }
            }
        }

        public PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddPricePosition", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@pricePositionId", SqlDbType.Int)).Value = pricePositionDto.PricePositionId;
                    command.Parameters.Add(new SqlParameter("@pricePositionCode", SqlDbType.NVarChar)).Value = pricePositionDto.UserCode;
                    command.Parameters.Add(new SqlParameter("@priceGroupId", SqlDbType.Int)).Value = pricePositionDto.PriceGroupId;
                    command.Parameters.Add(new SqlParameter("@pricePositionName", SqlDbType.NVarChar)).Value = pricePositionDto.Name;
                    command.Parameters.Add(new SqlParameter("@materialId", SqlDbType.Int)).Value = pricePositionDto.MaterialId;

                    command.ExecuteNonQuery();

                    var price = new PriceDto
                    {
                        PricePositionId = pricePositionDto.PricePositionId,
                        //PriceModel = pricePositionDto.PriceModel,
                        //PriceSTL = pricePositionDto.PriceStl
                    };

                    UpdatePrice(price, connection);

                    return pricePositionDto;
                }
            }
        }

        public DeleteResult DeletePricePosition(int pricePositionId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DeletePricePosition", connection))
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
            var cmdText = string.Format("select distinct PriceGroupID, PriceGroupName from GetPriceListForBranch({0})", (int)branchType);
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
            var cmdText = string.Format("select distinct PricePositionID,PriceGroupID, PricePositionCode, PricePositionName, MaterialID, DateBeginning, PriceSTL, PriceModel from GetPriceListForBranch({0})", (int)branchType);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var pricePositionCollection = new List<PricePositionDto>();
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
                        pricePositionEntity.PriceStl = reader[nameof(pricePositionEntity.PriceStl)].ToDecimal();
                        pricePositionEntity.PriceModel = reader[nameof(pricePositionEntity.PriceModel)].ToDecimal();
                        var materialId = reader[nameof(pricePositionEntity.MaterialId)];
                        if (materialId != DBNull.Value)
                            pricePositionEntity.MaterialId = materialId.ToInt();

                        var pricePositionDto = _converter.ConvertToPricePosition(pricePositionEntity);
                        pricePositionDto.BranchType = branchType;
                        pricePositionCollection.Add(pricePositionDto);
                    }
                    reader.Close();

                    return pricePositionCollection.ToArray();
                }
            }
        }

        public PriceDto[] GetPrices(int branchType)
        {
            var cmdText = string.Format("select distinct PricePositionID, DateBeginning, DateEnd, PriceSTL, PriceModel from GetPriceListForBranch({0})", branchType);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var priceCollection = new List<PriceDto>();
                    while (reader.Read())
                    {
                        var priceEntity = new PriceEntity();
                        priceEntity.PricePositionId = reader[nameof(priceEntity.PricePositionId)].ToInt();
                        priceEntity.DateBeginning = reader[nameof(priceEntity.DateBeginning)].ToDateTime();
                        priceEntity.PriceSTL = reader[nameof(priceEntity.PriceSTL)].ToDecimal();
                        priceEntity.PriceModel = reader[nameof(priceEntity.PriceModel)].ToDecimal();

                        var priceDto = _converter.ConvertToPrice(priceEntity);
                        priceCollection.Add(priceDto);
                    }
                    reader.Close();

                    return priceCollection.ToArray();
                }
            }
        }

        public ProductDto[] GetProductSetForPricePosition(int pricePositionId)
        {
            var cmdText = string.Format("SELECT * FROM GetProductSetForPricePosition({0})", pricePositionId);
            using var connection = new SqlConnection(_configuration.ConnectionString);
            connection.Open();

            using var command = new SqlCommand(cmdText, connection);
            using var reader = command.ExecuteReader();
            var products = new List<ProductDto>();
            while (reader.Read())
            {
                var productEntity = new ProductEntity();

                productEntity.ProductId = reader[nameof(productEntity.ProductId)].ToInt();
                productEntity.PricePositionId = reader[nameof(productEntity.PricePositionId)].ToInt();
                productEntity.ProstheticsName = reader[nameof(productEntity.ProstheticsName)].ToString();

                products.Add(_converter.ConvertToProduct(productEntity));
            }

            return products.ToArray();
        }

        private static void AddPrice(PriceDto price, SqlConnection connection)
        {
            using (var command = new SqlCommand("AddPrice", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@pricePositionId", SqlDbType.Int)).Value = (int)price.PricePositionId;
                command.Parameters.Add(new SqlParameter("@dateBeginning", SqlDbType.Date)).Value = price.DateBeginning.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@priceSTL", SqlDbType.Money)).Value = price.PriceSTL;
                command.Parameters.Add(new SqlParameter("@priceModel", SqlDbType.Money)).Value = price.PriceModel;

                command.ExecuteNonQuery();
            }
        }

        private static void UpdatePrice(PriceDto price, SqlConnection connection)
        {
            using (var command = new SqlCommand("UpdatePrice", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@pricePositionId", SqlDbType.Int)).Value = price.PricePositionId;
                command.Parameters.Add(new SqlParameter("@dateBeginningCurrent", SqlDbType.Date)).Value = price.DateBeginning.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@priceSTL", SqlDbType.Money)).Value = price.PriceSTL;
                command.Parameters.Add(new SqlParameter("@priceModel", SqlDbType.Money)).Value = price.PriceModel;
                //command.Parameters.Add(new SqlParameter("@dateEnd", SqlDbType.Date)).Value = price.DateEnd;

                command.ExecuteNonQuery();
            }
        }

        public ProductDto[] GetProductSetForToothCard(BranchType branchType)
        {
            var cmdText = string.Format("select distinct PriceGroupID, PricePositionID, PricePositionCode, MaterialID, MaterialName, ProstheticsID, ProstheticsName, PriceSTL, PriceModel from GetPriceListForBranch({0})", (int)branchType);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var productSetCollection = new List<ProductDto>();
                    while (reader.Read())
                    {
                        var productSetEntity = new ProductSetForPriceGroupEntity();

                        var productId = reader["ProstheticsID"];
                        if (productId == DBNull.Value)
                            continue;

                        productSetEntity.PriceGroupId = reader[nameof(productSetEntity.PriceGroupId)].ToInt();
                        productSetEntity.PricePositionId = reader[nameof(productSetEntity.PricePositionId)].ToInt();
                        productSetEntity.PricePositionCode = reader[nameof(productSetEntity.PricePositionCode)].ToString();
                        productSetEntity.MaterialId = reader[nameof(productSetEntity.MaterialId)].ToIntOrNull();
                        productSetEntity.MaterialName = reader[nameof(productSetEntity.MaterialName)].ToString();
                        productSetEntity.ProductId = productId.ToInt();
                        productSetEntity.ProductName = reader["ProstheticsName"].ToString();
                        productSetEntity.PriceSTL = reader[nameof(productSetEntity.PriceSTL)].ToDecimal();
                        productSetEntity.PriceModel = reader[nameof(productSetEntity.PriceModel)].ToDecimal();

                        var productSetDto = _converter.ConvertToProductSetForPriceGroup(productSetEntity);
                        productSetCollection.Add(productSetDto);
                    }
                    reader.Close();
                    return productSetCollection.ToArray();
                }
            }
        }
    }
}
