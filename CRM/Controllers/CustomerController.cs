using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OgsysCRM.DAL;
using OgsysCRM.Models;
using OgsysCRM.Infrastructure;
using OgsysCRM.Models.DataTables;
using System.Data;
using OgsysCRM.Extensions;
using OgsysCRM.Helpers;

namespace OgsysCRM.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IAddressRepo _addressRepo;

        public CustomerController()
        {
            _customerRepo = new CustomerRepo(new AppContext());
            _addressRepo = new AddressRepo(new AppContext());
        }

        [AppAuthorize]
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult CustomerTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest requestModel)
        {
            requestModel.Length = requestModel.Length < Constants.MinLength ? Constants.MinLength : requestModel.Length;
            var customers = _customerRepo.GetAllCustomersForTable(requestModel.Columns.GetSortedColumns().FirstOrDefault(), requestModel.Search);

            var result = new
            {
                draw = requestModel.Draw,
                data = customers.Skip(requestModel.Start).Take(requestModel.Length).Select(x => new
                {
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.CompanyName,
                    x.Email,
                    x.Phone,
                    x.Address
                }).ToArray().Select(x => new
                {
                    x.Id,
                    Name = x.FirstName + " " + x.LastName,
                    x.CompanyName,
                    x.Email,
                    x.Phone,
                    Address = GetAddress(x.Address)
                }),
                recordsFiltered = customers.Count(),
                recordsTotal = customers.Count()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CustomerViewModel();

            return View("Partials/_CustomerForm", model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = _customerRepo.GetCustomerById(id);
            var model = new CustomerViewModel(customer);

            return View("Partials/_CustomerForm", model);
        }

        [HttpPost]
        public ActionResult Customer(CustomerViewModel customerViewModel)
        {
            if (customerViewModel.Customer.Id == 0)
            {
                try
                {
                    var address = customerViewModel.Customer.Address;
                    _addressRepo.InsertAddress(address);
                    _addressRepo.Save();

                    var customer = customerViewModel.Customer;
                    customer.AddressId = address.Id;
                    _customerRepo.InsertCustomer(customer);
                    _customerRepo.Save();

                    return Json(new { msg = "Customer Added!", error = false });
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
                    _addressRepo.UpdateAddress(customerViewModel.Customer.Address);
                    _addressRepo.Save();

                    var customer = customerViewModel.Customer;
                    _customerRepo.UpdateCustomer(customer);
                    _customerRepo.Save();

                    return Json(new { msg = "Customer Updated!", error = false });
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
                var addressId = _customerRepo.GetCustomerById(id).AddressId;
                
                _customerRepo.DeleteCustomer(id);
                _customerRepo.Save();
                _addressRepo.DeleteAddress(addressId);
                _addressRepo.Save();

                return Json(new { msg = "Customer Deleted!", error = false });
            }
            catch (Exception)
            {
                return Json(new { msg = "There was an error!", error = true });
            }
        }

        #region Helper
        private string GetAddress(Address address)
        {
            var fullAddress = "";

            if (address != null)
            {
                fullAddress = address.Address1 + ", " +
                    address.City + ", " +
                    address.State + " " +
                    address.Country + " " +
                    address.Zipcode;
            }

            return fullAddress;
        }
        #endregion
    }
}
