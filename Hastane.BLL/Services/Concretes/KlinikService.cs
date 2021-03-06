﻿using Hastane.BLL.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hastane.BLL.Models;
using Hastane.DAL.DataModel;
using System.Linq.Expressions;
using Hastane.DAL.Repositories.Abstracts;
using Hastane.BLL.Validations;
using FluentValidation.Results;

namespace Hastane.BLL.Services.Concretes
{
    public class KlinikService : IKlinikService

    { private readonly IKlinikRepository _repo;

        public KlinikService(IKlinikRepository repo)
        {
            _repo = repo;
        }
        public MessageResult Create(Klinikler model)
        {
            var _validator = new KlinikAddValidator();
            ValidationResult result = _validator.Validate(model);
            if (result.IsValid)
            {
                _repo.Add(model);
            }
            var m = new MessageResult
            {
                ErrorMessage = result.Errors.Select(x => x.ErrorMessage).ToList(),
                IsSucceed = result.IsValid
            };
            m.SuccessMessage = m.IsSucceed == true ? "Klinik Ekleme İşlemi Başarılı." : "Hatalı bilgiler mevcut";
            return m;
        }

        public MessageResult Delete(int id)
        {
            _repo.Delete(id);
            var m = new MessageResult
            {
                IsSucceed = true
            };
            m.SuccessMessage = m.IsSucceed == true ? "Klinik Silme İşlemi Başarılı." : "Hatalı bilgiler mevcut";
            return m;
        }

        public MessageResult Edit(Klinikler model)
        {
            var kontrol = _repo.GetList(x => x.KlinikID != model.KlinikID && x.KlinikAd == model.KlinikAd).Count;
            if (Convert.ToBoolean(kontrol))
            {
                var msg = new MessageResult();
                msg.ErrorMessage = new List<string> { "Bu Klinik Adıyla bir klinik zaten var." };
                return msg;
            }
        
            else
            {
                var _validator = new KlinikUpdateValidator();
        ValidationResult result = _validator.Validate(model);
                if (result.IsValid)
                {
                    _repo.Update(model);
                }
    var m = new MessageResult
    {
        ErrorMessage = result.Errors.Select(x => x.ErrorMessage).ToList(),
        IsSucceed = result.IsValid
    };
    m.SuccessMessage = m.IsSucceed == true ? "Klinik Güncelleme İşlemi Başarılı." : "Hatalı bilgiler mevcut";
                return m;
            }
        }

        public Klinikler GetKlinikById(int id)
        {
            return _repo.FindById(id);
        }

        public List<Klinikler> KlinikList()
        {
            return _repo.GetList(null);
        }

        public List<Klinikler> KlinikListWithSorgu(Expression<Func<Klinikler, bool>> predicate)
        {
            return _repo.GetList(predicate);
        }
    }

   
    }

