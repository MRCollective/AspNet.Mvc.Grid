using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using NUnit.Framework;
using Rhino.Mocks;
using MvcContrib.UI.Grid.ActionSyntax;
namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridTester
	{
		private List<Person> _people;
		private Grid<Person> _grid;
		private IGridModel<Person> _model;
		private ViewContext _context;

		[SetUp]
		public void Setup()
		{
			_people = new List<Person>();
			_model = new GridModel<Person>();
			_context = new ViewContext();
			_grid = new Grid<Person>(_people, _context);
			_grid.WithModel(_model);
		}

		[Test]
		public void Should_use_custom_renderer()
		{
			var mockRenderer = MockRepository.GenerateMock<IGridRenderer<Person>>();
			mockRenderer.Expect(x => x.Render(null, null, null, null)).IgnoreArguments().Do(new Action<IGridModel<Person>, IEnumerable<Person>, TextWriter, ViewContext>((g, d, w, c) => w.Write("foo")));
			var result = _grid.RenderUsing(mockRenderer).ToString();
			result.ShouldEqual("foo");
		}

		[Test]
		public void Columns_should_be_stored()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).Columns(col => col.For(x => x.Name));
			((IGridModel<Person>)model).Columns.Count.ShouldEqual(1);
		}

		[Test]
		public void Columns_should_be_stored_in_the_order_they_were_defined()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model)
				.Columns(col =>
				{
					col.For(x => x.Id);
					col.For(x => x.Name);
				});

			var cols = ((IGridModel<Person>)model).Columns;
			cols.ElementAt(0).Name.ShouldEqual("Id");
			cols.ElementAt(1).Name.ShouldEqual("Name");
		}

		[Test]
		public void Custom_columns_should_be_added_at_end()
		{
			_grid
				.AutoGenerateColumns()
				.Columns(col => {
					col.For(x => null).Named("Custom");
				});

			var cols = _grid.Model.Columns;
			cols.ElementAt(0).Name.ShouldEqual("Name");
			cols.ElementAt(1).Name.ShouldEqual("DateOfBirth");
			cols.ElementAt(2).Name.ShouldEqual("Id");
			cols.ElementAt(3).DisplayName.ShouldEqual("Custom");
		}

		[Test]
		public void Custom_column_should_be_inserted_when_a_custom_position_is_specified()
		{
			_grid
			.AutoGenerateColumns()
			.Columns(col => {
				col.For(x => null).Named("Custom").InsertAt(1);
			});

			var cols = _grid.Model.Columns;
			cols.ElementAt(0).Name.ShouldEqual("Name");
			cols.ElementAt(1).DisplayName.ShouldEqual("Custom");
			cols.ElementAt(2).Name.ShouldEqual("DateOfBirth");
			cols.ElementAt(3).Name.ShouldEqual("Id");
		}

		[Test]
		public void RowStart_section_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowStart(x=>"foo");
			_grid.ToString();
			((IGridModel<Person>)model).Sections.Row
				.StartSectionRenderer(
					new GridRowViewData<Person>(new Person(), true),
					GridRendererTester.FakeRenderingContext()
				).ShouldBeTrue();
		}

		[Test]
		public void RowEnd_section_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowEnd(x => "foo");
			_grid.ToString();
			((IGridModel<Person>)model).Sections.Row
				.EndSectionRenderer(
					new GridRowViewData<Person>(new Person(), true),
					GridRendererTester.FakeRenderingContext()
				).ShouldBeTrue();

		}

		[Test, Obsolete]
		public void RowStart_action_should_be_stored_when_rendered_old_syntax()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowStart((p) => {});
			_grid.ToString();
			((IGridModel<Person>)model).Sections.Row
				.StartSectionRenderer(
					new GridRowViewData<Person>(new Person(), true),
					GridRendererTester.FakeRenderingContext()
				).ShouldBeTrue();
		}

		[Test, Obsolete]
		public void RowStart_action_context_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowStart((p, x) => {});
			_grid.ToString();
			((IGridModel<Person>)model).Sections.Row
				.StartSectionRenderer(
					new GridRowViewData<Person>(new Person(), true),
					GridRendererTester.FakeRenderingContext()
				).ShouldBeTrue();
		}

		[Test, Obsolete]
		public void RowEnd_actoion_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowEnd((p) => { });
			_grid.ToString();
			((IGridModel<Person>)model).Sections.Row
			.EndSectionRenderer(
				new GridRowViewData<Person>(new Person(), true),
				GridRendererTester.FakeRenderingContext()
			).ShouldBeTrue();
		}


		[Test]
		public void Empty_text_should_be_stored()
		{
			_grid.Empty("Foo");
			_model.EmptyText.ShouldEqual("Foo");
		}

		[Test]
		public void Custom_attributes_should_be_stored()
		{
			var attrs = new Dictionary<string, object> { { "foo", "bar" } };
			_grid.Attributes(attrs);
			_model.Attributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Custom_attributes_should_be_stored_using_lambdas()
		{
			_grid.Attributes(foo => "bar");
			_model.Attributes["foo"].ShouldEqual("bar");
		}


		[Test]
		public void Renders_to_provided_renderer_by_default()
		{
			_grid.Empty("Foo");
			var result = _grid.ToString();
            result.ShouldEqual("<table class=\"grid\"><thead><tr><th></th></tr></thead><tbody><tr><td>Foo</td></tr></tbody></table>");
		}

		[Test]
		public void Should_store_custom_attributes_for_row()
		{
			var attrs = new Hash();
			_grid.RowAttributes(x => attrs);
			_model.Sections.Row.Attributes(null).ShouldBeTheSameAs(attrs);
		}

		[Test]
		public void Should_Store_custom_attributes_for_header_row()
		{
			var attrs = new Hash();
			_grid.HeaderRowAttributes(attrs);
			_model.Sections.HeaderRow.Attributes(null).ShouldBeTheSameAs(attrs);
		}
	}
}