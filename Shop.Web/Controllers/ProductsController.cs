using System.IO;
using Shop.Web.Models;

namespace Shop.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Data.Entities;
    using Helpers;

    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IUserHelper _userHelper;

        public ProductsController(IProductRepository repository, IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _repository = repository;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            if (!ModelState.IsValid)
                return View(product);

            var path = string.Empty;
            if (product.ImageFile != null && product.ImageFile.Length > 0)
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Products", product.ImageFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                    await product.ImageFile.CopyToAsync(stream);
                path = $"~/images/Products/{product.ImageFile.FileName}";
            }

            product.ImageUrl = path;

            //TODO: Pending to change to: this.User.Identity.Name
            product.User = await _userHelper.GetUserByEmailAsync("salas.john@hotmail.com");
            await _repository.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                var path = product.ImageUrl;
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Products", product.ImageFile.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                        await product.ImageFile.CopyToAsync(stream);
                    path = $"~/images/Products/{product.ImageFile.FileName}";
                }

                product.ImageUrl = path;
                //TODO: change for the logged user
                product.User = await _userHelper.GetUserByEmailAsync("salas.john@hotmail.com");
                await _repository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistAsync(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}