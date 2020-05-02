﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Germadent.UserManagementCenter.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;

        public UsersController(IUmcDbOperations umcDbOperations)
        {
            _umcDbOperations = umcDbOperations;
        }

        [HttpGet]
        public UserDto[] GetUsers()
        {
            return _umcDbOperations.GetUsers();
        }

        [HttpPost]
        public UserDto AddUser(UserDto user)
        {
            return _umcDbOperations.AddUser(user);
        }
    }
}