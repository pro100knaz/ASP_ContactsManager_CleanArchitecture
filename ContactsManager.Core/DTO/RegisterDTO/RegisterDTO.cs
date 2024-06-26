﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
	public class RegisterDTO
	{

		[Required(ErrorMessage = "Name can't be blank")]
		public string?	PersonName { get; set; }


		[Required(ErrorMessage = "Email can't be blank")]
		[EmailAddress(ErrorMessage = "Email should be in a preoper emaol address")]
		[Remote(action: "IsEmailAlreadyRegistered" , controller: "AccountController" , ErrorMessage = "Email is already taken")] //Json запрос к методу и его возвращение пользователю в виде валидации
		//если возращает true то валидация прошла иначе ошибка
		//лучше проверять только одну строчки чем ждать remoute запрос соло всеми данные и full reload data в случае неудачи это действительно экономить ресурсы
		public string? Email { get; set; }



		[Required(ErrorMessage = "Phone Number can't be blank")]
		[RegularExpression("^[0-9]*$", ErrorMessage = "Phone number shuold contains only numbers")]
		[DataType(DataType.PhoneNumber)]
		public string? PhoneNumber { get; set; }


		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Password can't be blank")]
		public string? Password { get; set; }


		[DataType(DataType.Password)]
		[Required(ErrorMessage = "ConfirmPassword can't be blank")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
	}
}
