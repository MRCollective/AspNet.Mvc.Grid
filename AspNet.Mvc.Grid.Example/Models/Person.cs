using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNet.Mvc.Grid.Example.Models
{
	public class Person
	{
		public string Name { get; set; }
		[DisplayName("Name2"), ScaffoldColumn(false)]
		public string NameWithAttribute { get; set; }

		public DateTime DateOfBirth { get; set; }

		public int Id { get; set; }

		[ScaffoldColumn(false), DisplayFormat(DataFormatString = "{0:dd}")]
		public DateTime DateWithAttribute { get; set; }

		[ScaffoldColumn(false)]
		public Address Address { get; set; }
	}

	public class Address
	{
		public string Line1 { get; set; }
	}
}