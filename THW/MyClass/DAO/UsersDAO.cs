using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class UsersDAO
    {
        private MyDBContext db = new MyDBContext();

        public Users getItem(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

        // Select * from ...
        public List<Users> getList()
        {
            return db.Users.ToList();
        }

        // select * from cho Index voi status 1, 2
       

        // detail
        public Users getRow(int? id)
        {
            if (id == null) return null;
            else
            {
                return db.Users.Find(id);
            }
        }

        // create
        public int Insert(Users row)
        {
            db.Users.Add(row);
            return db.SaveChanges();
        }

        //cap nhat mau tin
        public int Update(Users row)
        {

            var us = db.Users.FirstOrDefault(x => x.Id == row.Id);
            us.Password = row.Password;
            us.FullName = row.FullName;
            us.Email = row.Email;
            us.Phone = row.Phone;
            us.Address = row.Address;
            us.UpdateBy = row.UpdateBy;
            us.UpdateAt = DateTime.Now;
            return db.SaveChanges();
        }

        public int Login(string email, string Pass)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return -2;
            }
            else
            {
                if (user.Status == false)
                {
                    return 0;
                }
                else
                {
                    if (user.Password == Pass)
                    {
                        return 1;
                    }
                    else return -1;
                }
            }
        }
        // xoa mau tin
        public int Delete(Users row)
        {
            db.Users.Remove(row);
            return db.SaveChanges();
        }
    }
}
