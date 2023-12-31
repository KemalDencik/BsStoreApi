﻿using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//bütün servisler burda toplanır
namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUploadFilesService _uploadFilesService;

        public ServiceManager(IBookService bookService, ICategoryService categoryService, IAuthenticationService authenticationService, IUploadFilesService uploadFilesService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _authenticationService = authenticationService;
            _uploadFilesService = uploadFilesService;
        }

        public IBookService BookService => _bookService;

        public IAuthenticationService AuthenticationService => _authenticationService;

        //ne zaman biri bizden bu nesneyi isterse value dönecez public kısım burası 
        public ICategoryService CategoryService => _categoryService;

        public IUploadFilesService UploadFilesService => _uploadFilesService;
    }
}
