using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using MvcContrib.UI.Grid.Syntax;
using NUnit.Framework;
using System.Collections.Generic;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class AutoColumnBuilderTester
	{
		private DataAnnotationsModelMetadataProvider _provider;

		[SetUp]
		public void Setup()
		{
			_provider = new DataAnnotationsModelMetadataProvider();
		}

		[Test]
		public void Should_generate_columns()
		{
			var builder = new AutoColumnBuilder<Person>(_provider);

			builder.Count().ShouldEqual(2);
			builder.First().Name.ShouldEqual("Name");
			builder.Last().Name.ShouldEqual("Id");
		}

		[Test]
		public void Calling_AutoGenerateColumns_should_add_columns()
		{
			IGrid<Person> grid = new Grid<Person>(new Person[0], new ViewContext());
			grid.AutoGenerateColumns();

			((Grid<Person>)grid).Model.Columns.Count.ShouldEqual(2);
		}

		[Test]
		public void Does_not_scaffold_property()
		{
			var buuilder = new AutoColumnBuilder<ScaffoldPerson>(_provider);
			buuilder.Count().ShouldEqual(1);
			buuilder.Single().Name.ShouldEqual("Name");
		}

		[Test]
		public void Uses_custom_displayname()
		{
			var builder = new AutoColumnBuilder<DisplayNamePerson>(_provider);
			builder.Single().DisplayName.ShouldEqual("Foo");
		}

		[Test]
		public void Uses_custom_displayformat()
		{
			var builder = new AutoColumnBuilder<DisplayFormatPerson>(_provider);
			var date = new DateTime(2010, 1, 15);
			var person = new DisplayFormatPerson
			{
				DateOfBirth = date
			};

			builder.Single().GetValue(person).ShouldEqual(date.ToString("d"));
		}

		[Test]
		public void Supports_adding_additional_columns()
		{
			var grid = new Grid<Person>(new List<Person>(), new ViewContext());
			grid
				.AutoGenerateColumns()
				.Columns(column => {
					column.For(x => null).Named("Some custom column");
				});

			grid.Model.Columns.Count.ShouldEqual(3);

		}

		private class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}

		private class ScaffoldPerson
		{
			[ScaffoldColumn(false)]
			public int Id { get; set; }

			public string Name { get; set; }
		}

		private class DisplayNamePerson
		{
			[Display(Name = "Foo")]
			public string Name { get; set; }
		}

		private class DisplayFormatPerson
		{
			[DisplayFormat(DataFormatString = "{0:d}")]
			public DateTime DateOfBirth { get; set; }
		}
	}
}