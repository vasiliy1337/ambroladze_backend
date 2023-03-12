using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ambroladze_backend.Controllers
{
    public class LibController : Controller
    {
        // GET: LibController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LibController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibController/Create
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

        // GET: LibController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibController/Edit/5
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

        // GET: LibController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibController/Delete/5
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
