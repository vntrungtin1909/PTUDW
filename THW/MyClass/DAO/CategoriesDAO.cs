﻿using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class CategoriesDAO
    {
        private MyDBContext db = new MyDBContext();

        // Select * from ...
        public List<Categories> getList()
        {
            return db.Categories.ToList();
        }

        // select * from cho Index voi status 1, 2
        public List<Categories> getList(string status = "ALL") // status 0, 1, 2
        {
            List<Categories> list = null;
            switch (status)
            {
                case "Index": // 1, 2
                    {
                        list = db.Categories.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash": // 0
                    {
                        list = db.Categories.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Categories.ToList();
                        break;
                    }
            }
            return list;
        }

        // detail
        public Categories getRow(int? id)
        {
            if (id == null) return null;
            else
            {
                return db.Categories.Find(id);
            }
        }

        // create
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }

        //cap nhat mau tin
        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        // xoa mau tin
        public int Delete(Categories row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }
}
