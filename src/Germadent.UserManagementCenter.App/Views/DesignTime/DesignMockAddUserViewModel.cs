﻿using System.IO;
using System.Linq;
using Germadent.Common.FileSystem;
using Germadent.Model;
using Germadent.UI.ViewModels.DesignTime;
using Germadent.UserManagementCenter.App.Mocks;
using Germadent.UserManagementCenter.App.ViewModels;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    public class DesignMockFileManager : IFileManager
    {
        public void SaveAsText(string data, string path)
        {
            throw new System.NotImplementedException();
        }

        public string GetShortFileName(string fullFileName)
        {
            throw new System.NotImplementedException();
        }

        public Stream OpenFileAsStream(string path)
        {
            throw new System.NotImplementedException();
        }

        public string ReadAllText(string path)
        {
            throw new System.NotImplementedException();
        }

        public void OpenFileByOS(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(string path)
        {
            throw new System.NotImplementedException();
        }

        public byte[] ReadAllBytes(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public FileInfo Save(byte[] data, string filePath)
        {
            throw new System.NotImplementedException();
        }

        public FileInfo Save(Stream stream, string filePath)
        {
            throw new System.NotImplementedException();
        }

        public void Copy(string sourceFileName, string destFileName)
        {
            throw new System.NotImplementedException();
        }
    }

    public class DesignMockAddUserViewModel : AddUserViewModel
    {
        public DesignMockAddUserViewModel() 
            : base(new DesignMockUserManagementCenterOperations(), new DesignMockShowDialogAgent(), new DesignMockFileManager())
        {
            Initialize(new UserDto(), ViewMode.Add);

            FirstName = "Еблантий";
            Surname = "Конопаптов";
            Patronymic = "Васисуалиевич";
            Login = "Vasya";
            Password = "123";
            PasswordOnceAgain = "123";
            Description = "Какое то описание какогото юзверя";

            Roles.First().IsChecked = true;

            IsAdmin = true;
            IsOperator = true;
            IsTechnic = true;
            TechnicQualifyingRank = 3;
            IsModeller = true;
            ModellerQualifyingRank = 2;
        }
    }
}
