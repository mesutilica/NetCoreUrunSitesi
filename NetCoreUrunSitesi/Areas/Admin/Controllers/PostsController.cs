﻿using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreUrunSitesi.Utils;
using Service.Abstract;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class PostsController : Controller
    {
        private readonly IService<Post> _repository;
        private readonly IService<Category> _categoryRepository;

        public PostsController(IService<Post> repository, IService<Category> categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
        }

        // GET: PostsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: PostsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: PostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Post post, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    post.CreateDate = DateTime.Now;
                    post.Image = await FileHelper.FileLoaderAsync(Image);
                    await _repository.AddAsync(post);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View(post);
        }

        // GET: PostsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View(await _repository.FindAsync(id));
        }

        // POST: PostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Post post, IFormFile? Image, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true) post.Image = string.Empty;
                    if (Image != null) post.Image = await FileHelper.FileLoaderAsync(Image);
                    _repository.Update(post);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(post);
        }

        // GET: PostsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: PostsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _repository.Delete(post);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
