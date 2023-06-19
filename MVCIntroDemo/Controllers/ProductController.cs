using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MVCIntroDemo.Models.Product;
using System.Collections;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace MVCIntroDemo.Controllers
{
	public class ProductController : Controller
	{
		[ActionName("My-Products")]
		public IActionResult All(string keyword)
		{
			if (keyword != null)
			{
				var foundProduct = _products
					.Where(p => p.Name
					               .ToLower()
								   .Contains(keyword.ToLower())
					       );

				return View(foundProduct);
			}
			return View(_products);
		}

		public IActionResult ById(int id)
		{
			var product = _products.FirstOrDefault(x => x.Id == id);

			if (product == null) 
			{
				return NotFound();
			}

			return View(product);
		}

		public IActionResult AllAsJson()
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true
			};

			return Json(_products, options);
		}

		public IActionResult AllAsText()
		{
			//var text = string.Empty;
			//foreach (var product in _products)
			//{
			//	text += ($"Product {product.Id}: {product.Name} - {product.Price} lv. ");
			//	text += Environment.NewLine;

			//}
			//return Content(text);

			StringBuilder sb = new StringBuilder();
			foreach (var product in _products)
			{
				sb.AppendLine ($"Product {product.Id}: {product.Name} - {product.Price} lv. ");
			}

			return Content(sb.ToString());

		}

		public IActionResult AllAsTextFile()
		{
			StringBuilder sb = new StringBuilder();
			foreach (var product in _products)
			{
				sb.AppendLine($"Product {product.Id}: {product.Name} - {product.Price:f2} lv. ");
			}

			Response.Headers.Add(HeaderNames.ContentDisposition,
				@"attachment;filename=products.txt");

			return File(Encoding.UTF8.GetBytes(sb.ToString().TrimEnd()), "text/plain");
		}



		private IEnumerable<ProductViewModel> _products =
			new List<ProductViewModel>()
			{
			new ProductViewModel()
			{
				Id = 1,
				Name = "Cheese",
				Price = 7.00
			},

			new ProductViewModel() 
			{ 
				Id = 2,
				Name = "Ham",
				Price = 5.50
			},

			new ProductViewModel()
			{
				Id = 3,
				Name = "Bread",
				Price = 1.50
			}

			};
		
	}
}
