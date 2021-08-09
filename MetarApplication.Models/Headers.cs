using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MetarApplication.Models
{
	public class Headers
	{
		[FromHeader]
		public int nocache { get; set;}
	}
}