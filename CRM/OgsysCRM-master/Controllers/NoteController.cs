using OgsysCRM.DAL;
using OgsysCRM.Extensions;
using OgsysCRM.Helpers;
using OgsysCRM.Infrastructure;
using OgsysCRM.Models;
using OgsysCRM.Models.DataTables;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OgsysCRM.Controllers
{
    public class NoteController : BaseController
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly INoteRepo _noteRepo;
        private readonly IUserRepo _userRepo;

        public NoteController()
        {
            _noteRepo = new NoteRepo(new AppContext());
            _customerRepo = new CustomerRepo(new AppContext());
            _userRepo = new UserRepo(new AppContext());
        }

        [AppAuthorize]
        public ActionResult Index()
        {
            try
            {
                var users = _userRepo.GetUsers().ToList();
                var customers = _customerRepo.GetCustomers().ToList();
                var model = new NoteViewModel();
                model.Users = users;
                model.Customers = customers;

                return View(model);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult NoteTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest requestModel, int? customerFilter, int? userFilter)
        {
            requestModel.Length = requestModel.Length < Constants.MinLength ? Constants.MinLength : requestModel.Length;
            var notes = _noteRepo.GetAllNotesForTable(customerFilter, userFilter, requestModel.Columns.GetSortedColumns().FirstOrDefault(), requestModel.Search);

            var result = new
            {
                draw = requestModel.Draw,
                data = notes.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new
                {
                    x.Id,
                    x.Customer,
                    x.DateCreated,
                    x.Body,
                    x.User
                }).ToArray().Select(x => new
                {
                    x.Id,
                    Customer = x.Customer.FirstName + " " + x.Customer.LastName,
                    DateCreated = x.DateCreated.ToShortDateString(),
                    x.Body,
                    CreatedBy = x.User.FirstName + " " + x.User.LastName
                }),
                recordsFiltered = notes.Count(),
                recordsTotal = notes.Count()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var user = _userRepo.GetUserById(UserID);
            var note = new Note();
            var customers = _customerRepo.GetCustomers().ToList();
            var model = new NoteViewModel(note, customers);
            model.CreatedBy = FirstName + " " + LastName;
            model.DateCreated = DateTime.Now.ToShortDateString();

            return View("Partials/_NoteForm", model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var note = _noteRepo.GetNoteById(id);
            var customers = _customerRepo.GetCustomers().ToList();
            var model = new NoteViewModel(note, customers);
            model.CreatedBy = note.User.FirstName + " " + note.User.LastName;
            model.DateCreated = note.DateCreated.ToShortDateString();

            return View("Partials/_NoteForm", model);
        }

        [HttpPost]
        public ActionResult Note(NoteViewModel noteViewModel)
        {
            if (noteViewModel.Note.Id == 0)
            {
                try
                {
                    var note = noteViewModel.Note;
                    note.UserId = UserID;
                    note.DateCreated = DateTime.Now;
                    _noteRepo.InsertNote(note);
                    _noteRepo.Save();

                    return Json(new { msg = "Note Added!", error = false });
                }
                catch (Exception)
                {
                    return Json(new { msg = "There was an error!", error = true });
                }
            }
            else
            {
                try
                {
                    var note = noteViewModel.Note;
                    _noteRepo.UpdateNote(note);
                    _noteRepo.Save();

                    return Json(new { msg = "Note Updated!", error = false });
                }
                catch (Exception)
                {
                    return Json(new { msg = "There was an error!", error = true });
                }

            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _noteRepo.DeleteNote(id);
                _noteRepo.Save();

                return Json(new { msg = "Note Deleted!", error = false });
            }
            catch (Exception)
            {
                return Json(new { msg = "There was an error!", error = true });
            }
        }
    }
}