using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ContactsController : Controller
    {
        private readonly IRepository<Contact> _repository;

        public ContactsController(IRepository<Contact> repository)
        {
            _repository = repository;
        }

        // GET: ContactsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: ContactsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContactsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Contact contact)
        {
            try
            {
                contact.CreateDate = DateTime.Now;
                await _repository.AddAsync(contact);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(contact);
            }
        }

        // GET: ContactsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: ContactsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contact contact)
        {
            try
            {
                _repository.Update(contact);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(contact);
            }
        }

        // GET: ContactsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: ContactsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contact contact)
        {
            try
            {
                _repository.Delete(contact);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
