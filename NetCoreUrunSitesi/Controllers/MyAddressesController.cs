﻿using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Abstract;
using Service.Concrete;

namespace NetCoreUrunSitesi.Controllers
{
    [Authorize(Policy = "UserPolicy")]
    public class MyAddressesController : Controller
    {
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Address> _serviceAddress;

        public MyAddressesController(IService<AppUser> serviceAppUser, IService<Address> serviceAddress)
        {
            _serviceAppUser = serviceAppUser;
            _serviceAddress = serviceAddress;
        }

        // GET: AddressesController
        public async Task<ActionResult> Index()
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser is null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Lütfen Tekrar Giriş Yapın!");
            }
            var model = _serviceAddress.GetAll(u => u.AppUserId == appUser.Id);

            return View(model);
        }

        // GET: AddressesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddressesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddressesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Address collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                    if (appUser is not null)
                    {
                        collection.AppUserId = appUser.Id;
                        await _serviceAddress.AddAsync(collection);
                        await _serviceAddress.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ModelState.AddModelError("", "Kayıt Başarısız!");
            return View(collection);
        }

        // GET: AddressesController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser is null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Lütfen Tekrar Giriş Yapın!");
            }
            var model = _serviceAddress.Get(x => x.AddressGuid.ToString() == id && x.AppUserId == appUser.Id);
            if (model is null)
                return NotFound("Adres Bilgisi Bulunamadı!");
            return View(model);
        }

        // POST: AddressesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(string id, Address collection)
        {
            try
            {
                var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                if (appUser is null)
                    return NotFound("Kullanıcı Datası Bulunamadı! Lütfen Tekrar Giriş Yapın!");
                var model = _serviceAddress.Get(x => x.AddressGuid.ToString() == id && x.AppUserId == appUser.Id);
                if (model is null)
                {
                    return NotFound("Adres Datası Bulunamadı! Lütfen Tekrar Giriş Yapın!");
                }
                model.Title = collection.Title;
                model.District = collection.District;
                model.City = collection.City;
                model.OpenAddress = collection.OpenAddress;
                model.IsActive = collection.IsActive;
                model.IsBillingAddress = collection.IsBillingAddress;
                model.IsDeliveryAddress = collection.IsDeliveryAddress;
                var otherAddresses = await _serviceAddress.GetAllAsync(x => x.AppUserId == appUser.Id && x.Id != model.Id);
                foreach (var item in otherAddresses)
                {
                    item.IsBillingAddress = !model.IsBillingAddress;
                    item.IsDeliveryAddress = !model.IsDeliveryAddress;
                    _serviceAddress.Update(item);
                }
                _serviceAddress.Update(model);
                await _serviceAddress.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(collection);
        }

        // GET: AddressesController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser is null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Lütfen Tekrar Giriş Yapın!");
            }
            var model = _serviceAddress.Get(x => x.AddressGuid.ToString() == id);
            return View(model);
        }

        // POST: AddressesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, Address address)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapın!");
            }
            var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
                return NotFound("Adres Bilgisi Bulunamadı!");
            try
            {
                _serviceAddress.Delete(model);
                await _serviceAddress.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(model);
        }
    }
}
