using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class NeighborhoodsController : Controller
    {
        private readonly INeighborhoodRepository _neighborhoodRepo;

        public NeighborhoodsController(INeighborhoodRepository neighborhoodRepository)
        {
            _neighborhoodRepo = neighborhoodRepository;
        }

        // GET: NeighborhoodsController
        public ActionResult Index()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAllNeighborhoods();
            return View(neighborhoods);
        }

        // GET: NeighborhoodsController/Details/5
        public ActionResult Details(int id)
        {
            Neighborhood neighborhood = _neighborhoodRepo.GetNeighborhoodById(id);
            if (neighborhood == null)
            {
                return NotFound();
            }
            return View(neighborhood);
        }

        // GET: NeighborhoodsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NeighborhoodsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NeighborhoodsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NeighborhoodsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NeighborhoodsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NeighborhoodsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
