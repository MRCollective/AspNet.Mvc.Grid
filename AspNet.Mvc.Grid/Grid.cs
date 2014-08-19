using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Defines a grid to be rendered.
	/// </summary>
	/// <typeparam name="T">Type of datasource for the grid</typeparam>
	public class Grid<T> : IGrid<T> where T : class
	{
		private readonly ViewContext _context;
		private IGridModel<T> _gridModel = new GridModel<T>();

		/// <summary>
		/// The GridModel that holds the internal representation of this grid.
		/// </summary>
		public IGridModel<T> Model
		{
			get { return _gridModel; }
		}

		/// <summary>
		/// Creates a new instance of the Grid class.
		/// </summary>
		/// <param name="dataSource">The datasource for the grid</param>
		/// <param name="context"></param>
		public Grid(IEnumerable<T> dataSource, ViewContext context)
		{
			this._context = context;
			DataSource = dataSource;
		}

		/// <summary>
		/// The datasource for the grid.
		/// </summary>
		public IEnumerable<T> DataSource { get; private set; }

		public IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer)
		{
			_gridModel.Renderer = renderer;
			return this;
		}

		public IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder)
		{
			var builder = new ColumnBuilder<T>();
			columnBuilder(builder);

			foreach (var column in builder)
			{
				if (column.Position == null) 
				{
					_gridModel.Columns.Add(column);
				} 
				else
				{
					_gridModel.Columns.Insert(column.Position.Value, column);	
				}
            }

			return this;
		}

		public IGridWithOptions<T> Empty(string emptyText)
		{
			_gridModel.EmptyText = emptyText;
			return this;
		}

		public IGridWithOptions<T> Attributes(IDictionary<string, object> attributes)
		{
			_gridModel.Attributes = attributes;
			return this;
		}

		public IGrid<T> WithModel(IGridModel<T> model)
		{
			_gridModel = model;
			return this;
		}

		public IGridWithOptions<T> Sort(GridSortOptions sortOptions)
		{
			_gridModel.SortOptions = sortOptions;
			return this;
		}

		public IGridWithOptions<T> Sort(GridSortOptions sortOptions, string prefix)
		{
			_gridModel.SortOptions = sortOptions;
			_gridModel.SortPrefix = prefix;
			return this;
		}

		public override string ToString()
		{
			return ToHtmlString();
		}

		public string ToHtmlString()
		{
			var writer = new StringWriter();
			_gridModel.Renderer.Render(_gridModel, DataSource, writer, _context);
			return writer.ToString();
		}

		public IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes)
		{
			_gridModel.Sections.HeaderRowAttributes(attributes);
			return this;
		}

		[Obsolete("The Render method is deprecated. From within a Razor view, use @Html.Grid() without a call to Render.")]
		public void Render()
		{
			_gridModel.Renderer.Render(_gridModel, DataSource, _context.Writer, _context);
		}

		public IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
		{
			_gridModel.Sections.RowAttributes(attributes);
			return this;
		}
	}
}