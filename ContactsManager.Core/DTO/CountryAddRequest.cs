﻿using Enities;

namespace ServiceContracts.DTO
{
	public class CountryAddRequest
	{
		public string? CountryName { get; set; }

		public Country ToCountry()
		{
			return new Country()
			{
				Name = CountryName,
			};


		}
	}
}
